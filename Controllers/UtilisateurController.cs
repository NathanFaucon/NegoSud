using Cube_4.Data;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            List<User> myUsers = context.Users.ToList();
            
            if (myUsers.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Users:",
                    Article = myUsers
                });

            } else
            {
                return NotFound(new
                {
                    Message = "Aucun Users dans la base de données !"
                });
            }
        }
        
        [HttpGet("users/{userId}")] 
        public IActionResult GetUserById(string userId)
        {
            User? findUser = context.Users.FirstOrDefault(x => x.Id == userId);

            if (findUser == null)
            {
                return NotFound(new
                {
                    Message = "Aucun user trouvé avec cet ID !"
                });
            } else
            {
                return Ok(new
                {
                    Message = "User trouvé !",
                    Article = new UserDTO() { Id = findUser.Id, Firstname = findUser.Firstname, Lastname = findUser.Lastname, Email = findUser.Email, Password = findUser.Password, isAdmin = findUser.IsAdmin}
                });
            }
        }

        [HttpPost("users")]
        public IActionResult AddUser(UserDTO newUser)
        {
            User addUser = new User()
            {
                Firstname = newUser.Firstname, Lastname = newUser.Lastname, Email = newUser.Email,
                Password = newUser.Password, IsAdmin = newUser.isAdmin
            };
            context.Users.Add(addUser);
            if (context.SaveChanges() > 0)
            {
                return Ok(new
                {
                    Message = "L'utilisateur a été ajouté avec succès!",
                    ArticleId = addUser.Id
                });
            } else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }
        
        [HttpPatch("users")]
        public IActionResult EditUser(UserDTO newInfos)
        {
            User? findUser = context.Users.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findUser != null )
            {
                findUser.Firstname = newInfos.Firstname;
                findUser.Lastname = newInfos.Lastname;
                findUser.Email = newInfos.Email;
                findUser.Password = newInfos.Password;
                findUser.IsAdmin = newInfos.isAdmin;

                context.Users.Update(findUser);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "L'utilisateur a bien été modifié !"
                    });
                } else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur a eu lieu durant la modification..."
                    });
                }
            } else
            {
                return NotFound(new
                {
                    Message = "Aucun article n'a été trouvé avec cet ID !"
                });
            }
        }
        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            User? findUser = context.Users.FirstOrDefault(x => x.Id == userId);

            if (findUser == null)
            {
                return NotFound(new
                {
                    Message = "Aucun User trouvé avec cet ID !"
                });
            }
            else
            {
                context.Users.Remove(findUser);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "L'utilisateur a bien été supprimé",
                    });
                } else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur est survenue..."
                    });
                }
            }
        }
    }
}