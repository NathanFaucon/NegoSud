using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.models
{
    public class Famille
    {
        [Key] public int Id { get; set; }
        [StringLength(48)] public string Nom { get; set; } = "";
    }
}