using System.Net;

namespace PrisApi.Models.Responses
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Result { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}