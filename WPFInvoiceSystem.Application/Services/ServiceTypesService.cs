using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Application.Utils;
using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Application.Services
{
    public class ServiceTypesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceTypesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceTypeDto> CreateServiceType(ServiceTypeInputDto typeData)
        {
            var serviceType = new ServiceType() { Name = typeData.Name };

            _unitOfWork.ServiceTypesRepository.Add(serviceType);

            await _unitOfWork.CompleteAsync();

            return serviceType.AsDto();
        }

        public async Task DeleteServiceType(int id)
        {
            ServiceType serviceType = await GetServiceTypeAsEntity(id);

            await EnsureTypeIsNotRelatedToAnyService(id);

            _unitOfWork.ServiceTypesRepository.Remove(serviceType);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<ServiceTypeDto> GetServiceType(int id)
        {
            ServiceType? serviceType = await _unitOfWork.ServiceTypesRepository.GetAsync(id);

            if (serviceType == null)
                throw new CoreNotFoundException($"Couldn't find service type with the id {id}");

            return serviceType.AsDto();
        }

        public async Task<IEnumerable<ServiceTypeDto>> GetAllServiceTypes()
        {
            IEnumerable<ServiceType> serviceTypes = await _unitOfWork.ServiceTypesRepository
                .GetAllAsync();

            return serviceTypes.Select(s => s.AsDto());
        }

        public async Task<ServiceTypeDto> UpdateServiceType(int id, ServiceTypeInputDto typeData)
        {
            ServiceType serviceType = await GetServiceTypeAsEntity(id);

            serviceType.Name = typeData.Name;

            await _unitOfWork.CompleteAsync();

            return serviceType.AsDto();
        }

        private async Task EnsureTypeIsNotRelatedToAnyService(int typeId)
        {
            IEnumerable<Service> queryResult = await _unitOfWork.ServicesRepository
                .FindAsync(s => s.Type.Id == typeId);

            if (queryResult.Any())
                throw new CoreActionForbiddenException("The Service Type cannot be deleted because one or more Services are still associated with it");
        }

        private async Task<ServiceType> GetServiceTypeAsEntity(int id)
        {
            ServiceType? serviceType = await _unitOfWork.ServiceTypesRepository.GetAsync(id);

            if (serviceType == null)
                throw new CoreNotFoundException($"Couldn't find service type with the id {id}");

            return serviceType;
        }
    }
}