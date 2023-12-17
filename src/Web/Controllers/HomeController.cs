using Core.Entities.ShowcaseAggregate;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Security.Cryptography;
using Web.Configuration;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly CatalogContext _context;

        public HomeController(
            CatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var all = await _context.Products.Where(p => !p.IsDeleted && p.QuantityInStock > 0).OrderBy(p => p.Date).ToListAsync();

            var trending = all.Where(p => p.IsTrending).ToList();

            if (all.Count > 8)
            {
                all = all.GetRange(all.Count - 8, 8);
            }

            if (trending.Count > 4)
            {
                trending = trending.GetRange(trending.Count - 4, 4);
            }

            ShowcaseTemplate template;
            try
            {
                template = await _context.Showcases
                    .Include(s => s.Main)
                    .Include(s => s.Mone)
                    .Include(s => s.Mtwo)
                    .Include(s => s.Sone)
                    .Include(s => s.Stwo)
                    .Include(s => s.Sthree)
                    .Include(s => s.Sfour).OrderBy(s => s.Id).LastAsync();
            }
            catch (Exception ex)
            {
                template = new ShowcaseTemplate()
                {
                    Main = new ShowcaseBlock(),
                    Mone = new ShowcaseBlock(),
                    Mtwo = new ShowcaseBlock(),
                    Sone = new ShowcaseBlock(),
                    Stwo = new ShowcaseBlock(),
                    Sthree = new ShowcaseBlock(),
                    Sfour = new ShowcaseBlock()
                };
            
                _context.Add(template);
                await _context.SaveChangesAsync();

                template = await _context.Showcases
                    .Include(s => s.Main)
                    .Include(s => s.Mone)
                    .Include(s => s.Mtwo)
                    .Include(s => s.Sone)
                    .Include(s => s.Stwo)
                    .Include(s => s.Sthree)
                    .Include(s => s.Sfour).OrderBy(s => s.Id).LastAsync();
            }

            var model = new IndexViewModel()
            {
                Trendings = trending,
                RecentArrivals = all,
                Template = template
            };

            return View(model);
        }

        public IActionResult AgeCheck([FromQuery] bool confirm)
        {
            if (confirm)
            {
                AgeConfirmationExtensions.ConfirmAge(HttpContext);

                return Ok();
            }
            else
            {
                return Redirect("/Denied");
            }
        }

        public IActionResult Denied()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}