using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NotTryAnymore.ViewModels
{
	public class LoginViewModel
	{
		[EmailAddress]
		public string Mail { get; set; } = null!;
		[Required, MinLength(8), MaxLength(15), NotMapped]
		public string Password { get; set; }
	}
}
