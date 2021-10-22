using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Service;

namespace zShopWeb.Pages.Products
{
	[Authorize(Roles = "Admin")]
	public class ListModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public ListModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		public IList<Service.DTO.ProductDTO> Products { get; set; }


		public async Task OnGetAsync()
		{
			Products = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);
		}

		public IActionResult OnPostDelete(int pID)
		{
			_siteFunctions.PerformAction(ActionType.Delete, FunctionName.Product, pID);
			return RedirectToPage("/Products/List");
		}
	}
}
