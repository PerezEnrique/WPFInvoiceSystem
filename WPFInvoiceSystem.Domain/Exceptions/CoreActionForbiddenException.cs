
namespace WPFInvoiceSystem.Domain.Exceptions
{
    public class CoreActionForbiddenException : CoreException
    {
        public CoreActionForbiddenException()
        {
        }

        public CoreActionForbiddenException(string? message) : base(message)
        {
        }
    }
}
