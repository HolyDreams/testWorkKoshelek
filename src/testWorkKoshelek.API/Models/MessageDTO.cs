using System.ComponentModel.DataAnnotations;

namespace testWorkKoshelek.API.Models
{
    /// <summary>
    /// Новое сообщение
    /// </summary>
    public class MessageDTO
    {
        /// <summary>
        /// Текст сообщения
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Текст сообщения обазателен")]
        public required string Text { get; set; }
    }
}
