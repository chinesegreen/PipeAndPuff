using Core.Entities;
using Core.Entities.ShowcaseAggregate;
using Web.Configuration;

namespace Web.ViewModels
{
    public class IndexViewModel
    {
        public List<Product> Trendings { get; set; }
        public List<Product> RecentArrivals { get; set; }
        public ShowcaseTemplate Template { get; set; }
    }
}
