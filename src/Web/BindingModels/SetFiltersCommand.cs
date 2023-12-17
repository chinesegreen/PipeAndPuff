using System.ComponentModel.DataAnnotations;

namespace Web.BindingModels
{
    public class SetFiltersCommand
    {
        [Range(0, 100000)]
        public int MinPrice { get; set; } = 0;
        [Range(0, 100000)]
        public int MaxPrice { get; set; } = 100000;
        public List<string>? Brands { get; set; }
        public List<string>? Categories { get; set; }
    }
}
