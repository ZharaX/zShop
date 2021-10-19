using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace zShopWeb.Pages.Orders
{
	public class OrdersModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public OrdersModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }


		[BindProperty]
		public Service.DTO.CustomerDTO Customer { get; set; }	// CUSTOMER
		public List<Service.DTO.OrderDTO> Orders { get; set; }	// LIST OF ORDERS
		public Service.DTO.OrderDTO NewOrder { get; set; }		// THIS NEW ORDER



		// DISPLAY BOOLS
		public bool DisplayOrder { get; set; }


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

			if (Customer == null) return RedirectToPage("/Index");
			TempData.Set("Customer", Customer);

			if(TempData.ContainsKey("NewOrder"))
			{
				Models.NewOrder no = TempData.Get<Models.NewOrder>("NewOrder");


			}
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
				User.AddUpdateClaim("name", Customer.FirstName);
				RedirectToPage("./Details");
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
				Customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Customer, Customer.SID);
				
			if(Customer != null)
				Orders = (List<Service.DTO.OrderDTO>)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Order, 1);

			if (Customer == null) return RedirectToPage("/Index");

			TempData.Set("Customer", Customer);
			DisplayOrder = true;
			return Page();
		}
	}
}
