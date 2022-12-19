using System.ComponentModel.DataAnnotations;

namespace TryALittlerHarder.Models
{
	public class Product
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string? Image { get; set; }
		public int? CategoryId { get; set; }
		public virtual Category? Category { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
	}
}
