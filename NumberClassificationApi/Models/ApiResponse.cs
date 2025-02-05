using System.Net;

namespace NumberClassificationApi.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
