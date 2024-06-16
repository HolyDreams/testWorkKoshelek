using Base.Models.Results;
using Core.Domain.Domain;

namespace Logic.Interfaces
{
    public interface ISenderMessage
    {
        Task<Result> SendMessage(Message message);
    }
}
