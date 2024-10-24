namespace WPFInvoiceSystem.Domain.Exceptions
{
    public class CoreNotFoundException : CoreException
    {
        public CoreNotFoundException()
        {
        }

        public CoreNotFoundException(string? message) : base(message)
        {
        }
    }
}
