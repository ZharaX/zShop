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
	public class EditModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public EditModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		[BindProperty]
		public Service.DTO.CustomerDTO Customer { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			id = 1;
			if (id == null)
			{
				return NotFound();
			}

			Customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Customer, 1);

			if (Customer == null)
			{
				return NotFound();
			}
			return Page();
		}

		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page();

			if (!CustomerExists(Customer.SID)) return NotFound();
			else
			{
				if ((bool)_siteFunctions.PerformAction(ActionType.Update, FunctionName.Customer, Customer))
				{
					RedirectToPage("./Details");
				}
			}

			return RedirectToPage("./Index");
		}

		private bool CustomerExists(string sID)
		{
			return ((List<Service.DTO.CustomerDTO>)_siteFunctions.PerformAction<List<Service.DTO.CustomerDTO>>(ActionType.Query, FunctionName.Customer, null)).Any(e => e.SID == sID);
		}
	}
}
