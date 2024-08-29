using System.ComponentModel.DataAnnotations;

namespace SAOnlineMart.Models
{
    public class OrderModel
    {
        [Key]
        public int OrderId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Total { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; }
    }
}
