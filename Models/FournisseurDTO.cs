using System.ComponentModel.DataAnnotations;

namespace Cube_4.models
{
    public class FournisseurDTO
    {
        public int? Id { get; set; }

        [Required]
        public string? Nom { get; set; } = "";
    }
}
