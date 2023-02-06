using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.models
{
    public class Commande
    {
        [Key] public int Id { get; set; }
        public int Quantite { get; set; }
        [Column(TypeName="Date")] public DateTime Date { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public bool isFournisseur { get; set; }
    }
}