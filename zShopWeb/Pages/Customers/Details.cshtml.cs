using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace zShopWeb.Pages.Customer
{
	public class DetailsModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public DetailsModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }


		[BindProperty]
		public Service.DTO.CustomerDTO Customer { get; set; }
		public List<Service.DTO.OrderDTO> Orders { get; set; }

		// DISPLAY BOOLS
		public bool DisplayDetails { get; set; }
		public bool DisplayOrders { get; set; }


		/// <summary>
		/// OnGet checks if user is logged in, if not he shouldn't even be on this page.
		/// Redirect to Index
		/// </summary>
		/// <param name="sID">SessionID</param>
		public async Task<IActionResult> OnGetAsync(string sID)
		{
			if (TempData.ContainsKey("Customer"))
				Customer = TempData.Get<Service.DTO.CustomerDTO>("Customer");

			if(Customer == null)
				Customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Customer, sID);

			if (Customer == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return RedirectToPage("/Index", "Logout");
			}

			TempData.Set("Customer", Customer);
			return Page();
		}

		/// <summary>
		/// OnPost user is changing Details, update Claims for Name and RedirectToPage
		/// Redirect to Index
		/// </summary>
		/// <param name="sID">SessionID</param>
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page();

			Customer.SID = TempData.Get<Service.DTO.CustomerDTO>("Customer").SID;
			TempData.Set("Customer", Customer);

			if ((bool)_siteFunctions.PerformAction(ActionType.Update, FunctionName.Customer, Customer))
			{
				TempData.Set("Message", "Details Updated");
				User.AddUpdateClaim("name", Customer.FirstName);
				DisplayDetails = true;
				return Page();
			}

			return RedirectToPage("/Index");
		}

		/// <summary>
		/// OnGetOrders user wants to display Orders associated
		/// Redirect to Index
		/// </summary>
		/// <param name="sID">SessionID</param>
		public async Task<IActionResult> OnGetOrdersAsync()
		{
			if (TempData.ContainsKey("Customer"))
				Customer = TempData.Get<Service.DTO.CustomerDTO>("Customer");

			if (Customer == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return RedirectToPage("/Index", "Logout");
			}
				
			if(Customer != null)
				Orders = (List<Service.DTO.OrderDTO>)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Order, 1);

			TempData.Set("Customer", Customer);
			DisplayDetails = true;
			DisplayOrders = true;
			return Page();
		}


		public async Task<IActionResult> OnGetDisplayOrder(int id)
		{
			TempData.Set("Order#", id.ToString());
			return await OnGetOrdersAsync();
		}
	}
}
