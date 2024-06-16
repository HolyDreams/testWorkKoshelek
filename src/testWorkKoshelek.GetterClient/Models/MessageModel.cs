using System.Text.Json.Serialization;

namespace testWorkKoshelek.GetterClient.Models
{
    public class MessageModel
    {
        [JsonPropertyName("text")]
        public required string Text { get; set; }

        [JsonPropertyName("acceptedDate")]
        public DateTime CreatedDate { get; set; }
    }
}
