using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace zShopWeb.Pages.Products
{
	/// <summary>
	/// Product Index Page Model
	/// </summary>
	public class IndexModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public List<Service.DTO.ProductDTO> Products { get; private set; }

		[BindProperty(SupportsGet = true)]
		public string SearchString { get; set; }

		public Models.NewOrder NewOrder { get; set; }


		public IndexModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; LoadProducts(); }

		public void OnGet()
		{
			if (TempData.ContainsKey("NewOrder"))
			{
				NewOrder = TempData.Get<Models.NewOrder>("NewOrder");
				TempData.Set("NewOrder", NewOrder);
			}

			Products = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);

			if(!string.IsNullOrEmpty(SearchString))
			{
				Products = Products.Where(p => p.Name.ToLower().Contains(SearchString.ToLower()) || p.Description.ToLower().Contains(SearchString.ToLower())).ToList();
			}
		}

		public IActionResult OnGetAddToCart(int? id)
		{
			if (TempData.ContainsKey("NewOrder"))
				NewOrder = TempData.Get<Models.NewOrder>("NewOrder");

			if (NewOrder == null)
				NewOrder = new Models.NewOrder { OrderProducts = new List<int>() };

			if(id != 0 || id != null)
			{ 
				NewOrder.OrderProducts.Add((int)id);

				var productList = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);
				var total = 0.00m;

				foreach (int idx in NewOrder.OrderProducts)
				{
					total += productList.Where(p => p.ProductID == idx).Select(p => p.Price).Single();
				}

				NewOrder.OrderTotalPrice = total;
			}

			TempData.Set("NewOrder", NewOrder);

			return RedirectToPage();
		}

		private void LoadProducts()
		{
			Products = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);
		}
	}
}
