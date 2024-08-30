using Microsoft.AspNetCore.Mvc;
using SAOnlineMart.Data;
using SAOnlineMart.Services.Implementation;
using SAOnlineMart.ViewModels;

namespace SAOnlineMart.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {

            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = CartActions.GetCart(_context, _httpContextAccessor);

            // Set up view model
            var viewModel = new ShoppingCartViewModel
            {
                //Get the cart items and their total
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Set CartCount for the partial view
            ViewData["CartCount"] = cart.GetCartItems().Count();

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/CartSummary
        public ActionResult CartSummary()
        {
            var cart = CartActions.GetCart(_context, _httpContextAccessor);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }

        public IActionResult AddToCart(Guid productId)
        {
            var addedProduct = _context.Products.Single(
                product => product.Id == productId
                );

            var cart = CartActions.GetCart(_context, _httpContextAccessor);

            cart.AddToCart(addedProduct);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartModel = await _context.Carts.FindAsync(id);

            if (cartModel != null)
            {
                _context.Carts.Remove(cartModel);
            }
            await _context.SaveChangesAsync();

            return RedirectToActionPermanent(nameof(Index));
        }
    }
}
