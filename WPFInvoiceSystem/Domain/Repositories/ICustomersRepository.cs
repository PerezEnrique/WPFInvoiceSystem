﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Domain.Repositories
{
    public interface ICustomersRepository : IBaseRepository<Customer>
    {
        public Task<Customer?> GetByIdentityCard(int identityCard);
    }
}