using AutoMapper;

namespace Amazon.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Domain.Entities.Order, DTOs.OrderDTO>();
            CreateMap<Domain.Entities.OrderItem, DTOs.OrderItemDTO>();
        }
    }
}
