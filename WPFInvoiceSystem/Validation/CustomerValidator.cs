using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;

namespace WPFInvoiceSystem.Validation
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Address).NotEmpty();
            RuleFor(c => c.Address).MaximumLength(150);
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Name).MaximumLength(75);
            RuleFor(c => c.Phone).NotEmpty();
            RuleFor(c => c.Phone).MaximumLength(150);
        }
    }
}
