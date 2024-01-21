using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductRetunDto>()
                .ForMember(PD => PD.ProductBrand, o => o.MapFrom(p => p.ProductBrand.Name))
                .ForMember(PD => PD.ProductType, o => o.MapFrom(p => p.ProductType.Name))
                .ForMember(PD => PD.PictureUrl, o => o.MapFrom< ProductPictureUrlResolver>());

            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggreation.Address>();
        }
    }
}
