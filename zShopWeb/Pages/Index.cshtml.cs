using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zShopWeb.Pages
{
	public class IndexModel : PageModel
	{
		private readonly Service.ISiteFunctions _sitefunctions;
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(Service.ISiteFunctions siteFunctions, ILogger<IndexModel> logger)
		{
			_sitefunctions = siteFunctions;
			_logger = logger;
		}

		public void OnGet()
		{
			//var customers = _sitefunctions.PerformAction<IQueryable>(Service.ActionType.Query, Service.FunctionName.Customer, null);
			//Service.DTO.CustomerDTO cust = (Service.DTO.CustomerDTO)_sitefunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Customer, 1);

			//cust.FirstName = "Jens";
			//bool t = (bool)_sitefunctions.PerformAction(Service.ActionType.Update, Service.FunctionName.Customer, cust);
		}
	}
}
