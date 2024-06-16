using Base.Models.Results;
using Core.Domain.Domain;
using Core.Interfaces.DataAccess;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Logic
{
    public class GetterMessage : IGetterMessage
    {
        #region Private

        private readonly ILogger<GetterMessage> _logger;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public GetterMessage(ILogger<GetterMessage> logger, IUnitOfWork unitOfWork) 
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        #region Methods

        public async Task<ListResult<Message>> GetByPeriod(DateTime start, DateTime end)
        {
            _logger.LogInformation("{Method} Начало получения сообщений за период {start} - {end}", nameof(GetByPeriod), start, end);

            var result = await _unitOfWork.MessageRepository.GetMessagesByPeriodAsync(start, end);

            _logger.LogInformation("{Method} За период {start} - {end} найденно {count} сообщений",
                nameof(GetByPeriod), start, end, result.Items.Count());

            return result;
        }

        #endregion
    }
}
