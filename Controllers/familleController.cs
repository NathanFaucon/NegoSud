using Cube_4.Data;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.Controllers
{

    public class FamilleController : Controller
    {
        private readonly ApplicationDbContext context;

        public FamilleController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            List<Famille> list = context.Familles.ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            Famille famille = GetFamilleById(id);
            return View(famille);
        }

        public IActionResult Details(int id)
        {
            Famille Famille = GetFamilleById(id);
            return View(Famille);
        }

        [HttpGet("familles")]
        public IActionResult GetFamilles()
        {
            List<Famille> myFamilles = context.Familles.ToList();

            if (myFamilles.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Familles:",
                    Famille = myFamilles
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucune Familles dans la base de données !"
                });
            }
        }

        public Famille GetFamilleById(int familleId)
        {
            return context.Familles.FirstOrDefault(famille => famille.Id == familleId);
        }

        [HttpPost("familles")]
        public IActionResult add_familles(FamilleDTO newFamille)
        {
            Famille addFamille = new Famille()
            {
                Nom = newFamille.Nom
           
            };
            context.Familles.Add(addFamille);
            if (context.SaveChanges() > 0)
            {
                List<Famille> Familles = context.Familles.ToList();
                return View("Index",Familles);
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }

        
        public IActionResult EditFamille(FamilleDTO newInfos)
        {
            Famille? findFamille = context.Familles.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findFamille != null)
            {
                findFamille.Nom = newInfos.Nom;

                context.Familles.Update(findFamille);
                if (context.SaveChanges() > 0)
                {
                    List<Famille> Familles = context.Familles.ToList();
                    return View("Index", Familles);
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur a eu lieu durant la modification..."
                    });
                }
            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucune famille n'a été trouvée avec cet ID !"
                });
            }
        }

        
        public IActionResult DeleteFamille(int Id)
        {
            Famille? findFamille = context.Familles.FirstOrDefault(x => x.Id == Id);

            if (findFamille == null)
            {
                return NotFound(new
                {
                    Message = "Aucune famille trouvé avec cet ID !"
                });
            }
            else
            {
                context.Familles.Remove(findFamille);
                if (context.SaveChanges() > 0)
                {
                    List<Famille> Familles = context.Familles.ToList();
                    return View("Index", Familles);
                }
                else
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