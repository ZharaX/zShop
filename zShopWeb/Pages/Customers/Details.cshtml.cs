using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace S21DMH3B11_zShop.Pages.Customer
{
	public class DetailsModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public DetailsModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		public Service.DTO.CustomerDTO Customer { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			id = 1;
			if (id == null) { return NotFound(); }

			Customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Customer, id);

			if (Customer == null) return NotFound();
			return Page();
		}
	}
}
