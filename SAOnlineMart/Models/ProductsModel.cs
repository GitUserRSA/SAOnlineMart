using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAOnlineMart.Models
{
    public class ProductsModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string? ProductName { get; set; }
        public int PriceIncents { get; set; }

        [ValidateNever]
        public string? ProductImage { get; set; }

        public string? ItemDescription { get; set; }
        public bool IsAvailableToBuy { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreateAt { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? UpdateAt { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
