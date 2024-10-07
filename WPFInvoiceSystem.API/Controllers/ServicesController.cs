using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Services;

namespace WPFInvoiceSystem.API.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : ControllerBase
    {
        private readonly ServicesService _servicesService;
        private readonly IMapper _mapper;

        public ServicesController(
            ServicesService servicesService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _servicesService = servicesService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResource>> Create(ServiceInputResource serviceData)
        {
            ServiceInputDto serviceInput = _mapper.Map<ServiceInputDto>(serviceData);

            ServiceDto service = await _servicesService.CreateService(serviceInput);
            string actionName = nameof(Get);
            object routeValues = new { id = service.Id };

            ServiceResource serviceResource = _mapper.Map<ServiceResource>(service);
            return CreatedAtAction(actionName, routeValues, serviceResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _servicesService.DeleteService(id);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceResource>>> GetAll()
        {
            IEnumerable<ServiceDto> serviceDtos = await _servicesService.GetAllServices();

            IEnumerable<ServiceResource> servicesResources = _mapper
                .Map<IEnumerable<ServiceResource>>(serviceDtos);
            return Ok(servicesResources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResource>> Get(int id)
        {
            ServiceDto service = await _servicesService.GetService(id);

            ServiceResource serviceResource = _mapper.Map<ServiceResource>(service);

            return Ok(serviceResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResource>> Update(int id, ServiceInputDto serviceData)
        {
            ServiceDto service = await _servicesService.UpdateService(id, serviceData);

            ServiceResource serviceResource = _mapper.Map<ServiceResource>(service);

            return Ok(serviceResource);
        }
    }
}
