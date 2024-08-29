using SAOnlineMart.Models;

namespace SAOnlineMart.Services.Interface
{
    public partial interface ICartActions
    {
        public void AddToCart(ProductsModel products);

        public string GetCartId();

        public List<CartModel> GetCartItems();

        public int GetTotal();

        public void EmptyCart();

        public int RemoveFromCart(int id);

        public int CreateOrder(OrderModel order);
    }
}
