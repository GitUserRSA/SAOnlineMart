using SAOnlineMart.Models;

namespace SAOnlineMart.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartModel> CartItems { get; set; }
        public int CartTotal { get; set; }
    }
}
