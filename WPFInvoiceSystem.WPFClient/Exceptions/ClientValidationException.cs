namespace WPFInvoiceSystem.WPFClient.Exceptions
{
    public class ClientValidationException : ClientException
    {
        public ClientValidationException()
        {
        }

        public ClientValidationException(string message) : base(message)
        {
        }
    }
}
