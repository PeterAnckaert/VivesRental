namespace VivesRental.WebApp.Models
{
    public class CommonViewModel
    {
        public SortKey SortKey { get; set; }
        public string Error { get; set; }
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
