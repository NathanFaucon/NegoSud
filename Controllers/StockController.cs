using Cube_4.Data;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cube_4.Controllers
{
    public class StockController : Controller
    {
        private readonly ApplicationDbContext context;

        public StockController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            List<Stock> list = context.Stocks.ToList();
            ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
            Stock editStock = getStockById(id);
            return View(editStock);
        }

        public IActionResult GetStock()
        {
            List<Stock> myStock = context.Stocks.ToList();

            if (myStock.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici votre Stock :",
                    Stock = myStock
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun objet dans le Stock !"
                });
            }
        }

        public Stock getStockById(int id)
        {
            return context.Stocks.FirstOrDefault(stock => stock.Id == id);
        }
        public IActionResult GetStockById(int articleId)
        {
            Stock? findStock = context.Stocks.FirstOrDefault(x => x.ArticleId == articleId);

            if (findStock == null)
            {
                return NotFound(new
                {
                    Message = "Aucun article trouvé avec cet ID dans le stock !"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Article dans le stock trouvé !",
                    //Article = new StockDTO() { Id = findStock.Id, ArticleId = findStock.Article.Id, Quantite = findStock.Quantite }
                });
            }
        }
        
        [HttpPost("stock")]
        public IActionResult AddStock(StockDTO newStock)
        {
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == newStock.Article.Id);
            
            if (findArticle == null)
            {
                return NotFound(new
                {
                    Message = "Aucun article trouvé avec cet ID !"
                });
            }
            Stock addStock = new Stock()
            {
                Quantite = newStock.Quantite,
                ArticleId = newStock.Article.Id
            };
            context.Stocks.Add(addStock);
            if (context.SaveChanges() > 0)
            {
                List<Stock> Stocks = context.Stocks.ToList();
                ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
                return View("Index", Stocks);
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }
        
        public IActionResult EditStock(StockDTO editStock)
        {
            Stock? findStock = context.Stocks.FirstOrDefault(x => x.ArticleId == editStock.Article.Id);

            if (findStock != null)
            {
                findStock.Quantite = editStock.Quantite;
                context.Stocks.Update(findStock);
                if (context.SaveChanges() > 0)
                {
                    List<Stock> Stocks = context.Stocks.ToList();
                    ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
                    return View("Index", Stocks);
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
       
        public IActionResult DeleteStock(int Id)
        {
            Stock? findStock = context.Stocks.FirstOrDefault(x => x.ArticleId == Id);

            if (findStock == null)
            {
                return NotFound(new
                {
                    Message = "Aucun Article trouvé avec cet ID !"
                });
            }
            else
            {
                context.Stocks.Remove(findStock);
                if (context.SaveChanges() > 0)
                {
                    List<Stock> Stocks = context.Stocks.ToList();
                    ViewBag.Article = new SelectList(context.Articles.ToList(), "Id", "Libelle");
                    return View("Index", Stocks);
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
