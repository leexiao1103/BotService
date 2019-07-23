using System.Net;

namespace BotService.Model.API
{
    public class APIResult<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
    }
}
