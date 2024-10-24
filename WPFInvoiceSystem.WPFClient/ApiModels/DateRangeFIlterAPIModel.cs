using System;

namespace WPFInvoiceSystem.WPFClient.ApiModels
{
    public record DateRangeFilterAPIModel(
        DateTime FromDate,
        DateTime ToDate
    );
}
