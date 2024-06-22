namespace RealEstateApp.Core.Application.DTOs.Account.Base
{
    public class BaseResponse
    {
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
