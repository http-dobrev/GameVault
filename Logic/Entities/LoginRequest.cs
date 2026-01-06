using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public class LoginRequest
    {
        public string EmailOrUsername { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
