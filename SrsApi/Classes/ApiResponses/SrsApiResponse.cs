using System.Net;

namespace SrsApi.Classes.ApiResponses
{
    public class SrsApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object? Response { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
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
