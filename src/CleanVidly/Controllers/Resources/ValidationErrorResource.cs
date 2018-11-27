using System.Collections.Generic;

namespace CleanVidly.Controllers.Resources
{
    public class ValidationErrorResource
    {
        public ValidationErrorResource()
        {
            Errors = new Dictionary<string, string[]>();
        }
        public Dictionary<string, string[]> Errors { get; private set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
    }
}