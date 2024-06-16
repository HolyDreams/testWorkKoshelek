using AutoMapper;
using Core.Domain.Domain;
using Infrastructure.WebSockets.Models;

namespace Infrastructure.WebSockets.Maps
{
    public class MessageDomainToDto : Profile
    {
        public MessageDomainToDto()
        {
            CreateMap<Message, MessageDTO>()
                .ForMember(dst => dst.Text,
                    opt => opt.MapFrom(src => src.Text))
                .ForMember(dst => dst.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate));
        }
    }
}
