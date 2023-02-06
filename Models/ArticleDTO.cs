using System.ComponentModel.DataAnnotations;

namespace Cube_4.models
{
    public class ArticleDTO
    {
        public int? Id { get; set; }

        [Required]
        public string Libelle { get; set; } = "";
        
        [Required]
        public float Prix { get; set; }
        
        [Required]
        public Famille Famille { get; set; }
        
        [Required]
        public Fournisseur Fournisseur { get; set; }
    }
}