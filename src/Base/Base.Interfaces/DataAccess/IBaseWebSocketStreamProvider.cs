namespace Base.Interfaces.DataAccess
{
    public interface IBaseWebSocketStreamProvider : IDisposable
    {
        /// <summary>
        /// Подключиться
        /// </summary>
        void Connect();

        /// <summary>
        /// Закрыть подключение
        /// </summary>
        void Close();

        /// <summary>
        /// Проверка на подключение
        /// </summary>
        /// <returns></returns>
        bool IsAlive();

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Send(ArraySegment<byte> message);
    }
}
