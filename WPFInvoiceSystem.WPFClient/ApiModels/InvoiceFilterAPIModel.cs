using System;

namespace WPFInvoiceSystem.WPFClient.ApiModels
{
    public record InvoicesFilterAPIModel(
        DateTime? FromDate,
        DateTime? ToDate,
        int? CustomerId
        );
}
