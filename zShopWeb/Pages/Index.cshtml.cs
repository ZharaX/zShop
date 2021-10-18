using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace zShopWeb.Pages
{
	[Authorize(AuthenticationSchemes = "Cookies")]
	public class IndexModel : PageModel
	{
		private readonly Service.ISiteFunctions _siteFunctions;
		private readonly Service.ICustomerHandler _customerHandler;
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(Service.ISiteFunctions siteFunctions, ILogger<IndexModel> logger)
		{
			_siteFunctions = siteFunctions;
			_customerHandler = new Service.CustomerHandler(siteFunctions);
			_logger = logger;
		}

		#region OnGetAsync() PAGE INITIALIZER : CHECKS USER AUTH & LOADS VIEWMODEL DATA
		/// <summary>
		/// DUNNO IF THIS BELONGS HERE
		/// Method which loads Page View Data if a User is Authenticated
		/// </summary>
		public async Task<IActionResult> OnGetAsync(string returnUrl = null, bool userAction = true)
		{
			// RETRIEVE USERS SESSION ID (NAMEIDENTIFIER)
			var sessionUID = HttpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			// CHECKS IF THE USER AUTHENTICATED
			if (_customerHandler.UpdateUserTimer(sessionUID, userAction))
			{
				// IF NO SUCH USER ID WAS FOUND -> SEND BACK TO LOGIN PAGE
				returnUrl ??= Url.Content("~/");
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return LocalRedirect(returnUrl);
			}
			return Page();
		}
		#endregion

		//public void OnGet()
		//{
		//	//var customers = _sitefunctions.PerformAction<IQueryable>(Service.ActionType.Query, Service.FunctionName.Customer, null);
		//	//Service.DTO.CustomerDTO customer = (Service.DTO.CustomerDTO)_sitefunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Customer, 1);
		//	//List<Service.DTO.ProductDTO> products = (List<Service.DTO.ProductDTO>)_sitefunctions.PerformAction<List<Service.DTO.ProductDTO>>(Service.ActionType.Query, Service.FunctionName.Product, null);

		//	//cust.FirstName = "Jens";
		//	//bool t = (bool)_sitefunctions.PerformAction(Service.ActionType.Update, Service.FunctionName.Customer, cust);

		//	//List<Service.DTO.OrderDTO> orders = (List<Service.DTO.OrderDTO>)_sitefunctions.PerformAction<List<Service.DTO.OrderDTO>>(Service.ActionType.Query, Service.FunctionName.Order, null);

		//	//Service.DTO.OrderDTO order = new Service.DTO.OrderDTO
		//	//{
		//	//	Customer = customer,
		//	//	Products = new List<Service.DTO.ProductDTO>()
		//	//	{
		//	//		products[1],
		//	//		products[2]
		//	//	}
		//	//};

		//	//order.Products[0].Amount = 6;
		//	//order.Products[1].Amount = 666;

		//	//_sitefunctions.PerformAction(Service.ActionType.Create, Service.FunctionName.Order, order);
		//}
	}
}
