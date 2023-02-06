using System.ComponentModel.DataAnnotations;

namespace Cube_4.models
{
    public class StockDTO
    {
        public int? Id { get; set; }

        [Required]
        public int Quantite { get; set; }

        [Required]
        public Article Article { get; set; }
    }
}
