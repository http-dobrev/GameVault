using Logic.Interfaces;
using UI.Mappers;
using UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UI.Helpers;
using System.Diagnostics;

namespace UI.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IUserGameService _userGameService;
        public LibraryController(IUserGameService userGameService)
        {
            _userGameService = userGameService;
        }

        // GET: /Library
        [HttpGet]
        public IActionResult Index()
        {
            int UserId = GetUserIdFromClaims();
            var userGames = _userGameService
                .GetAllUserGames(UserId)
                .ToList();

            var libraryList = new List<LibraryListItemVM>();

            foreach (var userGame in userGames)
            {
                var vm = UserGameModelMapper.ToLibraryListItemVM(userGame);
                libraryList.Add(vm);
            }
            return View(libraryList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserGameFormViewModel vm)
        {
            vm.UserId = GetUserIdFromClaims();

            try
            {
                var userGame = UserGameModelMapper.ToUserGameFromFormViewModel(vm);
                _userGameService.CreateUserGame(userGame);
                return RedirectToAction("Index","Game");
            }
            catch (ArgumentException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logic Error: {ex.Message}");

                // You can also pass the message to the user
                return Content(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Edit(int gameId)
        {
            var userId = GetUserIdFromClaims();
            var userGame = _userGameService.GetUserGame(userId, gameId);
            var vm = UserGameModelMapper.ToUserGameEditViewModel(userGame);
            PopulateDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserGameEditViewModel vm)
        {
            try
            {
                var userGame = UserGameModelMapper.ToUserGameFromEditViewModel(vm);
                _userGameService.UpdateUserGame(userGame);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine($"Logic Error: {ex.Message}");
                PopulateDropdowns(vm);
                // You can also pass the message to the user
                return Content(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int gameId)
        {
            var userId = GetUserIdFromClaims();
            _userGameService.DeleteUserGame(userId, gameId);
            return RedirectToAction(nameof(Index));
        }

        public int GetUserIdFromClaims()
        {
            int claimId;
            return claimId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public void PopulateDropdowns(UserGameEditViewModel vm)
        {
            vm.StatusOptions = EnumHelper.GetUserGameStatusOptions();
            vm.PlatformOptions = EnumHelper.GetUserGamePlatformOptions();
        }
    }
}
