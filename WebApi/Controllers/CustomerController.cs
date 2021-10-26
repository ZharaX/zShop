using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	/// <summary>
	/// API for Customers (All, Single, CRUD Functionality)
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class CustomerController : ControllerBase
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly Service.ISiteFunctions _siteFunctions;
		private readonly Service.Querys.IFilterService _filterService;
		#endregion

		public CustomerController(Service.ISiteFunctions siteFunction) { _siteFunctions = siteFunction; _filterService = new Service.Querys.FilterService(_siteFunctions); }

		/// <summary>
		/// Gets all Customers with supplied Filter/Order parameters.
		/// </summary>
		/// <param name="_searchString">The SearchString</param>
		/// <param name="isCompleted">Filter on Incomplete Orders</param>
		/// <param name="_curPage">The page we're curently on</param>
		/// <param name="_pageSize">Number of Customers to display</param>
		/// <returns>List of Customers</returns>
		[HttpGet]
		[Route("All")]
		public async Task<IEnumerable<Service.DTO.CustomerDTO>> GetAll(string _searchString, int _curPage, int _pageSize)
		{
			return await _filterService.FilterCustomers(
				_searchString,
				_curPage,
				_pageSize,
				Service.Querys.OrderBy.Descending,
				Service.Querys.CustomerFilterBy.Orders);
		}

		/// <summary>
		/// Gets a single Customer by it's Customer SID
		/// </summary>
		/// <param name="sID">SESSION ID</param>
		/// <returns>Customer as DTO Class</returns>
		[HttpGet]
		[Route("{id}")]
		public async Task<Service.DTO.CustomerDTO> GetCustomer(string sID)
		{
			// ID MUST BE SUPPLIED
			if (sID == null) return null;

			// RETRIEVE THE PRODUCT
			var customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Customer, new string[] { sID });

			// NO CUSTOMER FOUND -> RETURN NULL
			if (customer == null) return null;

			return customer; // TODO: TASK AWAIT IMPLEMENTATION
		}

		/// <summary>
		/// Creates a new Customer in the system.
		/// </summary>
		/// <param name="customer">The Customer Data</param>
		/// <returns>Status Result</returns>
		[HttpPost]
		[Route("Create")]
		public async Task<IActionResult> CreateProduct(Service.DTO.CustomerDTO customer)
		{
			// ADD PRODUCT
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Create, Service.FunctionName.Customer, customer))
				return Ok("Product Created, ID: " + customer.SID); // TODO: ID/SID

			// SOMETHING WENT HORRIBLY WRONG HERE!
			return BadRequest();
		}

		/// <summary>
		/// Edits an existing Customer.
		/// </summary>
		/// <param name="sID">SESSION ID</param>
		/// <param name="customer">Customer Object to be edited</param>
		/// <returns>Status Result</returns>
		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> EditProduct(string sID, Service.DTO.CustomerDTO customer)
		{
			// CHECKS FOR ID MISMATCH/PRODUCT NOT FOUND
			if (sID != customer.SID) return NotFound("SID: " + sID + "does not match Customer with SID: " + customer.SID);
			if (!ProductExists(customer.SID)) return NotFound("Customer with SID: " + customer.SID + " was not found!");

			// ELSE UPDATE PRODUCT
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Update, Service.FunctionName.Customer, customer))
				return Ok(customer.SID + " Updated");

			// WE SHOULD NOT BE GETTING HERE
			return BadRequest();
		}

		/// <summary>
		/// Deletes a Customer from the system.
		/// </summary>
		/// <param name="sID">SESSION ID</param>
		/// <returns>Status Result</returns>
		[HttpDelete]
		[Route("Delete/{id}")]
		public async Task<IActionResult> DeleteConfirmed(string sID)
		{
			// FIND PRODUCT PER ID -> IF NONE FOUND RETURN NOT FOUND MESSAGE
			var customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Customer, sID);
			if (!ProductExists(customer.SID)) return NotFound("Customer with SID: " + customer.SID + " was not found!");

			// ELSE WE CONTINUE REMOVING PRODUCT
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Delete, Service.FunctionName.Customer, customer))
				return Ok("Customer Removed!");

			// WE SHOULD NOT BE GETTING HERE
			return BadRequest();
		}

		// Checker for Customer existence.
		private bool ProductExists(string sID)
		{
			var customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Customer, sID);

			if (customer != null) return true;
			return false;
		}
	}
}
