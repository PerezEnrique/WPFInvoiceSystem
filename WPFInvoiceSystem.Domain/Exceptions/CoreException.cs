namespace WPFInvoiceSystem.Domain.Exceptions
{
    public class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(string? message) : base(message)
        {
        }
    }
}
