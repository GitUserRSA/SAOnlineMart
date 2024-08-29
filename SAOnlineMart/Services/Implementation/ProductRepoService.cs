using SAOnlineMart.Data;
using SAOnlineMart.Models;
using SAOnlineMart.Services.Interface;

namespace SAOnlineMart.Services.Implementation
{
    public class ProductRepoService : IProductRepoService
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public bool Add(ProductsModel product)
        {
            try
            {
                _appDbContext.Products.Add(product);
                _appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
