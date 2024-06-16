using Base.Interfaces.DataAccess;
using Core.Interfaces.DataAccess.Repositories;

namespace Core.Interfaces.DataAccess
{
    public interface IUnitOfWork : IBaseUnitOfWork
    {
        public IMessageRepository MessageRepository { get; }
    }
}
