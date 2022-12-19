using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotTryAnymore.Models;
using NotTryAnymore.ViewModels;

namespace NotTryAnymore.Controllers
{
	public class ProductController : Controller
	{
		ShoppingListContext context = new ShoppingListContext();

		[HttpGet]
		public IActionResult ProductPage()
		{
			var products = context.Products.ToList();
			return View(products);
		}

		[HttpGet]
		public IActionResult AddProduct()
		{
			List<SelectListItem> selectList = (from a in context.Categories.ToList()
											   select new SelectListItem
											   {
												   Text = a.CategoryName,
												   Value = a.CategoryId.ToString()
											   }).ToList();
			ViewBag.CategoryList = selectList;
			return View();
		}


		[HttpPost]
		public IActionResult AddProducts(ProductViewModel product)
		{
			ProductModel productModel = new ProductModel();

			if (product.ImageURL != null)
			{
				productModel.ProductName = product.ProductName;
				var ex = Path.GetExtension(product.ImageURL.FileName);
				var newImageName = Guid.NewGuid() + ex;
				var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", newImageName);
				var stream = new FileStream(location, FileMode.Create);
				product.ImageURL.CopyTo(stream);
				productModel.ProductImage = newImageName;
			}
			productModel.CategoryId = product.CategoryId;
			productModel.ProductName = product.ProductName;
			context.Products.Add(productModel);
			context.SaveChanges();

			return RedirectToAction("ProductPage");
		}

		public IActionResult UpdateProducts(ProductViewModel product)
		{
			ProductModel productModel = new ProductModel();
			var products = context.Products.Find(product.ProductId);
			if (product.ImageURL != null)
			{
				var ex = Path.GetExtension(product.ImageURL.FileName);
				var newImageName = Guid.NewGuid() + ex;
				var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", newImageName);
				var stream = new FileStream(location, FileMode.Create);
				product.ImageURL.CopyTo(stream);
				context.Products.Find(product.ProductId).ProductImage = newImageName;
			}
			products.CategoryId = product.CategoryId;
			products.ProductName = product.ProductName;
			context.SaveChanges();
			return RedirectToAction("ProductPage");
		}

		public IActionResult UpdateProduct(int id)
		{
			List<SelectListItem> selectList = (from a in context.Categories.ToList()
											   select new SelectListItem
											   {
												   Text = a.CategoryName,
												   Value = a.CategoryId.ToString()
											   }).ToList();
			ViewBag.CategoryList = selectList;
			var product = context.Products.Find(id);
			return View("UpdateProduct", product);
		}

		public IActionResult DeleteProduct(int id)
		{
			ProductModel product = context.Products.Find(id);

			if (product == null)
			{
				return NotFound();
			}
			context.Products.Remove(product);
			context.SaveChanges();
			return RedirectToAction("ProductPage");
		}
	}
}
