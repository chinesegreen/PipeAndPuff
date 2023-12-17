using MediatR;
using Web.ViewModels;

namespace Web.Features.Category
{
    public class GetCategory : IRequest<CatalogViewModel>
    {
        public string Category { get; set; }

        public GetCategory(string category)
        {
            Category = category;
        }
    }
}
