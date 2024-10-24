using System;
using System.Collections.Generic;

namespace WPFInvoiceSystem.WPFClient.ApiModels
{
    public record InvoiceInputAPIModel(
        DateTime Date,
        int InvoiceNumber,
        int CustomerId,
        IEnumerable<InvoiceServiceInputAPIModel> InvoiceServices
        );
}
