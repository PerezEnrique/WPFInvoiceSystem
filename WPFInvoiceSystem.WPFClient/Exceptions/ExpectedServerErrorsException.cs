using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.WPFClient.Exceptions
{
    public class ExpectedServerErrorsException : Exception
    {
        public Collection<string> Errors { get; } = [];
        public ExpectedServerErrorsException()
        {
        }

        public ExpectedServerErrorsException(string? message) : base(message)
        {
        }

        public ExpectedServerErrorsException(IEnumerable<string> errors)
        {
            Errors = new Collection<String>().AddRange(errors);
        }
    }
}
