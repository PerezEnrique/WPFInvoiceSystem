using AutoMapper;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.Application.Dtos;

namespace WPFInvoiceSystem.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerDto, CustomerResource>();
            CreateMap<CustomerInputResource, CustomerInputDto>();
            CreateMap<DateRangeFilterResource, DateRangeFilterDto>();
            CreateMap<InvoiceDto, InvoiceResource>();
            CreateMap<InvoicesFilterResource, InvoicesFilterDto>();
            CreateMap<InvoiceInputResource, InvoiceInputDto>();
            CreateMap<InvoiceServiceInputResource, InvoiceServiceInputDto>();
            CreateMap<InvoiceServiceDto, InvoiceServiceResource>();
            CreateMap<ServiceDto, ServiceResource>();
            CreateMap<ServiceInputResource, ServiceInputDto>();
            CreateMap<ServiceTypeDto, ServiceTypeResource>();
            CreateMap<ServiceTypeInputResource, ServiceTypeInputDto>();
        }
    }
}
