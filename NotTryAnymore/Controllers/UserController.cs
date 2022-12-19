using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotTryAnymore.Models;
using NotTryAnymore.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace NotTryAnymore.Controllers
{
	public class UserController : Controller
	{
		ShoppingListContext context = new ShoppingListContext();
		private readonly IConfiguration configuration;
		readonly UserModel user = new();

		public UserController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		public IActionResult RegisterPage()
		{
			return View();
		}

		public IActionResult LoginPage()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel userRegister)
		{
			UserModel userModel = new UserModel();

			if (context.Users.Any(a => a.Mail == userRegister.Mail))
			{
				return BadRequest("User already exist");
			}

			if (Request.Form["Password"] != userRegister.ConfirmPassword)
			{
				return BadRequest("Passwords not match");
			}

			CreatePasswordHash(userRegister.Password,
				 out byte[] passwordHash,
				 out byte[] passwordSalt);
			userModel.UserName = userRegister.UserName;
			userModel.UserSurname = userRegister.UserSurname;
			userModel.Mail = userRegister.Mail;
			userModel.PasswordHash = passwordHash;
			userModel.PasswordSalt = passwordSalt;
			userModel.VerificationToken = CreateToken(userModel);

			context.Users.Add(userModel);
			await context.SaveChangesAsync();

			return RedirectToAction("LoginPage");
		}

		[HttpPost]
		public async Task<ActionResult<string>> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];

			if (!user.RefreshToken.Equals(refreshToken))
			{
				return Unauthorized("Invalid Refresh Token");
			}
			else if (user.TokenExpires < DateTime.Now)
			{
				return Unauthorized("Token Expired");
			}

			string token = CreateToken(user);
			var newRefreshToken = GenerateRefreshToken();
			SetRefreshToken(newRefreshToken);

			return Ok(token);
		}

		private static RefreshTokenViewModel GenerateRefreshToken()
		{
			var refreshToken = new RefreshTokenViewModel()
			{
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				Expires = DateTime.Now.AddDays(7),
				Created = DateTime.Now

			};

			return refreshToken;
		}

		private void SetRefreshToken(RefreshTokenViewModel newRefreshToken)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = newRefreshToken.Expires
			};
			Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

			user.RefreshToken = newRefreshToken.Token;
			user.TokenCreated = newRefreshToken.Created;
			user.TokenExpires = newRefreshToken.Expires;
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel userLogin)
		{
			UserModel useruserModel = new UserModel();
			var user = await this.context.Users.FirstOrDefaultAsync(a => a.Mail == userLogin.Mail);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			if (user.VerificationToken == null)
			{
				return BadRequest("Not verified");
			}

			if (!VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
			{
				return BadRequest("Password is incorrect");
			}

			return RedirectToAction("UserLists", "List");
		}


		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}

		private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512(passwordSalt);
			var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(passwordHash);
		}
		private string CreateToken(UserModel user)
		{
			if (user.Mail == "mystickalarama@force.com")
			{
				List<Claim> claims = new()
				{
				new Claim(ClaimTypes.NameIdentifier, user.UserName),
				new Claim(ClaimTypes.Role, "Admin")
				};

				var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
					this.configuration.GetSection("AppSettings:Token").Value));

				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

				var token = new JwtSecurityToken(
					claims: claims,
					expires: DateTime.Now.AddDays(1),
					signingCredentials: creds);

				var jwt = new JwtSecurityTokenHandler().WriteToken(token);

				return jwt;
			}

			else
			{
				List<Claim> claims = new()
				{
				new Claim(ClaimTypes.NameIdentifier, user.UserName),
				new Claim(ClaimTypes.Role, "User")
				};

				var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
					this.configuration.GetSection("AppSettings:Token").Value));

				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

				var token = new JwtSecurityToken(
					claims: claims,
					expires: DateTime.Now.AddDays(1),
					signingCredentials: creds);

				var jwt = new JwtSecurityTokenHandler().WriteToken(token);

				return jwt; 
			}
		}
	}
}
