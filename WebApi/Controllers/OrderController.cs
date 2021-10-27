using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	/// <summary>
	/// API for Orders (All, Single, CRUD Functionality)
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly Service.ISiteFunctions _siteFunctions;
		private readonly Service.Querys.IFilterService _filterService;
		#endregion

		public OrderController(Service.ISiteFunctions siteFunction) { _siteFunctions = siteFunction; _filterService = new Service.Querys.FilterService(_siteFunctions); }

		/// <summary>
		/// Gets all Orders with supplied Filter/Order parameters.
		/// </summary>
		/// <param name="_searchString">The SearchString</param>
		/// <param name="_curPage">The page we're curently on</param>
		/// <param name="_pageSize">Number of Orders to display</param>
		/// <returns>List of Orders</returns>
		[HttpGet]
		[Route("Orders/All")]
		public async Task<IEnumerable<Service.DTO.OrderDTO>> GetAll(string _searchString, int _curPage, int _pageSize)
		{
			return await _filterService.FilterOrders(
				_searchString,
				_curPage,
				_pageSize,
				Service.Querys.OrderBy.Descending,
				Service.Querys.OrderFilterBy.Date); // TODO: ENUM CORRECT VALUES
		}

		/// <summary>
		/// Gets a single Orders by it's Orders ID
		/// </summary>
		/// <param name="id">Orders ID</param>
		/// <returns>Order as DTO Class</returns>
		[HttpGet]
		[Route("Order/{id}")]
		public async Task<Service.DTO.OrderDTO> GetOrder(string id)
		{
			// ID MUST BE SUPPLIED
			if (id == null) return null;

			// RETRIEVE THE ORDER
			var order = (Service.DTO.OrderDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Order, id);

			// NO ORDER FOUND -> RETURN NULL
			if (order == null) return null;

			return order; // TODO: TASK AWAIT IMPLEMENTATION
		}

		/// <summary>
		/// Creates a new Order in the system.
		/// </summary>
		/// <param name="order">The Order Data</param>
		/// <returns>Status Result</returns>
		[HttpPost]
		[Route("Orders/Create")]
		public async Task<IActionResult> CreateOrder(Service.DTO.OrderDTO order)
		{
			// ADD ORDER
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Create, Service.FunctionName.Order, order))
				return Ok("Order Created, ID: " + order.OrderID);

			// SOMETHING WENT HORRIBLY WRONG HERE!
			return BadRequest();
		}

		/// <summary>
		/// Edits an existing Order.
		/// </summary>
		/// <param name="sID">Order ID</param>
		/// <param name="customer">Order Object to be edited</param>
		/// <returns>Status Result</returns>
		[HttpPut]
		[Route("Orders/Update")]
		public async Task<IActionResult> EditOrder(int id, Service.DTO.OrderDTO order)
		{
			// CHECKS FOR ID MISMATCH/ORDER NOT FOUND
			if (id != order.OrderID) return NotFound("ID: " + id + "does not match Order with ID: " + order.OrderID);
			if (!OrderExists(order.OrderID)) return NotFound("Order with ID: " + order.OrderID + " was not found!");

			// ELSE UPDATE ORDER
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Update, Service.FunctionName.Order, order))
				return Ok(order.OrderID + " Updated");

			// WE SHOULD NOT BE GETTING HERE
			return BadRequest();
		}

		/// <summary>
		/// Deletes a Order from the system.
		/// </summary>
		/// <param name="id">Order ID</param>
		/// <returns>Status Result</returns>
		[HttpDelete]
		[Route("Orders/Delete/{id}")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			// FIND ORDER PER ID -> IF NONE FOUND RETURN NOT FOUND MESSAGE
			var order = (Service.DTO.OrderDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Order, id);
			if (!OrderExists(order.OrderID)) return NotFound("Order with ID: " + order.OrderID + " was not found!");

			// ELSE WE CONTINUE REMOVING ORDER
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Delete, Service.FunctionName.Order, order))
				return Ok("Order Removed!");

			// WE SHOULD NOT BE GETTING HERE
			return BadRequest();
		}

		// Checker for Order existence.
		private bool OrderExists(int id)
		{
			var order = (Service.DTO.OrderDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Order, id);

			if (order != null) return true;
			return false;
		}
	}
}
