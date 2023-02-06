using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.models
{
    public class Stock
    {
        [Key] public int Id { get; set; }
        public int Quantite { get; set; }
        
        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}