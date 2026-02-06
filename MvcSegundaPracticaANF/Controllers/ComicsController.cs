using Microsoft.AspNetCore.Mvc;
using MvcSegundaPracticaANF.Models;
using MvcSegundaPracticaANF.Repositories;

namespace MvcSegundaPracticaANF.Controllers
{
    public class ComicsController : Controller
    {
        ComicsRepository repo;

        public ComicsController()
        {
            this.repo = new ComicsRepository();
        }
        public IActionResult Index()
        {
            List<Comic> model = this.repo.GetComics();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            Comic model = this.repo.GetComic(id);
            return View(model);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comic comic)
        {

            await this.repo.CreateComic(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }
    }

}
