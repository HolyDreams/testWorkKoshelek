using Base.Models.Results;
using Core.Domain.Domain;
using Core.Interfaces.DataAccess;
using Core.Interfaces.WebSocketConnection;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Logic
{
    public class SenderMessage : ISenderMessage
    {
        #region Private

        private readonly ILogger<SenderMessage> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebSocketStreamProvider _webSocket;

        #endregion

        public SenderMessage(ILogger<SenderMessage> logger,
            IUnitOfWork unitOfWork,
            IWebSocketStreamProvider webSocket)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _webSocket = webSocket;

        }

        #region Methods

        public async Task<Result> SendMessage(Message message)
        {
            _logger.LogInformation("{Method} Начало отправки сообщения", nameof(SendMessage));

            _unitOfWork.BeginTran();

            var sendResult = await _unitOfWork.MessageRepository.AddMessageAsync(message);

            if (sendResult.IsError)
            {
                _unitOfWork.RollBack();

                return sendResult;
            }

            _unitOfWork.Commit();

            _logger.LogDebug("{Method} Начало отправки сообщения в WebSocket", nameof(SendMessage));
            try
            {
                await _webSocket.ConnectAsync();

                _webSocket.SendMessage(message);

                _webSocket.Close();

                _logger.LogDebug("{Method} Успешная отправка сообщения в WebSocket", nameof(SendMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} Исключение при отправки сообщения в WebSocket", nameof(SendMessage));

                _webSocket.Close();

                return Result.CreateErrorResult(ex.Message);
            }

            _logger.LogInformation("{Method} Успешная отправка сообщения", nameof(SendMessage));

            return Result.CreateSuccessResult();
        }

        #endregion
    }
}
