using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class ArticleViewModel : CommonViewModel
    {
        public IOrderedEnumerable<Article> Articles { get; set; }
        public Article CurrentArticle { get; set; }
        public IOrderedEnumerable<Product> Products { get; set; }
        public IOrderedEnumerable<OrderLine> OrderLines { get; set; }
    }
}
