using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth_test.Policy
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public AuthorizationRequirement() {
            Console.WriteLine("권한있음");
        }
    }
}
