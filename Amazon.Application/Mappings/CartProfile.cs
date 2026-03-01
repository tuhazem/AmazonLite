using AutoMapper;

namespace Amazon.Application.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Domain.Entities.Cart, DTOs.CartDTO>();
            CreateMap<Domain.Entities.CartItem, DTOs.CartItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}
