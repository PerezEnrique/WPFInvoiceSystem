using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace WPFInvoiceSystem.Domain.Modals
{
    public class Customer
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public DateTime? Birthdate { get; set; }
        public int IdentityCard { get; set; }
        public virtual ObservableCollection<Invoice> Invoices { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
