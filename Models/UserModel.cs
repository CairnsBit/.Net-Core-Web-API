using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_App.Models
{
    public class UserModel
    {
        [Key]
        [Required]  
        [JsonPropertyName("Användarnamn")]
        public string Username { get; set; }  
        
        [JsonPropertyName("E-Postadress")]
        public string Mailaddress { get; set; }

        [Required]
        [JsonPropertyName("Lösenord")]
        public string Password { get; set; }  
        
        [Required]
        [JsonPropertyName("Skapelsedatum")]
        public DateTime CreationDate { get; set; }
    }
}