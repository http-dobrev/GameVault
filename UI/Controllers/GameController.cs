using Logic.Enums;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Helpers;
using UI.Mappers;
using UI.Models;

namespace UI.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IDeveloperService _developerService;
        private readonly IGenreService _genreService;
        private readonly IPublisherService _publisherService;

        public GameController(IGameService gameService, IDeveloperService developerService, IGenreService genreService, IPublisherService publisherService)
        {
            _gameService = gameService;
            _developerService = developerService;
            _genreService = genreService;
            _publisherService = publisherService;
        }

        // GET: /Game
        [HttpGet]
        public IActionResult Index()
        {
            var games = _gameService
                .GetAllGames()
                .Select(GameViewModelMapper.ToViewModel)
                .ToList();

            return View(games);
        }

        // GET: /Game/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var game = _gameService.GetById(id);
                var vm = GameViewModelMapper.ToViewModel(game);
                return View(vm);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        // GET: /Game/Create
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new GameViewModel();

            viewModel.ReleaseDate = DateTime.UtcNow;
            viewModel.PegiOptions = EnumHelper.GetPegiOptions();

            viewModel.Developers = _developerService.GetAllDevelopers()
                    .Select(d => new SelectListItem(d.Name, d.Id.ToString()))
                    .ToList();

            viewModel.Genres = _genreService.GetAllGenres()
                    .Select(g => new SelectListItem(g.Name, g.Id.ToString()))
                    .ToList();

            viewModel.Publishers = _publisherService.GetAllPublishers()
                    .Select(p => new SelectListItem(p.Name, p.Id.ToString()))
                    .ToList();

            return View(viewModel);
        }

        // POST: /Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PegiOptions = EnumHelper.GetPegiOptions();

                viewModel.Developers = _developerService.GetAllDevelopers()
                    .Select(d => new SelectListItem(d.Name, d.Id.ToString()))
                    .ToList();

                viewModel.Genres = _genreService.GetAllGenres()
                    .Select(g => new SelectListItem(g.Name, g.Id.ToString()))
                    .ToList();

                viewModel.Publishers = _publisherService.GetAllPublishers()
                    .Select(p => new SelectListItem(p.Name, p.Id.ToString()))
                    .ToList();

                return View(viewModel);
            }

            var entity = GameViewModelMapper.ToEntity(viewModel);

            try
            {
                _gameService.CreateGame(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            var game = _gameService.GetById(id);
            if (game == null)
                return NotFound();

            var viewModel = GameViewModelMapper.ToViewModel(game);
            return View(viewModel);
        }

        // POST: /Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, GameViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                viewModel.PegiOptions = EnumHelper.GetPegiOptions();
                return View(viewModel);
            }
   
            var entity = GameViewModelMapper.ToEntity(viewModel);

            try
            {
                _gameService.UpdateGame(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        // GET: /Game/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var game = _gameService.GetById(id);
                var vm = GameViewModelMapper.ToViewModel(game);
                return View(vm);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        // POST: /Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _gameService.DeleteGame(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}
