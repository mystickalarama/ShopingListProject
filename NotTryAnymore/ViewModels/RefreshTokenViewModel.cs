namespace NotTryAnymore.ViewModels
{
	public class RefreshTokenViewModel
	{
		public string Token { get; set; } = string.Empty;
		public DateTime Created { get; set; } = DateTime.Now;
		public DateTime Expires { get; set; }
	}
}
