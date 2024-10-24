using System.Collections.Generic;

namespace WPFInvoiceSystem.WPFClient.ApiModels
{
    public class ProblemDetailsResponse
    {
        public string type { get; set; }
        public string title { get; set; }
        public short status { get; set; }
        public string traceId { get; set; }
        public IEnumerable<string> errors { get; set; }
    }
}
