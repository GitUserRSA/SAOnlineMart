using Microsoft.EntityFrameworkCore;
using SAOnlineMart.Data;
using SAOnlineMart.Models;
using SAOnlineMart.Services.Interface;

namespace SAOnlineMart.Services.Implementation
{
    public partial class CartActions : ICartActions
    {
        AppDbContext _dbContext;
        Guid ShoppingCartId { get; set; }

        IHttpContextAccessor _httpContextAccessor;

        public const string CartSessionKey = "CartId";

        public CartActions(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }


        public static CartActions GetCart(AppDbContext dbContext, IHttpContextAccessor httpContextAcc)
        {
            var cart = new CartActions(dbContext, httpContextAcc);
            cart.ShoppingCartId = Guid.Parse(cart.GetCartId());
            return cart;
        }

        public void AddToCart(ProductsModel products)
        {
            var cartItem = _dbContext.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == products.Id);

            if (cartItem == null)
            {
                cartItem = new CartModel
                {
                    ProductId = products.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                    Products = products

                };

                _dbContext.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            _dbContext.SaveChanges();
        }

        public string GetCartId()
        {
            var context = _httpContextAccessor.HttpContext;

            var session = context.Session;

            // Retrieve or create the cart ID
            var cartId = session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartId))
            {
                // Create a new GUID if no cart ID exists
                cartId = Guid.NewGuid().ToString();
                session.SetString(CartSessionKey, cartId);
            }

            return context.Session.GetString(CartSessionKey).ToString();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            var count = _dbContext.Carts
                .Where(cartItems => cartItems.CartId == ShoppingCartId)
                .Sum(cartItems => (int?)cartItems.Count);
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public List<CartModel> GetCartItems()
        {
            return _dbContext.Carts
                .Include(cart => cart.Products) // Include the Products in the query
                .Where(cart => cart.CartId == ShoppingCartId)
                .ToList();
        }

        public int GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            int? total = (from cartItems in _dbContext.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count *
                          cartItems.Products.PriceIncents).Sum();

            return total ?? int.MaxValue;
        }

        public void EmptyCart()
        {
            var cartItems = _dbContext.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _dbContext.Carts.Remove(cartItem);
            }
            // Save changes
            _dbContext.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = _dbContext.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _dbContext.Carts.Remove(cartItem);
                }
                // Save changes
                _dbContext.SaveChanges();
            }
            return itemCount;

        }

        public int CreateOrder(OrderModel order)
        {
            int orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetailModel
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Products.PriceIncents,
                    Quantity = item.Count,
                    OrderModel = order,
                    ProductsModels = item.Products

                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Products.PriceIncents) / 100;

                _dbContext.OrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            _dbContext.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
    }
}
