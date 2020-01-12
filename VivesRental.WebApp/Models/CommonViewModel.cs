using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VivesRental.WebApp.Models
{
    public class CommonViewModel
    {
        public SortKey SortKey { get; set; }
        public string Error { get; set; }
        public bool IsErrorShown { get; set; }
    }

    public enum SortKey
    {
        Unsorted,
        ArticleStatusAsc,
        ArticleStatusDesc,
        DescriptionAsc,
        DescriptionDesc,
        EmailAsc,
        EmailDesc,
        ExpiresAtAsc,
        ExpiresAtDesc,
        FirstNameAsc,
        FirstNameDesc,
        LastNameAsc,
        LastNameDesc,
        NameAsc,
        NameDesc,
        ManufacturerAsc,
        ManufacturerDesc,
        PhoneNumberAsc,
        PhoneNumberDesc,
        PublisherAsc,
        PublisherDesc,
    }


}
