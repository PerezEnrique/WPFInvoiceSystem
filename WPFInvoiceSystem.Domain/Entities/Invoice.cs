using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public required virtual Customer Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal Exempt { get; set; }
        public int InvoiceNumber { get; set; }
        public virtual Collection<InvoiceService> Services { get; set; } = [];
        public bool IsPaid { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxBase { get; set; }
        public decimal Total { get; set; }

        public void Calculate(decimal standardTaxRate)
        {
            Exempt = 0;
            TaxBase = 0;

            foreach (var item in Services)
            {
                if (item.Service.IsExempt == true)
                {
                    Exempt += item.Service.Price * item.Quantity;
                }
                else
                {
                    TaxBase += item.Service.Price * item.Quantity;
                }
            }

            Tax = TaxBase * standardTaxRate;

            Total = Exempt + TaxBase + Tax;
        }
    }
}
