namespace WPFInvoiceSystem.Domain.Exceptions
{
    public class CoreValidationException : CoreException
    {
        public CoreValidationException()
        {
        }

        public CoreValidationException(string? message) : base(message)
        {
        }
    }
}
