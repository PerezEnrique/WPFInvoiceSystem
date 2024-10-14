namespace WPFInvoiceSystem.WPFClient.Exceptions
{
    public class ClientForbiddenActionException : ClientException
    {
        public ClientForbiddenActionException()
        {
        }

        public ClientForbiddenActionException(string message) : base(message)
        {
        }
    }
}
