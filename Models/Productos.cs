using System.ComponentModel.DataAnnotations;

namespace puntodeventa.Models
{
    public class Productos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Pcodigo { get; set; } = "0";
        [Required]
        public string Nombre { get; set; } = "";
        [Required]
        public int Precio { get; set; } = 0;
        [Required]
        public int Precioiva { get; set; } = 0;
        [Required]
        public DateTime? Fechain { get; set; } = DateTime.Now;
        [Required]
        public DateTime? Fechavenci { get; set; }
        [Required]
        public int Categoryid { get; set; } = 1;
        [Required]
        public int Cantidad { get; set; } = 0;
        public List<Ventas> Ventas { get; set; } = new List<Ventas>();
            
    }
}
