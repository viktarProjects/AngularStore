using AngularStore.Core.Entities;
using AngularStore.Core.Entities.Identity;
using AngularStore.Core.Entities.OrderAggregate;
using AngularStore.WebAPI.Dto_s;
using AutoMapper;

namespace AngularStore.WebAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                 .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                 .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                 .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, Basket>();
            CreateMap<BasketItemDto, BasketItem>()
                .ForMember(x => x.ProductId, y => y.MapFrom(z => z.ProductId))
                .ForMember(x => x.Quantity, y => y.MapFrom(z => z.Quantity));
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
            CreateMap<Basket, BasketDto>();
        }
    }
}
