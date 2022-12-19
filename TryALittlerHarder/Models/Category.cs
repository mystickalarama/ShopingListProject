using System.ComponentModel.DataAnnotations;

namespace TryALittlerHarder.Models
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = null!;
		public virtual ICollection<Product> Products { get; } = new List<Product>();

	}
}
