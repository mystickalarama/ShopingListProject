using Microsoft.AspNetCore.Mvc;
using NotTryAnymore.Models;

namespace NotTryAnymore.Controllers
{
	public class ListController : Controller
	{
		ShoppingListContext context = new ShoppingListContext();

		[HttpGet]
		public IActionResult UserLists()
		{
			var lists = context.ShopLists.ToList();
			return View(lists);
		}
	}
}
