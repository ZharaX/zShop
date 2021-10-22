using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace zShopWeb.Pages.Products
{
	public class EditModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		[BindProperty]
		public Service.DTO.ProductDTO Product { get; set; }


		public EditModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		public IActionResult OnGet(int id)
		{
			var products = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Product, id);
			Product = products.Where(p => p.ProductID == id).Single();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page();

			if ((bool)_siteFunctions.PerformAction(ActionType.Update, FunctionName.Product, Product))
			{
				TempData.Set("Message", "Product Updated");
				return Page();
			}

			return RedirectToPage("/Index");
		}
	}
}
