using Microsoft.AspNetCore.Identity;
using SAOnlineMart.Services.Interface;

namespace SAOnlineMart.Services.Implementation
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedManagerRoles()
        {
            Console.WriteLine("Function accessed");

            var roles = new[] { "Admin", "User", "Manager" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
