using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFInvoiceSystem.Domain.Entities
{
    public enum PaymentStatus
    {
        YetToPay,
        PartiallyPaid,
        Paid
    }
    public class Invoice
    {
        public int Id { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal Exempt { get; set; }
        public int InvoiceNumber { get; set; }
        public virtual ObservableCollection<InvoiceService> Services { get; set; }
        public virtual ObservableCollection<Payment> Payments { get; set; }
        public PaymentStatus PaymentStatus
        {
            get
            {
                return Payments.Aggregate(
                    0m,
                    (acc, current) =>
                    {
                        if (current.IsChange) return acc - current.Value;
                        return acc + current.Value;
                    },
                    (value) =>
                    {
                        if (value >= Total) return PaymentStatus.Paid;
                        if (value > 0 && value < Total) return PaymentStatus.PartiallyPaid;
                        return PaymentStatus.YetToPay;
                    }
                );
            }
        }

        public decimal Tax { get; set; }
        public decimal TaxBase { get; set; }
        public decimal Total { get; set; }

        public Invoice()
        {
            Services = new ObservableCollection<InvoiceService>();
            Payments = new ObservableCollection<Payment>();
        }
    }
}
