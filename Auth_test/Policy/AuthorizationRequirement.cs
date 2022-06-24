using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth_test.Policy
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public AuthorizationRequirement(int age)
        {
            MinimumAge = age;
        }

        protected int MinimumAge { get; set; }
    }
}
