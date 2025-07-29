using BugTrackerMVC.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerMVC.Services.Factories
{
    public class BTUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<BTUser, IdentityRole>
    {
        public BTUserClaimsPrincipalFactory(UserManager<BTUser> userManager, 
                                            RoleManager<IdentityRole> roleManager,
                                            IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        {
   
        }
    }
}