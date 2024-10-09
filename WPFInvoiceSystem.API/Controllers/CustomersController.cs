using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Services;

namespace WPFInvoiceSystem.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomersService _customersService;
        private readonly IMapper _mapper;

        public CustomersController(CustomersService customersService, IMapper mapper)

        {
            _mapper = mapper;
            _customersService = customersService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerResource>> Create(CustomerInputResource customerData)
        {
            CustomerInputDto customerInput = _mapper.Map<CustomerInputDto>(customerData);

            CustomerDto customer = await _customersService.CreateCustomer(customerInput);
            string actionName = nameof(Get);
            object routeValues = new { id = customer.Id };

            CustomerResource customerResource = _mapper.Map<CustomerResource>(customer);
            return CreatedAtAction(actionName, routeValues, customerResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customersService.DeleteCustomer(id);

            return Ok();
        }

        [HttpGet("by-identity-card/{identityCard}")]
        public async Task<ActionResult<IEnumerable<CustomerResource>>> FindByIdentityCard(int identityCard)
        {
            IEnumerable<CustomerDto> customerDtos = await _customersService
                .FindCustomerByIdentityCard(identityCard);

            IEnumerable<CustomerResource> customerResources = _mapper
                .Map<IEnumerable<CustomerResource>>(customerDtos);

            return Ok(customerResources);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<CustomerResource>>> FindByName(string name)
        {
            IEnumerable<CustomerDto> customerDtos = await _customersService
                .FindCustomersByName(name);

            IEnumerable<CustomerResource> customerResources = _mapper
                .Map<IEnumerable<CustomerResource>>(customerDtos);

            return Ok(customerResources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResource>> Get(int id)
        {
            CustomerDto customer = await _customersService.GetCustomer(id);

            CustomerResource customerResource = _mapper.Map<CustomerResource>(customer);
            return Ok(customerResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerResource>> Update(int id, CustomerInputDto customerData)
        {
            CustomerDto customer = await _customersService.UpdateCustomer(id, customerData);

            CustomerResource customerResource = _mapper.Map<CustomerResource>(customer);
            return Ok(customerResource);
        }
    }
}
