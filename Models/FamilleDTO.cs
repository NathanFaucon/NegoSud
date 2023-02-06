using System;
using System.ComponentModel.DataAnnotations;

namespace Cube_4.models
{
    public class FamilleDTO
    {

        public int? Id { get; set; }

        [Required]
        public string? Nom { get; set; } = "";

    }
}