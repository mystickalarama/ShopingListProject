using System.ComponentModel.DataAnnotations;

namespace NotTryAnymore.Models
{
	public class ShopListModel
	{
		[Key]
		public int ShopListId { get; set; }
		public string ShopListName { get; set; } = null!;
		public int? UserId { get; set; }
		public virtual UserModel? User { get; set; }
		public virtual ICollection<ShopListDetailModel> ShopListDetails { get; } = new List<ShopListDetailModel>();
	}
}
