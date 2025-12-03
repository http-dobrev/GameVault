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
    }
}
