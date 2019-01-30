using System.Net;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class Error
    {
        public string Message { get; set; }
        
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
        public int StatusCode { get; }

        public Error(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = (int)statusCode;
        }
    }
}