using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WPFInvoiceSystem.Persistance
{
    public class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlite(connectionString: "Filename=../WPFInvoiceSystem.API/WPFInvocieSystem.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
