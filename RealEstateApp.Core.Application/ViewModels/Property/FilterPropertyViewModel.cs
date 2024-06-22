namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class FilterPropertyViewModel
    {
        public string? Code { get; set; }
        public string? TypeOfProperty { get; set; }
        public string? TypeOfSale { get; set; }
        public int? TypeOfPropertyId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? NumberOfRooms { get; set; }
        public int? NumberOfBathrooms { get; set; }
    }
}
