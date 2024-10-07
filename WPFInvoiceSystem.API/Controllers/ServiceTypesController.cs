using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Services;

namespace WPFInvoiceSystem.API.Controllers
{
    [ApiController]
    [Route("api/service-types")]
    public class ServiceTypesController : ControllerBase
    {
        private readonly ServiceTypesService _serviceTypesService;
        private readonly IMapper _mapper;

        public ServiceTypesController(
            ServiceTypesService serviceTypesService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _serviceTypesService = serviceTypesService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceTypeResource>> Create(ServiceTypeInputResource serviceTypeData)
        {
            ServiceTypeInputDto serviceTypeInput = _mapper.Map<ServiceTypeInputDto>(serviceTypeData);

            ServiceTypeDto serviceType = await _serviceTypesService.CreateServiceType(serviceTypeInput);
            string actionName = nameof(Get);
            object routeValues = new { id = serviceType.Id };

            ServiceTypeResource serviceTypeResource = _mapper.Map<ServiceTypeResource>(serviceType);
            return CreatedAtAction(actionName, routeValues, serviceTypeResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceTypesService.DeleteServiceType(id);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceTypeResource>>> GetAll()
        {
            IEnumerable<ServiceTypeDto> serviceTypesDto = await _serviceTypesService.GetAllServiceTypes();

            IEnumerable<ServiceTypeResource> serviceTypesResources = _mapper
                .Map<IEnumerable<ServiceTypeResource>>(serviceTypesDto);
            return Ok(serviceTypesResources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceTypeResource>> Get(int id)
        {
            ServiceTypeDto serviceType = await _serviceTypesService.GetServiceType(id);

            ServiceTypeResource serviceTypeResource = _mapper.Map<ServiceTypeResource>(serviceType);

            return Ok(serviceTypeResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceTypeResource>> Update(int id, ServiceTypeInputDto typeData)
        {
            ServiceTypeDto serviceType = await _serviceTypesService.UpdateServiceType(id, typeData);

            ServiceTypeResource serviceTypeResource = _mapper.Map<ServiceTypeResource>(serviceType);

            return Ok(serviceTypeResource);
        }
    }
}