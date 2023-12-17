using Ardalis.GuardClauses;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using SixLabors.ImageSharp.Memory;
using System.Diagnostics;
using System.Security.Claims;
using Web.BindingModels;
using Web.Configuration;
using Web.ViewModels;

namespace Web.Controllers
{
    // TODO: This class looks terrible and needs to be remade and distributed
    public class AdminController : BaseAdminController
    {
        public AdminController(
            CatalogContext context,
            ILocalStorageService service) : base(context, service) { }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.OrderBy(p => p.Date).ToListAsync();

            var model = new CatalogViewModel()
            {
                Products = products
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] ProductsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            foreach (var id in model.Ids)
            {
                var product = _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();

                if (product != null)
                {
                    // TODO: Raises IOException: the client reset the request stream
                    //System.IO.File.Delete($"wwwroot/{product.Picture}");

                    var images = await _context.Images.Where(i => i.ProductId == product.Id).ToListAsync();

                    if (images != null && images.Any())
                    {
                        foreach (var image in images)
                        {
                            _context.Remove(image);
                            //System.IO.File.Delete($"wwwroot{image.Image}");
                        }
                    }

                    _context.Remove(product);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest();
                }
            }

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] ProductsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            foreach (var id in model.Ids)
            {
                var product = _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();

                if (product == null)
                {
                    return BadRequest();
                }

                product.IsDeleted = true;

                await _context.SaveChangesAsync();
            }

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        public async Task<IActionResult> Restore([FromBody] ProductsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            foreach (var id in model.Ids)
            {
                var product = _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();

                if (product == null)
                {
                    return NotFound();
                }

                product.IsDeleted = false;

                await _context.SaveChangesAsync();
            }

            return Redirect("/Admin/Products");
        }

        [HttpGet("[controller]/[action]/{searchString}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var products = from p in _context.Products
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.NormalizedVendorCode!
                    .Contains(searchString.ToUpper()));
            }

            var viewProducts = await products.ToListAsync();

            var model = new CatalogViewModel()
            {
                Products = viewProducts
            };

            return View(nameof(Products), model);
        }
    }
}
