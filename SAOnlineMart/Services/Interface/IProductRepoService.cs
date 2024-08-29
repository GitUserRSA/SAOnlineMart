using SAOnlineMart.Models;

namespace SAOnlineMart.Services.Interface
{
    public interface IProductRepoService
    {
        public bool Add(ProductsModel product);
    }
}
