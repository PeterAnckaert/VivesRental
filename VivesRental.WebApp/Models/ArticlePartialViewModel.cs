using System;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class ArticlePartialViewModel
    {
        public Article Article { get; set; }
        public Guid CurrentArticleId { get; set; }
    }
}
