namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse:ApiErrorResponses
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statuscode, string? details=null, string? message=null):base(statuscode, message)
        {
            Details = details;
        }



    }
}
