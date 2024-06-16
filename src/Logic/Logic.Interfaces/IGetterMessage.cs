using Base.Models.Results;
using Core.Domain.Domain;

namespace Logic.Interfaces
{
    public interface IGetterMessage
    {
        /// <summary>
        /// Получить сообщения за период
        /// </summary>
        /// <param name="start">Начало периода</param>
        /// <param name="end">Окончания периода</param>
        /// <returns></returns>
        public Task<ListResult<Message>> GetByPeriod(DateTime start, DateTime end);
    }
}
