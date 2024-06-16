using Base.Interfaces.DataAccess;
using Core.Domain.Domain;

namespace Core.Interfaces.WebSocketConnection
{
    public interface IWebSocketStreamProvider : IBaseWebSocketStreamProvider
    {
        /// <summary>
        /// При подключении 
        /// </summary>
        event EventHandler OnConnected;

        /// <summary>
        /// При отключении
        /// </summary>
        event EventHandler OnClosed;

        /// <summary>
        /// Асинхронное подключение
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">Доменное сообщение</param>
        /// <returns></returns>
        void SendMessage(Message message);
    }
}
