using FoodApplication.ContextDbConfig;
using FoodApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodApplication.Controllers
{
    public class RecipeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FoodDbContext _foodDbContext;
        public RecipeController(UserManager<ApplicationUser> userManager, FoodDbContext foodDbContext)
        {
            _userManager = userManager;
            _foodDbContext = foodDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetRecipeCard([FromBody] List<Recipe> recpies)
        {
            return PartialView("_RecipeCard", recpies);
        }
        public IActionResult Search([FromQuery] string recipe)
        {
            ViewBag.Recipe = recipe;
            return View();
        }
        public IActionResult Order([FromQuery] string id)
        {
            ViewBag.Id = id;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ShowOrder(OrderRecipeDetails orderRecipeDetails)
        {
            Random random = new Random();
            ViewBag.Price = Math.Round(random.Next(15, 50) / 5.0) * 5;
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserId = user?.Id;
            ViewBag.Address = user?.Address;
            return PartialView("_ShowOrder", orderRecipeDetails);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Order([FromForm]Order order)
        {
            order.OrderDate = DateTime.Now;
            if (ModelState.IsValid)
            {              
                _foodDbContext.Add(order);
                 _foodDbContext.SaveChanges();
                return RedirectToAction("Index","Recipe");
            }
            return RedirectToAction("Order", "Recipe", new {id=order.Id});
        }
    }
}
