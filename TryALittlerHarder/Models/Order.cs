using System.ComponentModel.DataAnnotations;

namespace TryALittlerHarder.Models
{
	public class Order
	{
		public int OrderId { get; set; }
		public string OrderName { get; set; } = null!;
		public int? CustomerId { get; set; }
		public virtual Customer? Customer { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

	}
}
