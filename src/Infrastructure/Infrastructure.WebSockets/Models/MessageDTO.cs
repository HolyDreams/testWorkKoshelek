namespace Infrastructure.WebSockets.Models
{
    public class MessageDTO
    {
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
