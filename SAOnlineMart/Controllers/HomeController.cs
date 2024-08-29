using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAOnlineMart.Data;
using SAOnlineMart.Models;
using SAOnlineMart.Services.Implementation;
using SAOnlineMart.Services.Interface;

namespace SAOnlineMart.Controllers
{
    public class HomeController(IRoleManagerService roleService, IAccountSeederService accountSeederService, AppDbContext context) : Controller
    {

        private readonly IRoleManagerService _roleService = roleService;

        private readonly IAccountSeederService _accountSeederService = accountSeederService;

        private readonly AppDbContext _context = context;

        public async Task<IActionResult> Index() //Home page index
        {
            Console.WriteLine("Init roles...");

            await _roleService.SeedManagerRoles(); //Initialize roles

            await _accountSeederService.SeedAccounts(); //Seed the admin account

            return View(await _context.Products.ToListAsync()); //List all the products currently available
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
