using System.Linq;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class ProductViewModel : CommonViewModel
    {
        public IOrderedEnumerable<Product> Products { get; set; }
        public Product CurrentProduct { get; set; }
    }
}
