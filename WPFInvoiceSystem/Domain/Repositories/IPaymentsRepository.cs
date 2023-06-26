﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Domain.Repositories
{
    public interface IPaymentsRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetWithBankAndPaymentMethod(int id);
    }
}