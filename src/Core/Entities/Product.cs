using Ardalis.GuardClauses;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product : IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public string NormalizedName { get; set; }
        public string VendorCode { get; set; }
        public string NormalizedVendorCode { get; set; }
        public int Price { get; set; }
        public int? PriceWithoutDiscount { get; set; }
        public string? Description { get; set; }
        public int? ManufacturerId { get; set; }
        public string Picture { get; set; }
        public List<ImageObj>? Images { get; set; }
        public List<Category>? Categories { get; set; }
        public bool IsTrending { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int QuantityInStock { get; set; } = 0;
        public int ValueTax { get; set; }
        public Dimensions? Dimensions { get; set; }

        public bool IsInStock()
        {
            return QuantityInStock > 0;
        }

        public void AddQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            QuantityInStock += quantity;
        }

        public void SetQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            QuantityInStock = quantity;
        }
    }
}
