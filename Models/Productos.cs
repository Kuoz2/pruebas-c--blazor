using System.ComponentModel.DataAnnotations;

using puntodeventa.Services;
using puntodeventa.Models;
using System.Text.Json.Serialization;




namespace puntodeventa.Models
{
    public class Productos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [JsonPropertyName("Pcodigo")]
        public string Pcodigo { get; set; } = "0";
        [Required]
        [JsonPropertyName("Nombre")]
        public string Nombre { get; set; } = "";
        [Required]
        [JsonPropertyName("Precio")]
        public int Precio { get; set; } = 0;
        [Required]
        [JsonPropertyName("Precioiva")]
        public int Precioiva { get; set; } = 0;
        [JsonPropertyName("Fechain")]
        public DateTime? Fechain { get; set; } = DateTime.Now;
        [JsonPropertyName("Fechavenci")]
        public DateTime? Fechavenci { get; set; }
        [Required]
        [JsonPropertyName("Categoryid")]
        public int Categoryid { get; set; } = 1;
        [Required]
        [JsonPropertyName("Cantidad")]
        public int Cantidad { get; set; } = 0;
        [JsonIgnore] // Ignorar al serializar
        public List<Ventas> Ventas { get; set; } = new List<Ventas>();
            
    }
}
