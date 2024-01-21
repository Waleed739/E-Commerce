namespace Talabat.APIs.Errors
{
    public class ApiErrorResponses
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiErrorResponses(int statuscode, string? message = null )
        {
            StatusCode = statuscode;
            Message=message?? GetErrorMessage(StatusCode);
        }
        private string GetErrorMessage(int statuscode)
        {
            return statuscode switch
            {
                400=>"A bad Request, You Have Made",
                401=>"Authorized, you are not",
                404=>"Resources Not Found",
                500=>"There is Server Error",
                _  => null
            };
        }
    }
}
