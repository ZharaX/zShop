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

		public IndexModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; LoadProducts(); }

		public void OnGet()
		{
		}


		private void LoadProducts()
		{
			Products = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);
		}
	}
}
