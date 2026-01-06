using Logic.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<SelectListItem> GetPegiOptions()
        {
            return Enum.GetValues(typeof(PegiAge))
                .Cast<PegiAge>()
                .Select(p => new SelectListItem
                {
                    Value = ((int)p).ToString(),
                    Text = p.ToString()
                });
        }
        public static IEnumerable<SelectListItem> GetUserGameStatusOptions()
        {
            return Enum.GetValues(typeof(UserGameStatus))
                .Cast<UserGameStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                });
        }
        public static IEnumerable<SelectListItem> GetUserGamePlatformOptions()
        {
            return Enum.GetValues(typeof(UserGamePlatform))
                .Cast<UserGamePlatform>()
                .Select(p => new SelectListItem
                {
                    Value = ((int)p).ToString(),
                    Text = p.ToString()
                });
        }
    }
}
