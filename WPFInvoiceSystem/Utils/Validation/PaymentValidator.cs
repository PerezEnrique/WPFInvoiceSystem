using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Utils.Validation
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(p => p.PaymentMethod).NotNull();
            RuleFor(p => p.Bank).NotNull().When(p => !p.PaymentMethod.IsCash);
            RuleFor(p => p.ReferenceNumber).NotNull().When(p => !p.PaymentMethod.IsCash);
            RuleFor(p => p.ReferenceNumber).MaximumLength(25);
            RuleFor(p => p.Value).GreaterThan(0);
        }
    }
}
