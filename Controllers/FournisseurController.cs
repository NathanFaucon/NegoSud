using Cube_4.Data;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.Controllers
{

    public class FournisseurController : Controller
    {
        private readonly ApplicationDbContext context;

        public FournisseurController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            List<Fournisseur> Fournisseurs = context.Fournisseurs.ToList();
            return View(Fournisseurs);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            Fournisseur fournisseur = GetFournisseurById(id);
            return View(fournisseur);
        }

        public IActionResult Edit(int id)
        {
            Fournisseur fournisseur = GetFournisseurById(id);
            return View(fournisseur);
        }

        [HttpGet("fournisseurs")]
        public IActionResult GetFournisseurs()
        {
            List<Fournisseur> myFournisseurs = context.Fournisseurs.ToList();

            if (myFournisseurs.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Fournisseurs:",
                    Fournisseur = myFournisseurs
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun Fournisseurs dans la base de données !"
                });
            }
        }

        public Fournisseur GetFournisseurById(int fournisseurId)
        {
            return context.Fournisseurs.FirstOrDefault(fournisseur => fournisseur.Id == fournisseurId);
        }

        public IActionResult add_fournisseurs(FournisseurDTO newFournisseur)
        {
            Fournisseur addFournisseur= new Fournisseur()
            {
                Nom = newFournisseur.Nom
            };
            context.Fournisseurs.Add(addFournisseur);
            if (context.SaveChanges() > 0)
            {
                List<Fournisseur> Fournisseurs = context.Fournisseurs.ToList();
                return View("Index", Fournisseurs);
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }

        public IActionResult EditFournisseur(FournisseurDTO newInfos)
        {
            Fournisseur? findFournisseur = context.Fournisseurs.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findFournisseur != null)
            {
                findFournisseur.Nom = newInfos.Nom;

                context.Fournisseurs.Update(findFournisseur);
                if (context.SaveChanges() > 0)
                {
                    List<Fournisseur> Fournisseurs = context.Fournisseurs.ToList();
                    return View("Index", Fournisseurs);
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
                    Message = "Aucun fournisseur n'a été trouvé avec cet ID !"
                });
            }
        }

        public IActionResult DeleteFournisseur(int Id)
        {
            Fournisseur? findFournisseur = context.Fournisseurs.FirstOrDefault(x => x.Id == Id);

            if (findFournisseur == null)
            {
                return NotFound(new
                {
                    Message = "Aucun Fournisseur trouvé avec cet ID !"
                });
            }
            else
            {
                context.Fournisseurs.Remove(findFournisseur);
                if (context.SaveChanges() > 0)
                {
                    List<Fournisseur> Fournisseurs = context.Fournisseurs.ToList();
                    return View("Index", Fournisseurs);
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
