using Base.Models.Results;
using Core.Domain.Domain;
using Core.Interfaces.WebSocket;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.WebSocket.Hubs
{
    public class MessageSenderHub : Hub<IMessageSender>
    {
        #region Private

        private readonly ILogger<MessageSenderHub> _logger;

        #endregion

        public MessageSenderHub(ILogger<MessageSenderHub> logger) 
        {
            _logger = logger;
        }

        public async Task<Result> PostMessage(Message message)
        {
            _logger.LogDebug("Начало отправки сообщения в WebSocket. MessageId = {messageId}", message.Id);
            try
            {
                await Clients.All.PostMessage(message.Id, message.Text, message.CreatedDate);

                _logger.LogDebug("Успешная отправка сообщения в WebSocket. MessageId = {messageId}", message.Id);

                return Result.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Исключение при отправки сообщения в WebSocket. MessageId = {messageId}", message.Id);

                return Result.CreateErrorResult(ex.Message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Подключен вебсокет: {connectionId}", Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogError(exception, "Отключен вебсокет: {connectionId}", Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
