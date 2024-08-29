
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAOnlineMart.Data;
using SAOnlineMart.Services.Interface;
using SAOnlineMart.Services.Implementation;

namespace SAOnlineMart
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options => //Database context added to DI to read and save data
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //Connection string grabbed from appsettings.json
                options.UseSqlServer(connectionString);

            });

            //Scaffolded identity pages for user registration and login
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>() //Add identity role to the service
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddScoped<IFileService, FileService>(); //DI for image handling
            builder.Services.AddScoped<IProductRepoService, ProductRepoService>(); //Di for adding products to db

            builder.Services.AddScoped<IRoleManagerService, RoleManagerService>(); //DI for role manager service
            builder.Services.AddScoped<IAccountSeederService, AccountSeederService>(); //DI for account seeding services

            builder.Services.AddRazorPages(); //Add razor pages services

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages(); //Map the pages otherwise partial view wont work

            app.Run();
        }

    }
}
