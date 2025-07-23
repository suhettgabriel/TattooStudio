using System.Text.Json.Serialization;

namespace TattooStudio.WebUI.ViewModels
{
    public class CalendarEventViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("start")]
        public DateTime Start { get; set; }

        [JsonPropertyName("end")]
        public DateTime End { get; set; }

        [JsonPropertyName("extendedProps")]
        public object ExtendedProps => new { tattooRequestId = TattooRequestId };

        [JsonIgnore]
        public int TattooRequestId { get; set; }
    }
}