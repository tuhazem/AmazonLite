using AutoMapper;

namespace Amazon.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Domain.Entities.Order, DTOs.OrderDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<Domain.Entities.OrderItem, DTOs.OrderItemDTO>();
        }
    }
}
