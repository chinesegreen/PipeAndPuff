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

        public int NumberOfPages
        {
            get
            {
                return Products.Count / 32 + 1;
            }
            private set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                NumberOfPages = value;
            }
        }

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
