using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NotTryAnymore.Models
{
	public class UserModel
	{
		[Key]
		public int UserId { get; set; }
		public string UserName { get; set; } = null!;
		public string UserSurname { get; set; } = null!;
		[EmailAddress]
		public string Mail { get; set; } = null!;
		[Required, MinLength(8), MaxLength(15), NotMapped]
		public string Password { get; set; }
		[Required, Compare("Password"), NotMapped]
		public string ConfirmPassword { get; set; } = null!;
		public string VerificationToken { get; set; } = null!;
		public byte[] PasswordHash { get; set; } = null!;
		public byte[] PasswordSalt { get; set; } = null!;
		public virtual ICollection<ShopListModel> ShopLists { get; } = new List<ShopListModel>();

		[NotMapped]
		public string RefreshToken { get; set; } = string.Empty;
		[NotMapped]
		public DateTime TokenCreated { get; set; } = DateTime.Now;
		[NotMapped]
		public DateTime TokenExpires { get; set; } = DateTime.Now.AddDays(7);
	}
}
