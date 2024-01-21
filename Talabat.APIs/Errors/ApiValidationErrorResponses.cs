namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorResponses: ApiErrorResponses
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponses():base(400)
        {
            Errors = new List<string>();
        }

    }
}
