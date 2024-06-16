using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace testWorkKoshelek.API.Models
{
    /// <summary>
    /// Сообщения с датой создания
    /// </summary>
    public class MessageRecieveDTO : MessageDTO
    {
        /// <summary>
        /// Время получения сообщения
        /// </summary>
        [BindRequired]
        public DateTime AcceptedDate { get; set; }
    }
}
