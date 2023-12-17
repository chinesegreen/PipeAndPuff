using Amazon.S3;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.Identity.Client.Extensions.Msal;
using Web.Configuration;

namespace Web.ViewModels
{
    public class CatalogViewModel
    {
        public List<Product> Products { get; set; }
        public decimal TotalAmount { get; set; }
        public List<string> Brands { get; set; }
        public List<string> Categories { get; set; }

        public List<string> GetCategories()
        {
            return Categories.GroupBy(x => x).Select(x => x.First()).ToList();
        }
        public List<string> GetBrands()
        {
            return Brands.GroupBy(x => x).Select(x => x.First()).ToList();
        }

        public int NumberOfPages { get; set; }

        public int CurrentPage { get; set; }

        public int CalculatePrevious()
        {
            if (CurrentPage < 2)
            {
                return 1;
            }
            else
            {
                return CurrentPage - 1;
            }
        }

        public int CalculateNext()
        {
            if (CurrentPage > NumberOfPages - 1)
            {
                return NumberOfPages;
            }
            else
            {
                return CurrentPage + 1;
            }
        }
    }
}
