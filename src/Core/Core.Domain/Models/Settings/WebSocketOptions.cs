namespace Core.Domain.Models.Settings
{
    public class WebSocketOptions
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        public required string Uri { get; set; }

        /// <summary>
        /// Размер пачки
        /// </summary>
        public int ChunkSize { get; set; } = 1024;
    }
}
