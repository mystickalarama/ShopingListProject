using System.ComponentModel.DataAnnotations;

namespace NotTryAnymore.Models
{
	public class CategoryModel
	{
		[Key]
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = null!;
		public virtual ICollection<ProductModel> Products { get; } = new List<ProductModel>();
	}
}
