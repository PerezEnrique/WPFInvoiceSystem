using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFInvoiceSystem.Domain.Modals
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ObservableCollection<Service> Services { get; set; }

        public ServiceType()
        {
            Services = new ObservableCollection<Service>();
        }
    }
}
