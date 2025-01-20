using System.Net;
using System.Text.Json.Serialization;

namespace SrsApi.Classes.ApiResponses
{
    public class SrsApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object? Response { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorMessage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Exception { get; set; }

        public void SetExceptionDetails(Exception exception, bool includeFullExceptionInResponse = false)
        {
            ErrorMessage = exception.Message;

            if (includeFullExceptionInResponse)
            {   
                Exception = Newtonsoft.Json.JsonConvert.SerializeObject(exception);
            }
        }
    }
}
