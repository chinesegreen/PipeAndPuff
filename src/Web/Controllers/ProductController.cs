using Amazon.S3.Model.Internal.MarshallTransformations;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Web.BindingModels;
using Web.ViewModels;

namespace Web.Controllers
{
    [Route("Admin/[Controller]/[Action]")]
    public class ProductController : BaseAdminController
    {
        public ProductController(
            CatalogContext context,
            ILocalStorageService service) : base(context, service) { }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetQuantity([FromBody] SetQuantityCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = await _context.Products
                .Where(p => p.Id == cmd.Id)
                .FirstOrDefaultAsync();

            if (product != null)
                product.SetQuantity(cmd.Quantity);
            else
                return BadRequest();

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = new Product()
            {
                Name = cmd.Name,
                NormalizedName = cmd.Name.ToUpper(),
                Price = cmd.Price,
                PriceWithoutDiscount = cmd.PriceWithoutDiscount,
                Description = cmd.Description,
                ManufacturerId = await AddManufacturer(cmd.Manufacturer),
                VendorCode = cmd.VendorCode,
                NormalizedVendorCode = cmd.VendorCode.ToUpper(),
                ValueTax = cmd.ValueTax,
                IsTrending = cmd.IsTrending ?? false,
                QuantityInStock = 0,
                Dimensions = new Dimensions()
                {
                    Width = cmd.Width,
                    Height = cmd.Height,
                    Length = cmd.Length,
                    Weight = cmd.Weight
                },
                Picture = await _storage.SaveFile(cmd.Picture, "products"),
            };

            if (cmd.Categories != null && cmd.Categories.Any())
            {
                product.Categories = GetCategories(cmd.Categories)
                    .Select(c => new Category()
                    {
                        Name = c,
                        NormalizedName = c.ToUpper()
                    }).ToList();
            }

            if (cmd.Images != null && cmd.Images.Any())
            {
                var images = new List<ImageObj>();

                for (int i = 0; i < cmd.Images.Count; i++)
                {
                    var image = cmd.Images[i];

                    var imageObj = new ImageObj()
                    {
                        Position = i + 1,
                        Image = await _storage.SaveFile(image, "products")
                    };

                    images.Add(imageObj);
                }

                product.Images = images;
            }

            _context.Add(product);

            await _context.SaveChangesAsync();

            return Redirect("/Admin/Products");
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Edit(int productId)
        {
            var product = await _context.Products
                .Where(p => p.Id == productId)
                .Include(p => p.Categories)
                .Include(p => p.Images)
                .Include(p => p.Dimensions)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            var model = new ProductViewModel()
            {
                Product = product
            };

            var manufacturer = await _context.Manufacturers.Where(m => product.ManufacturerId == m.Id).FirstOrDefaultAsync();
            if (manufacturer == null)
                model.Manufacturer = "Empty";
            else
                model.Manufacturer = manufacturer.Name;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditProductCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = _context.Products.Where(p => p.Id == cmd.Id)
                .Include(p => p.Categories)    
                .FirstOrDefault();

            if (product != null)
            {
                product.Name = cmd.Name;
                product.NormalizedName = cmd.Name.ToUpper();
                product.Price = cmd.Price;
                product.PriceWithoutDiscount = cmd.PriceWithoutDiscount;
                product.Description = cmd.Description;
                product.ManufacturerId = await AddManufacturer(cmd.Manufacturer);
                product.VendorCode = cmd.VendorCode;
                product.NormalizedVendorCode = cmd.VendorCode.ToUpper();
                product.ValueTax = cmd.ValueTax;
                product.IsTrending = cmd.IsTrending ?? false;
                product.Dimensions = new Dimensions()
                {
                    Width = cmd.Width,
                    Height = cmd.Height,
                    Length = cmd.Length,
                    Weight = cmd.Weight
                };
                if (cmd.Categories != null && cmd.Categories.Any())
                {
                    if (product.Categories != null)
                    {
                        foreach (var cat in product.Categories)
                        {
                            _context.Remove(cat);
                        }
                    }

                    await _context.SaveChangesAsync();

                    product.Categories = GetCategories(cmd.Categories)
                        .Select(c => new Category()
                        {
                            Name = c,
                            NormalizedName = c.ToUpper()
                        }).ToList();
                }
            }
            else
            {
                return BadRequest();
            }

            await _context.SaveChangesAsync();

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        public async Task<IActionResult> EditImage([FromForm] EditImageCommand cmd)
        {
            var product = _context.Products.Where(p => p.Id == cmd.Id)
                    .FirstOrDefault();

            if (product != null)
            {
                if (cmd.Position == 0)
                {
                    product.Picture = await _storage.SaveFile(cmd.Replacer, "products");
                }
                else
                {
                    var image = await _context.Images
                        .Where(i => i.ProductId == cmd.Id && i.Position == cmd.Position)
                        .FirstOrDefaultAsync();

                    if (image != null)
                    {
                        image!.Image = await _storage.SaveFile(cmd.Replacer, "products");
                    }
                }
            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage([FromForm] DeleteImageCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = await _context.Products.Where(p => p.Id == cmd.Id).FirstAsync();

            if (product != null)
            {
                if (cmd.Position == 0)
                {
                    return BadRequest();
                }
                else
                {
                    var image = await _context.Images
                        .Where(i => i.ProductId == cmd.Id && i.Position == cmd.Position)
                        .FirstOrDefaultAsync();

                    if (image != null)
                    {
                        _context.Remove(image);

                        var images = await _context.Images
                            .Where(i => i.ProductId == cmd.Id && i.Position > cmd.Position)
                            .ToListAsync();

                        if (images != null && images.Any())
                        {
                            foreach (var img in images)
                            {
                                img.Position -= 1;
                            }
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddImage([FromForm] AddImageCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var image = new ImageObj()
            {
                Image = await _storage.SaveFile(cmd.Image, "products"),
                ProductId = cmd.Id
            };

            var images = await _context.Images
            .Where(i => i.ProductId == cmd.Id)
            .ToListAsync();

            var positions = images.Select(i => i.Position).ToList();
            positions.Sort();
            
            for (int i = 1; i <= positions.Count; i++)
            {
                if (i != positions[i-1])
                {
                    image.Position = i;
                    return Ok();
                }
            }

            if (positions.Any())
            {
                image.Position = positions.Last() + 1;
            }
            else
            {
                image.Position =  + 1;
            }

            _context.Add(image);

            await _context.SaveChangesAsync();

            return Ok();
        }

        public List<string>? GetCategories(List<string>? categories)
        {
            if (categories == null) return null;

            return categories.GroupBy(x => x).Select(x => x.First()).ToList();
        }
    }
}
