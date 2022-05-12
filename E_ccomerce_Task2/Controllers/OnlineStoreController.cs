using E_ccomerce_Task2.Data;
using E_ccomerce_Task2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_ccomerce_Task2.Controllers
{
    [Authorize]
    public class OnlineStoreController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private ApplicationDbContext context;

        public OnlineStoreController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }
        [HttpGet]
       
        public IActionResult Index()
        {
            var categories = (from cate in context.Categories
                              select cate).ToList();
            return View(categories);
        }

        public async Task<IActionResult> Cart()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var orderUser = from order in context.orders
                               where order.UserId == user.Id
                               select order;
                foreach(var order in orderUser)
                return View(order.products);
            }
            return View(new List<Product>());
        }
         
        public IActionResult viewProducts(int categoryId)
        {
            var products = (from proCat in context.productCategories
                           where proCat.CategorieId == categoryId
                           select proCat.product).ToList();

            return View(products);
        }
    }
}
