using Cube_4.Data;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cube_4.Controllers
{
    public class CommandeController : Controller
    {
        private readonly ApplicationDbContext context;

        public CommandeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            List<Commande> list = context.Commandes.ToList();
            ViewBag.User = new SelectList(context.Users.ToList(), "Id", "Email");
            ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.User = new SelectList(context.Users.ToList(), "Id", "Email");
            ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
            return View();
        }

        public IActionResult Edit(int id)
        {
            //Famille famille = GetFamilleById(id);
            return View();
        }

        
        public IActionResult GetCommande()
        {
            List<Commande> myCommands = context.Commandes.ToList();
            
            if (myCommands.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Commandes:",
                    Commande = myCommands
                });

            } else
            {
                return NotFound(new
                {
                    Message = "Aucune commandes dans la base de donnée !"
                });
            }
        }
        
        
        public IActionResult GetCommandById(int commandeId)
        {
            Commande? findCommand = context.Commandes.FirstOrDefault(x => x.Id == commandeId);

            if (findCommand == null)
            {
                return NotFound(new
                {
                    Message = "Aucune commande trouvé avec cet ID !"
                });
            } else
            {
                return Ok(new
                {
                    Message = "Commande trouvée !",
                    Commande = new CommandeDTO() { Id = findCommand.Id, Quantite = findCommand.Quantite, Date = findCommand.Date, User = findCommand.User, Article = findCommand.Article}
                });
            }
        }

        [HttpPost("commande")]
        public IActionResult AddCommand(CommandeDTO newCommand, int quantite, string UserId, bool isFournisseur)
        {
            User? findUser = context.Users.FirstOrDefault(x => x.Id == UserId);
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == newCommand.Article.Id);

            if (findUser == null || findArticle == null)
            {
                return NotFound(new
                {
                    Message = "Aucun User trouvé avec cet ID !"
                });
            }
            if (findUser.IsAdmin == true && isFournisseur == true) {
                Commande addCommandFournisseur = new Commande()
                {
                    Quantite = quantite,
                    Date = newCommand.Date,
                    User = findUser,
                    Article = findArticle,
                    isFournisseur = true
                };
                Stock? findStockFournisseur = context.Stocks.FirstOrDefault(x => x.ArticleId == newCommand.Article.Id);
                if (findStockFournisseur == null)
                {
                    return NotFound(new
                    {
                        Message = "Aucun Article correspondant dans le stock !"
                    });
                }
                findStockFournisseur.Quantite += addCommandFournisseur.Quantite;
                context.Commandes.Add(addCommandFournisseur);
                context.Stocks.Update(findStockFournisseur);
                if (context.SaveChanges() > 0)
                {
                    List<Commande> Commandes = context.Commandes.ToList();
                    ViewBag.User = new SelectList(context.Users.ToList(), "Id", "Email");
                    ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
                    return View("Index", Commandes);
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur est survenue..."
                    });
                }
            }
            else
            {
                Commande addCommand = new Commande()
                {
                    Quantite = quantite,
                    Date = newCommand.Date,
                    User = findUser,
                    Article = findArticle,
                    isFournisseur = false
                };
                Stock? findStock = context.Stocks.FirstOrDefault(x => x.ArticleId == newCommand.Article.Id);
                if (findStock == null)
                {
                    return NotFound(new
                    {
                        Message = "Aucun Article correspondant dans le stock !"
                    });
                }
                findStock.Quantite -= addCommand.Quantite;
                if (findStock.Quantite < 0)
                {
                    Commande addCommandFournisseur = new Commande()
                    {
                        Quantite = Math.Abs(findStock.Quantite),
                        Date = newCommand.Date,
                        User = findUser,
                        Article = findArticle,
                        isFournisseur = true
                    };
                    Stock? findStockFournisseur = context.Stocks.FirstOrDefault(x => x.ArticleId == newCommand.Article.Id);
                    if (findStockFournisseur == null)
                    {
                        return NotFound(new
                        {
                            Message = "Aucun Article correspondant dans le stock !"
                        });
                    }
                    findStockFournisseur.Quantite += addCommandFournisseur.Quantite;
                    context.Commandes.Add(addCommandFournisseur);
                }
                context.Commandes.Add(addCommand);
                context.Stocks.Update(findStock);
                if (context.SaveChanges() > 0)
                {
                    List<Commande> Commandes = context.Commandes.ToList();
                    ViewBag.User = new SelectList(context.Users.ToList(), "Id", "Email");
                    ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
                    return View("Index", Commandes);
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
        
        [HttpPatch("commande")]
        public IActionResult EditCommand(CommandeDTO newInfos)
        {
            Commande? findCommand = context.Commandes.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findCommand != null )
            {
                findCommand.Quantite = newInfos.Quantite;
                findCommand.Date = newInfos.Date;
                findCommand.User = newInfos.User;
                findCommand.Article = newInfos.Article;

                context.Commandes.Update(findCommand);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "La commande a bien été modifié !"
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
                    Message = "Aucune commande n'a été trouvé avec cet ID !"
                });
            }
        }
        
        public IActionResult DeleteCommand(int Id)
        {
            Commande? findCommand = context.Commandes.FirstOrDefault(x => x.Id == Id);

            if (findCommand == null)
            {
                return NotFound(new
                {
                    Message = "Aucune commande trouvé avec cet ID !"
                });
            }
            else
            {
                context.Commandes.Remove(findCommand);
                if (context.SaveChanges() > 0)
                {
                    List<Commande> Commandes = context.Commandes.ToList();
                    ViewBag.User = new SelectList(context.Users.ToList(), "Id", "Email");
                    ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
                    return View("Index", Commandes);
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