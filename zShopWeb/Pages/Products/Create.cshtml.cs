using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service;

namespace zShopWeb.Pages.Products
{
	public class CreateProductModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public CreateProductModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		public IActionResult OnGet() { return Page(); }

		[BindProperty]
		public Service.DTO.ProductDTO Product { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page();

			_siteFunctions.PerformAction(ActionType.Create, FunctionName.Product, Product);

			return RedirectToPage("./Index");
		}
	}
}
