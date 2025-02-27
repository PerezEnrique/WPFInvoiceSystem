using Microsoft.EntityFrameworkCore;
using WPFInvoiceSystem.API.ErrorHandling;
using WPFInvoiceSystem.API.Mapping;
using WPFInvoiceSystem.Application.Abstractions;
using WPFInvoiceSystem.Application.Services;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Persistance;
using WPFInvoiceSystem.ReportsGenerator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<AppErrorHandler>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder
    .Configuration
    .GetConnectionString("default");

    options.UseSqlite(connectionString);
});
builder.Services.AddScoped<IInvoicesReportGenerator, InvoiceReport>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
builder.Services.AddScoped<CustomersService>();
builder.Services.AddScoped<InvoicesService>((serviceProvider) =>
{
    var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    var taxRate = serviceProvider.GetRequiredService<IConfiguration>()
        .GetValue<decimal>("TaxRate");


    return new InvoicesService(unitOfWork, taxRate);
});
builder.Services.AddScoped<ReportsService>();
builder.Services.AddScoped<ServicesService>();
builder.Services.AddScoped<ServiceTypesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

/*This approach of applying migrations is not recommended for production
 however it's safe for our simple use case*/
if(app.Environment.IsProduction())
{
    AppDbContext dbContext = app.Services.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseExceptionHandler();
app.MapControllers();
app.Run();
