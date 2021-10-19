using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Service;

namespace S21DMH3B11_zShop.Pages.Customer
{
	[Authorize(Roles = "Admin")]
	public class IndexModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public IndexModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		public IList<Service.DTO.CustomerDTO> Customers { get; set; }

		
		public async Task OnGetAsync()
		{
			Customers = (List<Service.DTO.CustomerDTO>)_siteFunctions.PerformAction<List<Service.DTO.CustomerDTO>>(ActionType.Query, FunctionName.Customer, null);
		}
	}
}
