using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NetMind.Models
{
    public class User: IdentityUser<int>
    {
        public User()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public User(string userName) : this()
        {
            UserName = userName;
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }

    public class CustomRole: IdentityRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }

    public class CustomRoleClaim : IdentityRoleClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }
}
