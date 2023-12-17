using Amazon.S3.Model;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Web.ViewModels;

namespace Web.Features.Category
{
    public class GetCategoryHandler : IRequestHandler<GetCategory, CatalogViewModel>
    {
        private readonly CatalogContext _context;

        public GetCategoryHandler(IReadRepository<Product> repository,
            CatalogContext context)
        {
            _context = context;
        }

        public async Task<CatalogViewModel> Handle(GetCategory request,
            CancellationToken cancellationToken)
        {
            var entries = await _context.Categories
                .Where(c => c.NormalizedName == request.Category).ToListAsync();

            List<Product> products = new List<Product>();

            if (entries != null && entries.Any())
            {
                foreach (var category in entries)
                {
                    products.AddRange(category.Products!.Where(p => !p.IsDeleted));
                }
            }

            return new CatalogViewModel()
            {
                Products = products
            };
        }
    }
}
