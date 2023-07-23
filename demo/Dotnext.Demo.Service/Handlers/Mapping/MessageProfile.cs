using AutoMapper;
using Dotnext.Demo.Core.Domain;
using Dotnext.Demo.Service.Messages;

namespace Dotnext.Demo.Service.Handlers.Mapping;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<UserMessage, User>()
            .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
            .ForMember(dst => dst.Name, src => src.MapFrom(e => e.Name))
            .ForMember(dst => dst.Email, src => src.MapFrom(e => e.Email));

        CreateMap<OrderMessage, Order>()
            .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
            .ForMember(dst => dst.ProductName, src => src.MapFrom(e => e.ProductName))
            .ForMember(dst => dst.Date, src => src.MapFrom(e => e.Date))
            .ForMember(dst => dst.Price, src => src.MapFrom(e => e.Price));
    }
}
