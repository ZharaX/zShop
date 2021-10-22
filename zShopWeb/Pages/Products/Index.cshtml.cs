using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		private readonly Service.Querys.IProductFilter _productFilter;
		#endregion
		#region SEARCH N FILTER
		[BindProperty(SupportsGet = true)]
		public string SearchString { get; set; }

		[BindProperty(SupportsGet = true)]
		public int CurrentPage { get; set; } = 1;

		[BindProperty(SupportsGet = true)]
		public int PageSize { get; set; } = 10;

		public int Count { get; set; }

		public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
		#endregion

		public List<Service.DTO.ProductDTO> Products { get; private set; }
		
		public Models.NewOrder NewOrder { get; set; }


		public IndexModel(ISiteFunctions siteFunctions)
		{ 
			_siteFunctions = siteFunctions;
			_productFilter = new Service.Querys.ProductFilterService(_siteFunctions);
		}

		public IActionResult OnGet()
		{
			if (TempData.ContainsKey("NewOrder"))
			{
				NewOrder = TempData.Get<Models.NewOrder>("NewOrder");
				TempData.Set("NewOrder", NewOrder);
			}

			Products = _productFilter.GetProductsContainingString(SearchString, CurrentPage, PageSize, Service.Querys.OrderBy.Descending, Service.Querys.FilterBy.Price);

			if (!string.IsNullOrEmpty(SearchString)) { TempData.Set("query", SearchString); }

			return Page();
		}

		#region ADD TO CART
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
		#endregion
	}

	public enum PageSizeEnum
	{
		[Display(Name = "2")]
		_2 = 2,
		[Display(Name = "5")]
		_5 = 5,
		[Display(Name = "10")]
		_10 = 10,
	}
}
