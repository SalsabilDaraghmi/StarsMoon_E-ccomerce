using E_ccomerce_Task2.Data;
using E_ccomerce_Task2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_ccomerce_Task2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        private RoleManager<IdentityRole> _roleManager ;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _SignInManager;




            public HomeController(ILogger<HomeController> logger,
                                  ApplicationDbContext context,
                                  RoleManager<IdentityRole> roleManager,
                                  UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _context=context;
            _roleManager= roleManager;
            _userManager= userManager;
            _SignInManager = signInManager;
    }

        public async Task<IActionResult> Index()
        {

            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
               
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                ApplicationUser adminUser = new ApplicationUser();
                adminUser.UserName = "Admin@gmail.com";
                adminUser.Email = "Admin@gmail.com";
                adminUser.FirstName = "Admin";
                adminUser.LastName = "Admin";
                string password = "P@$$w0rd";

                var chekUser =await _userManager.CreateAsync(adminUser, password);

                //Add default User to Role Admin    
                if (chekUser.Succeeded)
                {
                 
                    var result1 =await _userManager.AddToRoleAsync(adminUser, "Admin");

                }
            }

            // creating Creating Manager role     
            if (!_roleManager.RoleExistsAsync("Customer").Result)
            {

                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Customer";
                await _roleManager.CreateAsync(role);

            }

            if (_SignInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "OnlineStore");
            }
                var products =(from product in _context.Products
                          select product).Take(5);
            return View(products);
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