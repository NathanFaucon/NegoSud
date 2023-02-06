using Cube_4.Data;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cube_4.Controllers
{

    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext context;

        public ArticleController(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public IActionResult Index()
        {
            List<Article> list = context.Articles.ToList();
            ViewBag.Famille = new SelectList(context.Familles.ToList(), "Id", "Nom");
            ViewBag.Fournisseur = new SelectList(context.Fournisseurs.ToList(), "Id", "Nom");
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Famille = new SelectList(context.Familles.ToList(), "Id", "Nom");
            ViewBag.Fournisseur = new SelectList(context.Fournisseurs.ToList(), "Id", "Nom");
            return View();
        }

        public IActionResult Edit(int id)
        {
            Article Article = GetArticleById(id);
            ViewBag.Famille = new SelectList(context.Familles.ToList(), "Id", "Nom");
            ViewBag.Fournisseur = new SelectList(context.Fournisseurs.ToList(), "Id", "Nom");
            return View(Article);
        }

        public IActionResult Details(int id)
        {
            Article Article = GetArticleById(id);
            ViewBag.Famille = new SelectList(context.Familles.ToList(), "Id", "Nom");
            ViewBag.Fournisseur = new SelectList(context.Fournisseurs.ToList(), "Id", "Nom");
            return View(Article);
        }

        [HttpGet("articles")]
        public IActionResult GetArticles()
        {
            List<Article> myArticles = context.Articles.ToList();

            if (myArticles.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Articles:",
                    Article = myArticles
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun Articles dans la base de données !"
                });
            }
        }

        
        [HttpGet("articles/{articleId}")] 
        public IActionResult GetArticlesById(int articleId)
        {
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == articleId);

            if (findArticle == null)
            {
                return NotFound(new
                {
                    Message = "Aucun article trouvé avec cet ID !"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Article trouvé !",
                    Article = new ArticleDTO() { Id = findArticle.Id, Libelle = findArticle.Libelle, Prix = findArticle.Prix, Famille = findArticle.Famille, Fournisseur = findArticle.Fournisseur }
                });
            }
        }

        [HttpPost("articles")]
        public IActionResult AddArticles(ArticleDTO newArticle)
        {
            Article addArticle = new Article()
            {
                Libelle = newArticle.Libelle,
                Prix = newArticle.Prix,
                Famille = newArticle.Famille,
                Fournisseur = newArticle.Fournisseur
            };

            Fournisseur? findFournisseur = context.Fournisseurs.FirstOrDefault(x => x.Id == newArticle.Fournisseur.Id);
            if (findFournisseur == null)
            {
                return NotFound(new
                {
                    Message = "Aucun fournisseur trouvé avec cet ID !"
                });
            }
            else
            {
                addArticle.Fournisseur = findFournisseur;
            }
            
            Famille? findFamille = context.Familles.FirstOrDefault(x => x.Id == newArticle.Famille.Id);

            if (findFamille == null)
            {
                return NotFound(new
                {
                    Message = "Aucune famille trouvée avec cet ID !"
                });
            }
            else
            {
                addArticle.Famille = findFamille;
            }
            context.Articles.Add(addArticle);
            Stock addStock = new Stock() { Article = addArticle, Quantite = 0 };
            context.Stocks.Add(addStock);
            if (context.SaveChanges() > 0)
            {
                List<Article> list = context.Articles.ToList();
                ViewBag.Famille = new SelectList(context.Familles.ToList(), "Id", "Nom");
                ViewBag.Fournisseur = new SelectList(context.Fournisseurs.ToList(), "Id", "Nom");
                return View("Index", list);
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }

        
        public IActionResult EditArticle(ArticleDTO newInfos)
        {
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findArticle != null)
            {
                findArticle.Libelle = newInfos.Libelle;
                findArticle.Prix = newInfos.Prix;
                findArticle.Famille = newInfos.Famille;
                findArticle.Fournisseur = newInfos.Fournisseur;
                
                Fournisseur? findFournisseur = context.Fournisseurs.FirstOrDefault(x => x.Id == newInfos.Fournisseur.Id);

                if (findFournisseur == null)
                {
                    return NotFound(new
                    {
                        Message = "Aucun fournisseur trouvé avec cet ID !"
                    });
                }
                else
                {
                    findArticle.Fournisseur = findFournisseur;
                }

                Famille? findFamille = context.Familles.FirstOrDefault(x => x.Id == newInfos.Famille.Id);

                if (findFamille == null)
                {
                    return NotFound(new
                    {
                        Message = "Aucune famille trouvée avec cet ID !"
                    });
                }
                else
                {
                    findArticle.Famille = findFamille;
                }

                context.Articles.Update(findArticle);
                if (context.SaveChanges() > 0)
                {
                    List<Article> list = context.Articles.ToList();
                    ViewBag.Famille = new SelectList(context.Familles.ToList(), "Id", "Nom");
                    ViewBag.Fournisseur = new SelectList(context.Fournisseurs.ToList(), "Id", "Nom");
                    return View("Index", list);
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
                    Message = "Aucun article n'a été trouvé avec cet ID !"
                });
            }
        }

        public IActionResult DeleteArticle(int id)
        {
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == id);
            Stock? findStock = context.Stocks.FirstOrDefault(x => x.Article.Id == id);
            if (findArticle == null || findStock == null)
            {
                return NotFound(new
                {
                    Message = "Aucun Article trouvé avec cet ID !"
                });
            }
            else
            {
                context.Stocks.Remove(findStock);
                context.Articles.Remove(findArticle);
                if (context.SaveChanges() > 0)
                {
                    List<Article> list = context.Articles.ToList();
                    ViewBag.Famille = new SelectList(context.Familles.ToList(), "Id", "Nom");
                    ViewBag.Fournisseur = new SelectList(context.Fournisseurs.ToList(), "Id", "Nom");
                    return View("Index", list);
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
        public Article GetArticleById(int articleId)
        {
            return context.Articles.FirstOrDefault(article => article.Id == articleId);
        }
    }
}