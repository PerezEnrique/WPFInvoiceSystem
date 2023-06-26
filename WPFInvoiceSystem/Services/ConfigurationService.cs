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
        private static IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        public static decimal StandardTaxRate { get; } = config.GetValue<decimal>("StandardTaxRate");
        public static decimal GFTTRate { get; } = config.GetValue<decimal>("GFTTRate");
    }
}
