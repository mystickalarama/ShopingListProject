namespace NotTryAnymore.Models
{
	public class ShopListDetailModel
	{
		public int ShopListId { get; set; }
		public int ProductId { get; set; }
		public int? Quantity { get; set; }
		public decimal? Price { get; set; }
		public string? Brand { get; set; }
		public string? Description { get; set; }
		public virtual ShopListModel ShopList { get; set; } = null!;
		public virtual ProductModel Product { get; set; } = null!;
	}
}
