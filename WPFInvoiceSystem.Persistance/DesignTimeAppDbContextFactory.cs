using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WPFInvoiceSystem.Persistance
{
    public class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlite(connectionString: "Filename=" + Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "WPFInvoiceSystem.db"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
