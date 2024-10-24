using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Utils;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Application.Services
{
    public class CustomersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerDto> CreateCustomer(CustomerInputDto customerData)
        {
            await EnsureIdentityCardIsNotRegisteredAlready(customerData.IdentityCard);

            var customer = new Customer()
            {
                Name = customerData.Name,
                IdentityCard = customerData.IdentityCard,
                Address = customerData.Address,
                Phone = customerData.Phone,
                Birthdate = customerData.Birthdate
            };

            _unitOfWork.CustomersRepository.Add(customer);

            await _unitOfWork.CompleteAsync();

            return customer.AsDto();
        }

        public async Task DeleteCustomer(int id)
        {
            Customer customer = await GetCustomerAsEntity(id);

            await EnsureCustomerIsNotRelatedToAnyInvoice(id);

            _unitOfWork.CustomersRepository.Remove(customer);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<CustomerDto>> FindCustomersByName(string name)
        {
            IEnumerable<Customer> customers = await _unitOfWork.CustomersRepository
                .FindAsync(c => c.Name.Contains(name));

            return customers.Select(c => c.AsDto());
        }

        public async Task<IEnumerable<CustomerDto>> FindCustomerByIdentityCard(int identityCard)
        {
            IEnumerable<Customer> customers = await _unitOfWork.CustomersRepository
                .FindAsync(c => c.IdentityCard == identityCard);

            return customers.Select(c => c.AsDto());
        }

        public async Task<CustomerDto> GetCustomer(int id)
        {
            Customer? customer = await _unitOfWork.CustomersRepository.GetAsync(id);

            if (customer == null)
                throw new CoreNotFoundException($"Couldn't find customer with the id {id}");

            return customer.AsDto();
        }

        public async Task<CustomerDto> UpdateCustomer(int id, CustomerInputDto customerData)
        {
            Customer customer = await GetCustomerAsEntity(id);

            customer.Name = customerData.Name;
            customer.Address = customerData.Address;
            customer.Phone = customerData.Phone;
            customer.Birthdate = customerData.Birthdate;
            if(customer.IdentityCard != customerData.IdentityCard)
            {
                await EnsureIdentityCardIsNotRegisteredAlready(customerData.IdentityCard);
            }
            customer.IdentityCard = customerData.IdentityCard;

            await _unitOfWork.CompleteAsync();

            return customer.AsDto();
        }

        private async Task<Customer> GetCustomerAsEntity(int id)
        {
            Customer? customer = await _unitOfWork.CustomersRepository.GetAsync(id);

            if (customer == null)
                throw new CoreNotFoundException($"Couldn't find customer with the id {id}");

            return customer;
        }

        private async Task EnsureIdentityCardIsNotRegisteredAlready(int identityCard)
        {
            IEnumerable<Customer> customersWithProvidedIdentityCard = await _unitOfWork.CustomersRepository
                .FindAsync(c => c.IdentityCard == identityCard);

            if (customersWithProvidedIdentityCard.Any())
                throw new CoreValidationException(
                    "An identity card with the number provided has already been registered"
                    );
        }

        private async Task EnsureCustomerIsNotRelatedToAnyInvoice(int customerId)
        {
            IEnumerable<Invoice> queryResult = await _unitOfWork.InvoicesRepository
                .FindAsync(i => i.Customer.Id == customerId);

            if (queryResult.Any())
                throw new CoreActionForbiddenException("The customer cannot be deleted because one or more invoices are still associated with it");
        }
    }
}
