using System;

namespace WPFInvoiceSystem.WPFClient.Exceptions
{
    public abstract class ClientException : Exception
    {
        protected ClientException()
        {
        }

        protected ClientException(string message) : base(message)
        {
        }
    }
}
