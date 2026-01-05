using Logic.Entities;
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
                .Select(GameViewModelMapper.ToGameListViewModel)
                .ToList();

            return View(games);
        }

        // GET: /Game/Details/5
        [HttpGet]
        public IActionResult Details(Game game)
        {
            try
            {
                var foundGame = _gameService.GetGame(game);
                var vm = GameViewModelMapper.ToGameListViewModel(foundGame);
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
            var viewModel = new GameFormViewModel();
            
            viewModel.ReleaseDate = DateTime.UtcNow;
            viewModel.PegiOptions = EnumHelper.GetPegiOptions();

            PopulateDropdowns(viewModel);

            return View(viewModel);
        }

        // POST: /Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GameFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PegiOptions = EnumHelper.GetPegiOptions();

                PopulateDropdowns(viewModel);

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
        public IActionResult Edit(Game game)
        {
            if (game.Id <= 0)
                return BadRequest();

            var foundGame = _gameService.GetGame(game);
            if (foundGame == null)
                return NotFound();

            var viewModel = GameViewModelMapper.ToGameFormViewModel(foundGame);

            PopulateDropdowns(viewModel);
            viewModel.PegiOptions = EnumHelper.GetPegiOptions();

            return View(viewModel);
        }

        // POST: /Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, GameFormViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                viewModel.PegiOptions = EnumHelper.GetPegiOptions();
                PopulateDropdowns(viewModel);
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
                PopulateDropdowns(viewModel);
                viewModel.PegiOptions = EnumHelper.GetPegiOptions();
                return View(viewModel);
            }
        }

        // POST: /Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Game game)
        {
            try
            {
                _gameService.ArchiveGame(game);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        private void PopulateDropdowns(GameFormViewModel vm)
        {
                vm.PegiOptions = EnumHelper.GetPegiOptions();

            vm.Genres = _genreService.GetAllGenres()
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToList();

            vm.Developers = _developerService.GetAllDevelopers()
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                .ToList();

            vm.Publishers = _publisherService.GetAllPublishers()
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();
        }
    }
}
