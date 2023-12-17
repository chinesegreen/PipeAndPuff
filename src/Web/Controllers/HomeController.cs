using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
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

            var all = await _context.Products.Where(p => !p.IsDeleted).OrderBy(p => p.Date).ToListAsync();

            var trending = all.Where(p => p.IsTrending).ToList();

            if (all.Count > 8)
            {
                all = all.GetRange(all.Count - 8, 8);
            }

            if (trending.Count > 4)
            {
                trending = trending.GetRange(trending.Count - 4, 4);
            }

            var model = new IndexViewModel()
            {
                Trendings = trending,
                RecentArrivals = all
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