using System.ComponentModel.DataAnnotations;

namespace NotTryAnymore.Models
{
	public class ProductModel
	{
		[Key]
		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string? ProductImage { get; set; }
		public int? CategoryId { get; set; }
		public virtual CategoryModel? Category { get; set; }
		public virtual ICollection<ShopListDetailModel> ShopListDetail { get; } = new List<ShopListDetailModel>();
	}
}
