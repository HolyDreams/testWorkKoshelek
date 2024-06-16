namespace Core.Domain.Models.Settings
{
    public class DbOptions
    {
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public required string ConnectionString { get; set; }

        /// <summary>
        /// Название схемы
        /// </summary>
        public required string Scheme { get; set; }

        /// <summary>
        /// Ограничение по времени на команду
        /// </summary>
        public int CommandTimeOut { get; set; }
    }
}
