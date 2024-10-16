using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Services;

namespace WPFInvoiceSystem.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ReportsService _reportsService;
        public ReportsController(ReportsService reportsService, IMapper mapper)
        {
            _mapper = mapper;
            _reportsService = reportsService; 
        }


        [HttpGet("invoices")]
        public async Task<IActionResult> GetIncomeByServiceReport([FromQuery] DateRangeFilterDto filter)
        {
            DateRangeFilterDto filterDto = _mapper.Map<DateRangeFilterDto>(filter);

            byte[] reportContent = await _reportsService
                .GenerateInvoicesReport(filterDto);

            return File(
                reportContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                );
        }
    }
}
