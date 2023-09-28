using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Api1.Modelos.DTO
{
    public class VillaUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public Double Tarifa { get; set; }
        [Required]
        public int Ocupantes { get; set; }
        [Required]
        public int MetrosCuadrados { get; set; }
        [Required]
        public string imagenUrl { get; set; }

        public string Amenidad { get; set; }
    }
}
