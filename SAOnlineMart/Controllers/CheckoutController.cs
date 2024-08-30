using Microsoft.AspNetCore.Mvc;
using SAOnlineMart.Data;
using SAOnlineMart.Models;
using SAOnlineMart.Services.Implementation;

namespace SAOnlineMart.Controllers
{
    public class CheckoutController : Controller
    {

        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CheckoutController(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]

        public ActionResult AddressAndPayment([FromForm][Bind("FirstName,LastName,Address,City,State,PostalCode,Country,UpdateAt,Phone,Email")] OrderModel order)
        {
            try
            {
                order.Email = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                //Save Order
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();


                var cart = CartActions.GetCart(_dbContext, _httpContextAccessor);
                cart.CreateOrder(order);

                return RedirectToAction("Complete", new { id = order.OrderId });

            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }

        }

        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = _dbContext.Orders.Any(
                o => o.OrderId == id &&
                o.Email == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

    }
}
