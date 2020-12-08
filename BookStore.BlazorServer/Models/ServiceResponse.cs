using System.Net;

namespace BookStore.BlazorServer.Models
{
    public class ServiceResponse<T>
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Content { get; set; }
    }
}