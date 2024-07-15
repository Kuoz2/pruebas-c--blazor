
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace puntodeventa.Models
{
    public class Ventas
    {
        public int id { get; set; }
        [JsonPropertyName("preciov")]
        [Column("preciov")]
        public int Preciov { get; set; }
        [JsonPropertyName("cantidadv")]
        [Column("cantidadv")]
        public int Cantidadv { get; set; }
        [JsonPropertyName("fechav")]
        [Column("fechav")]
        public string Fechav { get; set; }
        [JsonPropertyName("metodov")]
        [Column("metodov")]
        public string Metodov { get; set; }
        [JsonPropertyName("productoin")]
        [Column("productoin")]
        public int Productoin { get; set; }
        [JsonPropertyName("productoid")]
        [Column("productoid")]
        public int Productoid { get; set; }
        [JsonIgnore] // Ignorar al serializar
        public Productos? Productos {  get; set; }        
    }
}
