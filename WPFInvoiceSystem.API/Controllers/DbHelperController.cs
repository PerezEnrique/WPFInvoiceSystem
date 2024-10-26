using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPFInvoiceSystem.Persistance;

namespace WPFInvoiceSystem.API.Controllers
{
    [ApiController]
    [Route("api/db")]
    public class DbHelperController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public DbHelperController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAndRecreateDb()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.MigrateAsync();
            return Ok();
        }
    }
}
