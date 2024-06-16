using Base.Models.Results;
using Core.Domain.Domain;

namespace Core.Interfaces.DataAccess.Repositories
{
    public interface IMessageRepository
    {
        /// <summary>
        /// Создать новое сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public Task<Result> AddMessageAsync(Message message);

        /// <summary>
        /// Получить сообщения за диапазон дат
        /// </summary>
        /// <param name="start">Начальная дата</param>
        /// <param name="end">Окончательная дата</param>
        /// <returns></returns>
        public Task<ListResult<Message>> GetMessagesByPeriodAsync(DateTime start, DateTime end);
    }
}
