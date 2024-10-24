using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Utils;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Application.Services
{
    public class ServicesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceDto> CreateService(ServiceInputDto serviceData)
        {
            ServiceType serviceType = await GetServiceType(serviceData.TypeId);

            var service = new Service()
            {
                Name = serviceData.Name,
                Price = serviceData.Price,
                Type = serviceType,
                IsExempt = serviceData.IsExempt
            };

            _unitOfWork.ServicesRepository.Add(service);
            await _unitOfWork.CompleteAsync();
            return service.AsDto();
        }
        public async Task DeleteService(int id)
        {
            Service service = await GetServiceAsEntity(id);

            _unitOfWork.ServicesRepository.Remove(service);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ServiceDto>> FindByName(string name)
        {
            IEnumerable<Service> services = await _unitOfWork.ServicesRepository
                .FindAsync(s => s.Name.Contains(name));

            return services.Select(s => s.AsDto());
        }

        public async Task<IEnumerable<ServiceDto>> GetAllServices()
        {
            IEnumerable<Service> services = await _unitOfWork.ServicesRepository
                .GetAllAsync();

            return services.Select(s => s.AsDto());
        }

        public async Task<ServiceDto> GetService(int id)
        {
            Service? service = await _unitOfWork.ServicesRepository
                .GetAsync(id);

            if (service == null)
                throw new CoreNotFoundException($"Couldn't find service with the id {id}");

            return service.AsDto();
        }

        public async Task<ServiceDto> UpdateService(int id, ServiceInputDto serviceData)
        {
            Service service = await GetServiceAsEntity(id);
            ServiceType serviceType = await GetServiceType(serviceData.TypeId);

            service.Name = serviceData.Name;
            service.Price = serviceData.Price;
            service.Type = serviceType;
            service.IsExempt = serviceData.IsExempt;

            await _unitOfWork.CompleteAsync();

            return service.AsDto();
        }

        private async Task<ServiceType> GetServiceType(int typeId)
        {
            ServiceType? serviceType = await _unitOfWork.ServiceTypesRepository
                .GetAsync(typeId);

            if (serviceType == null)
                throw new CoreNotFoundException("Could not find the selected service type");

            return serviceType;
        }

        private async Task<Service> GetServiceAsEntity(int id)
        {
            Service? service = await _unitOfWork.ServicesRepository.GetAsync(id);

            if (service == null)
                throw new CoreNotFoundException($"Couldn't find service with the id {id}");

            return service;
        }
    }
}
