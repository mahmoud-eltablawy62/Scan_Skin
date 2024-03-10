namespace ScanSkin.Api.Errors
{
    public class ApiExcaptionResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiExcaptionResponse
            (int statusCode, string? massege = null, string? details = null) :
            base(statusCode, massege)
        {
            Details = details;
        }
    }
}


