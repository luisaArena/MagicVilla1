using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Api1.Modelos.DTO
{
    public class NumeroVillaDto
    {
        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }

        public string DetalleEspecial { get; set; }

    }
}
