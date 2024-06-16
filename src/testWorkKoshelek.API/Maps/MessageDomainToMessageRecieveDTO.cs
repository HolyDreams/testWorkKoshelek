using AutoMapper;
using Core.Domain.Domain;
using testWorkKoshelek.API.Models;

namespace testWorkKoshelek.API.Maps
{
    public class MessageDomainToMessageRecieveDTO : Profile
    {
        public MessageDomainToMessageRecieveDTO()
        {
            CreateMap<Message, MessageRecieveDTO>()
                .ForMember(dst => dst.Text,
                    opt => opt.MapFrom(src => src.Text))
                .ForMember(dst => dst.AcceptedDate,
                    opt => opt.MapFrom(src => src.CreatedDate));
        }
    }
}
