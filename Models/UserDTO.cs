using System.ComponentModel.DataAnnotations;

namespace Cube_4.models
{
    public class UserDTO
    {
        public string? Id { get; set; }

        [Required]
        public string Firstname { get; set; } = "";

        [Required] public string Lastname { get; set; } = "";

        [Required] public string Email { get; set; } = "";

        [Required] public string Password { get; set; } = "";

        [Required] public bool isAdmin { get; set; } = false;
    }
}