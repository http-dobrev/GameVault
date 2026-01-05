using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public class RegisterRequest
    {
        public string Email { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
    }
}
