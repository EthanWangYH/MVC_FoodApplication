using FoodApplication.ContextDbConfig;
using FoodApplication.Models;
using FoodApplication.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodApplication.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IData data;
        private FoodDbContext db;
        public CartController(IData data, FoodDbContext db)
        {
            this.data = data;
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var user = await data.GetUser(User);
            var cartList = db.Carts.Where(c=>c.UserId==user.Id).ToList();
            return View(cartList);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCart(Cart cart)
        {
            var user = await data.GetUser(User);
            cart.UserId = user?.Id;
            if (ModelState.IsValid)
            {
                db.Add(cart);
                db.SaveChanges();
                return Ok();
            }
            return BadRequest(cart);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddedCarts()
        {
            var user =await data.GetUser(User);
            var carts = db.Carts.Where(c => c.UserId == user.Id).Select(c=>c.RecipeId).ToList();
            return Ok(carts);
        }

        [HttpPost]
        public  IActionResult RemoveCartFromList(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var cart = db.Carts.Where(c=>c.RecipeId==Id).FirstOrDefault();
                if (cart != null)
                {
                    db.Carts.Remove(cart);
                    db.SaveChanges();
                }
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async  Task<IActionResult> GetCartList()
        {
            var user = await data.GetUser(User);
            var cartList = db.Carts.Where(c=>c.UserId==user.Id!).Take(3).ToList();
            return PartialView("_CartList", cartList);
        }
    }
}
