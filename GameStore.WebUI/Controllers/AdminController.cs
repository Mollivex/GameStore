using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        readonly IGameRepository repository;
        public AdminController (IGameRepository repo)
        {
            this.repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Games);
        }

        public ViewResult Edit(int gameId)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.GameId == gameId);
            return View(game);
        }

        // Edit() overloaded version for save changes
        [HttpPost]
        public ActionResult Edit (Game game)
        {
            if (ModelState.IsValid)
            {
                repository.SaveGame(game);
                TempData["message"] = string.Format("Game changes \"{0}\" was saved", game.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Something wrong happened with data values
                return View(game);
            }
        }
    }
}