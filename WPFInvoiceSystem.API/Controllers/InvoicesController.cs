using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Services;

namespace WPFInvoiceSystem.API.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoicesService _invoicesService;
        private readonly IMapper _mapper;

        public InvoiceController(InvoicesService invoicesService, IMapper mapper)
        {
            _invoicesService = invoicesService;
            _mapper = mapper;
        }

        [HttpPatch("change-payment-status/{id}")]
        public async Task<IActionResult> ChangeInvoicePaymentStatus(int id)
        {
            await _invoicesService.ChangeInvoicePaymentStatus(id);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceResource>> Create(InvoiceInputResource invoiceData)
        {
            InvoiceInputDto invoiceInput = _mapper.Map<InvoiceInputDto>(invoiceData);

            InvoiceDto invoice = await _invoicesService.CreateInvoice(invoiceInput);
            string actionName = nameof(Get);
            object routeValues = new { id = invoice.Id };

            InvoiceResource invoiceResource = _mapper.Map<InvoiceResource>(invoice);
            return CreatedAtAction(actionName, routeValues, invoiceResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _invoicesService.DeleteInvoice(id);

            return Ok();
        }

        [HttpGet("find")]
        public async Task<ActionResult<IEnumerable<InvoiceResource>>> FindInvoices([FromQuery] InvoicesFilterResource filter)
        {
            InvoicesFilterDto filterDto = _mapper.Map<InvoicesFilterDto>(filter);

            IEnumerable<InvoiceDto> invoiceDtos = await _invoicesService
                .FindInvoices(filterDto);

            IEnumerable<InvoiceResource> invoiceResources = _mapper
                .Map<IEnumerable<InvoiceResource>>(invoiceDtos);

            return Ok(invoiceResources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceResource>> Get(int id)
        {
            InvoiceDto invoice = await _invoicesService.GetInvoice(id);

            InvoiceResource invoiceResource = _mapper.Map<InvoiceResource>(invoice);
            return Ok(invoiceResource);
        }

        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetLastTenInvoices()
        {
            IEnumerable<InvoiceDto> invoiceDtos = await _invoicesService
                .GetLastTenInvoices();

            IEnumerable<InvoiceResource> invoiceResources = _mapper
                .Map<IEnumerable<InvoiceResource>>(invoiceDtos);

            return Ok(invoiceResources);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceResource>> Update(int id, InvoiceInputDto invoiceData)
        {
            InvoiceDto invoice = await _invoicesService.UpdateInvoice(id, invoiceData);

            InvoiceResource invoiceResource = _mapper.Map<InvoiceResource>(invoice);
            return Ok(invoiceResource);
        }
    }
}
