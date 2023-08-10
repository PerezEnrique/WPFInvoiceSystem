using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;

namespace WPFInvoiceSystem.Validation
{
    public class ServiceValidator : AbstractValidator<Service>
    {
        public ServiceValidator()
        {
            RuleFor(s => s.Name).NotEmpty();
            RuleFor(s => s.Name).MaximumLength(75);
            RuleFor(s => s.Price).GreaterThan(0);
            RuleFor(s => s.Type).NotNull();
        }
    }
}
