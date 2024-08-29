using System.ComponentModel.DataAnnotations;

namespace SAOnlineMart.Models
{
    public class OrderDetailModel
    {
        [Key]
        public int OrderDetailId { get; set; }
        public Guid ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public virtual ProductsModel ProductsModels { get; set; }
        public virtual OrderModel OrderModel { get; set; }
    }
}
