using System.ComponentModel.DataAnnotations;

namespace Cube_4.models
{
    public class CommandeDTO
    {
        public int? Id { get; set; }

        [Required]
        public int Quantite { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public User User { get; set; }
        
        [Required]
        public Article Article { get; set; }
    }
}