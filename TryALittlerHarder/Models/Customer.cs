namespace TryALittlerHarder.Models
{
	public class Customer
	{
		public int CustomerId { get; set; }
		public string CustomerName { get; set; } = null!;
		public virtual ICollection<Order> Orders { get; } = new List<Order>();

	}
}
