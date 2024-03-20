using System.Net;
using System.Text.Json.Serialization;

namespace textileManagment.Domain.Helper
{
    public record Response<T>
    {

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HttpStatusCode StatusCode { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<KeyValuePair<string, string>> Errors { get; set; }

        public static Response<T> NotFound(string message)
        {
            return new Response<T>() { StatusCode = HttpStatusCode.NotFound, Message = message };
        }

        public static Response<T> BadRequest(string message)
        {
            return new Response<T>() { StatusCode = HttpStatusCode.BadRequest, Message = message };
        }
        public static Response<T> Ok(string message, T data, object miscData = null)
        {
            return new Response<T>() { StatusCode = HttpStatusCode.OK, Data = data, Message = message };
        }
    }
}
