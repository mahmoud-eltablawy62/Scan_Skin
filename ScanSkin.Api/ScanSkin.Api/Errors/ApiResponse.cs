using Microsoft.AspNetCore.Http;

namespace ScanSkin.Api.Errors
{
    public class ApiResponse { 
    public int StatusCode { get; set; }
    public string? Massege { get; set; }

    public ApiResponse(int statusCode, string? massege = null)
    {
        StatusCode = statusCode;
        Massege = massege ?? GetDefaultMessageForStatusCode(statusCode);
    }

    private string? GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "BadRequest",
            401 => "Authorized",
            404 => "Resource Was Not Found",
            500 => "Internal Server Error",
            _ => null
        };

    }
}
}