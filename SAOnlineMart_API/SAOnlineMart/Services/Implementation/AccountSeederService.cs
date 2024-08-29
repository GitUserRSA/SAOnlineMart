using Microsoft.AspNetCore.Identity;

namespace SAOnlineMart.Services.Implementation
{
    public class AccountSeederService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountSeederService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAccounts() //Task that handles creating an admin account for testing if there is not one
        {
            string email = "admin@admin.com";
            string password = "Test1234,";



            if (await _userManager.FindByEmailAsync(email) == null) //Look for the admin email
            {
                var user = new IdentityUser(); //Create a new user if the email cant be found

                user.Email = email;
                user.UserName = email;

                await _userManager.CreateAsync(user, password); //Create the account with the user and pass variable

                await _userManager.AddToRoleAsync(user, "Admin"); //Add the new account to the role of Admin
            }
        }
    }
}
