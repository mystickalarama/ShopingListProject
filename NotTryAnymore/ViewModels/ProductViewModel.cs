namespace NotTryAnymore.ViewModels
{
	public class ProductViewModel
	{
		public int ProductId { get; set; }
		public int CategoryId { get; set; }
		public string ProductName { get; set; }
		public IFormFile ImageURL { get; set; }
	}
}
