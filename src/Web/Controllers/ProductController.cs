using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
                Categories = (cmd.GetCategories() ?? new List<string>() { "Empty" })
                    .Select(c => new Category()
                    {
                        Name = c,
                        NormalizedName = c.ToUpper()
                    }).ToList()
            };

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

        [HttpGet]
        [Route("[Action]/{productId}")]
        public async Task<IActionResult> Edit(int productId)
        {
            var product = await _context.Products
                .Where(p => p.Id == productId)
                .Include("Categories").FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            product.Images = await _context.Images
                    .Where(i => i.ProductId == product.Id).ToListAsync();

            product.Dimensions = await _context.Dimensions
                .Where(d => d.Id == productId).FirstOrDefaultAsync();

            var model = new ProductViewModel()
            {
                Product = product
            };

            return View(model);
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> Edit([FromForm] EditProductCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = _context.Products.Where(p => p.Id == cmd.Id)
                    .FirstOrDefault();

            if (product != null)
            {
                product.SetQuantity(cmd.QuantityInStock);

                product.Name = cmd.Name;
                product.NormalizedName = cmd.Name.ToUpper();
                product.Price = cmd.Price;
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
                product.Categories = (cmd.GetCategories() ?? new List<string>() { "Empty" })
                    .Select(c => new Category()
                    {
                        Name = c,
                        NormalizedName = c.ToUpper()
                    }).ToList();
            }
            else
            {
                return BadRequest();
            }

            await _context.SaveChangesAsync();

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        public async Task<IActionResult> EditImage([FromBody] EditImageCommand cmd)
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

            return Redirect("/Admin/Products");
        }
    }
}
