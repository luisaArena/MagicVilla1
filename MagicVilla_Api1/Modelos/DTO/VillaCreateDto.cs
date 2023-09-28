using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Api1.Modelos.DTO
{
    public class VillaCreateDto
    {
        
        
        [Required]
        [MaxLength]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public Double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadrados { get; set; }

        public string imagenUrl { get; set; }

        public string Amenidad { get; set; }
    }
}
