using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.models
{
    public class User : IdentityUser
    {
        [Key] public string Id { get; set; }
        [StringLength(48)] public string Firstname { get; set; } = "";
        [StringLength(48)] public string Lastname { get; set; } = "";
        [StringLength(48)] override public string Email { get; set; } = "";
        [StringLength(32)] public string Password { get; set; } = "";
        public bool IsAdmin { get; set; }
    }
}