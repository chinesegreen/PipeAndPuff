using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    [Authorize("Admin")]
    //[ValidateAntiForgeryToken]
    public class BaseAdminController : BaseController
    {
        protected readonly CatalogContext _context;
        protected readonly ILocalStorageService _storage;

        public BaseAdminController(CatalogContext context,
            ILocalStorageService storage)
        {
            _context = context;
            _storage = storage;
        }
        protected async Task<int?> AddManufacturer(string? name)
        {
            if (name == null)
            {
                return null;
            }

            try
            {
                var manufacturer = await _context.Manufacturers
                    .Where(m => m.NormalizedName == name.ToUpper()).FirstAsync();

                if (manufacturer == null)
                {
                    manufacturer = new Manufacturer()
                    {
                        Name = name,
                        NormalizedName = name.ToUpper()
                    };

                    _context.Add(manufacturer);
                }

                await _context.SaveChangesAsync();

                return manufacturer.Id;
            }
            catch (Exception e)
            {
                var manufacturer = new Manufacturer()
                {
                    Name = name,
                    NormalizedName = name.ToUpper()
                };

                 _context.Add(manufacturer);

                await _context.SaveChangesAsync();

                return manufacturer.Id;
            }
        }
    }
}
