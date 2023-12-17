using Core.Entities;

namespace Web.BindingModels
{
    public class EditProductCommand
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int? PriceWithoutDiscount { get; set; }
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }
        public int QuantityInStock { get; set; } = 0;
        public List<string>? Categories { get; set; }
        public int ValueTax { get; set; }
        public bool? IsTrending { get; set; }
        public int? Weight { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public List<string>? GetCategories()
        {
            if (Categories == null) return null;

            return Categories.GroupBy(x => x).Select(x => x.First()).ToList();
        }
    }
}
