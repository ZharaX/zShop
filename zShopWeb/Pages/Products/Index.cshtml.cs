using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace zShopWeb.Pages.Products
{
	public class IndexModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public List<Service.DTO.ProductDTO> Products { get; private set; }
		[BindProperty(SupportsGet = true)]
		public string SearchString { get; set; }

		public IndexModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; LoadProducts(); }

		public void OnGet()
		{
			Products = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);

			if(!string.IsNullOrEmpty(SearchString))
			{
				Products = Products.Where(p => p.Name.ToLower().Contains(SearchString.ToLower()) || p.Description.ToLower().Contains(SearchString.ToLower())).ToList();
			}
		}


		private void LoadProducts()
		{
			Products = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);
		}
	}
}
