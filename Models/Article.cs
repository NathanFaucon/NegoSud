using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.models
{
    public class Article
    {
        [Key] public int Id { get; set; }
        [StringLength(48)] public string Libelle { get; set; } = "";
        public float Prix { get; set; }

        [ForeignKey("Famille")]
        public int FamilleId { get; set; }
        public Famille Famille { get; set; }

        [ForeignKey("Fournisseur")]
        public int FournisseurId { get; set; }
        public Fournisseur Fournisseur { get; set; }
    }
}