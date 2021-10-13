using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service;

namespace S21DMH3B11_zShop.Pages.Customer
{
	public class CreateCustomerModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public CreateCustomerModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		public IActionResult OnGet() { return Page(); }

		[BindProperty]
		public Service.DTO.CustomerDTO Customer { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page();

			_siteFunctions.PerformAction(ActionType.Create, FunctionName.Customer, Customer);

			return RedirectToPage("./Index");
		}
	}
}
