using Logic.Interfaces;
using UI.Mappers;
using UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                var userGame = UserGameModelMapper.ToUserGame(vm);
                _userGameService.CreateUserGame(userGame);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logic Error: {ex.Message}");

                // You can also pass the message to the user
                return Content(ex.Message);
            }
        }

        public int GetUserIdFromClaims()
        {
            int claimId;
            return claimId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
