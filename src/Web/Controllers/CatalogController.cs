using Amazon.S3;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Ardalis.GuardClauses;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using System.Diagnostics;
using System.Security.Policy;
using Web.BindingModels;
using Web.Configuration;
using Web.Features.Category;
using Web.ViewModels;

namespace Web.Controllers
{
    public class CatalogController : BaseController
    {
        const string SessionKey = "_Filters";
        private readonly CatalogContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(CatalogContext context,
            IMediator mediator,
            ILogger<CatalogController> logger)    
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("[Controller]")]
        [HttpGet("[Controller]/{pageId}")]
        public async Task<IActionResult> Catalog(int pageId = 1)
        {
            var filters = GetFilters();

            var products = _context.Products
                .Where(p => !p.IsDeleted && p.Price <= filters.MaxPrice
                && p.Price >= filters.MinPrice && p.QuantityInStock > 0);

            if (filters.Manufacturers != null && filters.Manufacturers.Any())
            {
                var allowedBrands = await _context.Manufacturers
                    .Where(m => filters.Manufacturers.Contains(m.Name))
                    .Select(m => m.Id)
                    .ToListAsync();

                products = products.Where(p => allowedBrands.Contains(p.ManufacturerId ?? -1));
            }
            
            var list = new List<Product>();
            foreach (var product in products.Include(p => p.Categories))
            {
                bool include = true;

                if (filters.Categories != null)
                foreach (var cat in filters.Categories)
                {
                    if (product.Categories != null)
                    if (!product.Categories.Select(c => c.Name).Contains(cat))
                    {
                        include = false;
                    }
                }

                if (include)
                {
                    list.Add(product);
                }
            }

            List<Product> page = new List<Product>();
            int total = 0;

            try
            {
                if (products != null && products.Any())
                {
                    page = GetPage(list, pageId);
                    total = list.Count;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("/Error");
            }
            
            var brands = await _context.Manufacturers
                .Select(m => m.Name)
                .ToListAsync();
            var categories = await _context.Categories
                .Select(c => c.Name)
                .ToListAsync();

            var model = new CatalogViewModel()
            {
                Products = page,
                CurrentPage = pageId,
                NumberOfPages = total / 35 + 1,
                TotalAmount = total,
                Brands = brands,
                Categories = categories
            };

            return View(model);
        }

        [Route("[Controller]/[Action]/{searchString}")]
        public async Task<IActionResult> Search(string searchString, int pageId = 1)
        {
            var filters = GetFilters();

            var products = _context.Products
                .Where(p => !p.IsDeleted && p.Price <= filters.MaxPrice
                && p.Price >= filters.MinPrice && p.QuantityInStock > 0);

            if (filters.Manufacturers != null && filters.Manufacturers.Any())
            {
                var allowedBrands = await _context.Manufacturers
                    .Where(m => filters.Manufacturers.Contains(m.Name))
                    .Select(m => m.Id)
                    .ToListAsync();

                products = products.Where(p => allowedBrands.Contains(p.ManufacturerId ?? -1));
            }

            var brands = await _context.Manufacturers
                .Select(m => m.Name)
                .ToListAsync();
            var categories = await _context.Categories
                .Select(c => c.Name)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    (p.NormalizedName!.Contains(searchString.ToUpper()) || p.NormalizedVendorCode!.Contains(searchString.ToUpper()))
                    && !p.IsDeleted);
            }

            var list = new List<Product>();
            foreach (var product in products.Include(p => p.Categories))
            {
                bool include = true;

                if (filters.Categories != null)
                foreach (var cat in filters.Categories)
                {
                    if (product.Categories != null)
                    if (!product.Categories.Select(c => c.Name).Contains(cat))
                    {
                        include = false;
                    }
                }

                if (include)
                {
                    list.Add(product);
                }
            }

            var model = new CatalogViewModel();
            int total = 0;

            if (list != null && list.Any())
            {
                var page = new List<Product>();

                if (list != null)
                {
                    page = GetPage(list, pageId);
                    total = list.Count;
                }

                model.Products = page;
                model.CurrentPage = pageId;
                model.TotalAmount = total;
            }
            else
            {
                model.Products = new List<Product>();
                model.CurrentPage = 1;
                model.TotalAmount = total;
            }

            model.Brands = brands;
            model.Categories = categories;

            return View(nameof(Catalog), model);
        }

        public async Task<IActionResult> Product(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Categories)
                .Include(p => p.Images)
                .FirstAsync();

            if (product != null)
            {
                
                var model = new ProductViewModel()
                {
                    Product = product,
                };

                var manufacturer = await _context.Manufacturers.Where(m => product.Id == m.Id).FirstOrDefaultAsync();
                if (manufacturer == null)
                    model.Manufacturer = "Empty";
                else
                    model.Manufacturer = manufacturer.Name;

                return View(model);
            }

            return BadRequest();
        }

        [HttpGet("[controller]/[Action]/{category}")]
        public async Task<IActionResult> Category(string category)
        {
            var model = await _mediator.Send(new GetCategory(category.ToUpper()));

            return View("Index", model);
        }

        [HttpGet("[controller]/[Action]")]
        public IActionResult RemoveFilters()
        {
            HttpContext.Session.Remove(SessionKey);
            return Ok();
        }

        [HttpPost]
        public IActionResult SetFilters([FromBody] SetFiltersCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var fg = new FilterAggregate()
            {
                MinPrice = cmd.MinPrice,
                MaxPrice = cmd.MaxPrice,
                Categories = cmd.Categories,
                Manufacturers = cmd.Brands
            };

            HttpContext.Session.Set<FilterAggregate>(SessionKey, fg);

            return Ok();
        }

        public FilterAggregate GetFilters()
        {
            FilterAggregate? fg = HttpContext.Session.Get<FilterAggregate>(SessionKey);

            if (fg == null)
            {
                fg = new FilterAggregate();
                HttpContext.Session.Set<FilterAggregate>(SessionKey, fg);
            }

            return fg;
        }

        public static List<Product> GetPage(List<Product> products, int pageId)
        {
            int index = (pageId - 1) * 35;

            var page = new List<Product>();
            if (index < products.Count)
            {
                for (int i = 35; i > 0; i--)
                {
                    if (index + i <= products.Count)
                    {
                        page = products.GetRange(index, i);
                        return page;
                    }
                }
            }

            return page;
        }
    }
}
