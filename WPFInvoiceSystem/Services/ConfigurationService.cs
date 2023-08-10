using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFInvoiceSystem.Services
{
    public static class ConfigurationService
    {
        public static IConfiguration Config { get; } = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        public static decimal StandardTaxRate { get; } = Config.GetValue<decimal>("StandardTaxRate");
    }
}
