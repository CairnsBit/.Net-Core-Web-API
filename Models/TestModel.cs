using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_App.Models
{
    public class TestModel
    {
        [Key]
        [JsonPropertyName("ID")]
        public int Id { get; set; }
        
        [Required]
        [JsonPropertyName("Temperatur")] 
        public int Temperature { get; set; }

        [JsonPropertyName("Sammanfattning")]
        public string Summary { get; set; }
        
        [JsonPropertyName("Datum")]
        public DateTime Date { get; set; }
    }
}