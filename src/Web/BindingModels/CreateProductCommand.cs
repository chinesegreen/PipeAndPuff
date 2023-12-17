using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.BindingModels
{
    public class CreateProductCommand
    {
        public int Price { get; set; }
        public int? PriceWithoutDiscount { get; set; }
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }
        public List<string>? Categories { get; set; }
        public int ValueTax { get; set; }
        public bool? IsTrending { get; set; } = false;
        public IFormFile Picture { get; set; }
        public List<IFormFile>? Images { get; set; }
        public int? Weight { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
