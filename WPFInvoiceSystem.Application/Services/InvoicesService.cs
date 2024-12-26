using System.Collections.ObjectModel;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Utils;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Application.Services
{
    public class InvoicesService
    {
        private readonly decimal _taxRate;
        private readonly IUnitOfWork _unitOfWork;

        public InvoicesService(IUnitOfWork unitOfWork, decimal taxRate)
        {
            _taxRate = taxRate;
            _unitOfWork = unitOfWork;
        }

        public async Task ChangeInvoicePaymentStatus(int id)
        {
            Invoice invoice = await GetInvoiceAsEntity(id);

            invoice.IsPaid = !invoice.IsPaid;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<InvoiceDto> CreateInvoice(InvoiceInputDto invoiceData)
        {
            await EnsureInvoiceNumberIsNotRegisteredAlready(invoiceData.InvoiceNumber);

            var invoice = new Invoice()
            {
                Date = invoiceData.Date,
                InvoiceNumber = invoiceData.InvoiceNumber,
                Customer = await GetCustomer(invoiceData.CustomerId),
            };

            invoice.Services = await GetAndAttachInvoiceServices(invoice, invoiceData.InvoiceServices);
            invoice.Calculate(_taxRate);

            _unitOfWork.InvoicesRepository.Add(invoice);

            await _unitOfWork.CompleteAsync();

            return invoice.AsDto();
        }

        public async Task DeleteInvoice(int id)
        {
            Invoice invoice = await GetInvoiceAsEntity(id);

            _unitOfWork.InvoicesRepository.Remove(invoice);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<InvoiceDto>> FindInvoices(InvoicesFilterDto filter)
        {
            filter.Validate();

            IEnumerable<Invoice> invoices = await _unitOfWork.InvoicesRepository
                .FindAsync(filter);

            return invoices.Select(i => i.AsDto());
        }

        public async Task<IEnumerable<InvoiceDto>> GetLastTenInvoices()
        {
            IEnumerable<Invoice> invoices = await _unitOfWork.InvoicesRepository
                .GetLastTenInvoices();

            return invoices.Select(i => i.AsDto());
        }

        public async Task<InvoiceDto> GetInvoice(int id)
        {
            Invoice? invoice = await _unitOfWork.InvoicesRepository
                .GetAsync(id);

            if (invoice == null)
                throw new CoreNotFoundException($"Couldn't find customer with the id {id}");

            return invoice.AsDto();
        }

        public async Task<InvoiceDto> UpdateInvoice(int id, InvoiceInputDto invoiceData)
        {
            Invoice invoice = await GetInvoiceAsEntity(id);

            invoice.Date = invoiceData.Date;

            if (invoice.InvoiceNumber != invoiceData.InvoiceNumber)
            {
                await EnsureInvoiceNumberIsNotRegisteredAlready(invoiceData.InvoiceNumber);
            }
            invoice.InvoiceNumber = invoiceData.InvoiceNumber;

            if (invoiceData.CustomerId != invoice.Customer.Id)
            {
                invoice.Customer = await GetCustomer(invoiceData.CustomerId);
            }

            IEnumerable<int> currentServicesIds = invoice.Services.Select(s => s.Service.Id);
            IEnumerable<int> incomingServicesIds = invoiceData.InvoiceServices.Select(s => s.ServiceId);
            if (new HashSet<int>(incomingServicesIds).SetEquals(currentServicesIds))
            {
                invoice.Services = await GetAndAttachInvoiceServices(invoice, invoiceData.InvoiceServices);
            }

            invoice.Calculate(_taxRate);

            await _unitOfWork.CompleteAsync();

            return invoice.AsDto();
        }

        private async Task<Customer> GetCustomer(int id)
        {
            Customer? customer = await _unitOfWork.CustomersRepository
                .GetAsync(id);

            if (customer == null)
                throw new CoreNotFoundException($"Couldn't find customer with the id {id}");

            return customer;
        }

        private async Task<Collection<InvoiceService>> GetAndAttachInvoiceServices(Invoice invoice, IEnumerable<InvoiceServiceInputDto> servicesData)
        {
            var invoiceServices = new Collection<InvoiceService>();

            foreach (var serviceData in servicesData)
            {
                Service? service = await _unitOfWork.ServicesRepository
                    .GetAsync(serviceData.ServiceId);

                if (service == null)
                    throw new CoreNotFoundException("Couldn't find one of the services");

                var invoiceService = new InvoiceService()
                {
                    Invoice = invoice,
                    Service = service,
                    Quantity = serviceData.Quantity
                };

                invoiceServices.Add(invoiceService);
            }

            return invoiceServices;
        }

        private async Task<Invoice> GetInvoiceAsEntity(int id)
        {
            Invoice? invoice = await _unitOfWork.InvoicesRepository
                .GetAsync(id);

            if (invoice == null)
                throw new CoreNotFoundException($"Couldn't find invoice with the id {id}");

            return invoice;
        }

        private async Task EnsureInvoiceNumberIsNotRegisteredAlready(int invoiceNumber)
        {
            IEnumerable<Invoice> queryResult = await _unitOfWork.InvoicesRepository
                .FindAsync(i => i.InvoiceNumber == invoiceNumber);

            if (queryResult.Any())
                throw new CoreValidationException(
                    "An invoice with the number provided has already been registered"
                    );
        }
    }
}
