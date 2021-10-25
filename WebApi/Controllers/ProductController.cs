using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	/// <summary>
	/// API for Products (All, Single, CRUD Functionality)
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class ProductsController : ControllerBase
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly Service.ISiteFunctions _siteFunctions;
		private readonly Service.Querys.IFilterService _filterService;
		#endregion

		public ProductsController(Service.ISiteFunctions siteFunction) { _siteFunctions = siteFunction; _filterService = new Service.Querys.FilterService(_siteFunctions); }

		/// <summary>
		/// Gets all Products with supplied Filter/Order parameters.
		/// </summary>
		/// <param name="_searchString">The SearchString</param>
		/// <param name="_curPage">The page we're curently on</param>
		/// <param name="_pageSize">Number of Products to display</param>
		/// <returns>List of Products</returns>
		[HttpGet]
		[Route("All")]
		public async Task<IEnumerable<Service.DTO.ProductDTO>> GetAll(string _searchString, int _curPage, int _pageSize)
		{
			return await _filterService.FilterProducts(
				_searchString,
				_curPage,
				_pageSize,
				Service.Querys.OrderBy.Descending,
				Service.Querys.ProductFilterBy.Price);
		}

		/// <summary>
		/// Gets a single Product by it's PRODUCT ID
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns>Product as DTO Class</returns>
		[HttpGet]
		[Route("{id}")]
		public async Task<Service.DTO.ProductDTO> GetProduct(int? id)
		{
			// ID MUST BE SUPPLIED
			if (id == null) return null;

			// RETRIEVE THE PRODUCT
			var product = (Service.DTO.ProductDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Product, id);

			// NO PRODUCT FOUND -> RETURN NULL
			if (product == null) return null;

			return product; // TODO: TASK AWAIT IMPLEMENTATION
		}

		/// <summary>
		/// Creates a new Product in the system.
		/// </summary>
		/// <param name="product">The product Data</param>
		/// <returns>Status Result</returns>
		[HttpPost]
		[Route("Create")]
		public async Task<IActionResult> CreateProduct(Service.DTO.ProductDTO product)
		{
			// ADD PRODUCT
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Create, Service.FunctionName.Product, product))
				return Ok("Product Created, ID: " + product.ProductID);

			// SOMETHING WENT HORRIBLY WRONG HERE!
			return BadRequest();
		}

		/// <summary>
		/// Edits an existing Product.
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <param name="product">Product Object to be edited</param>
		/// <returns>Status Result</returns>
		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> EditProduct(int id, Service.DTO.ProductDTO product)
		{
			// CHECKS FOR ID MISMATCH/PRODUCT NOT FOUND
			if (id != product.ProductID) return NotFound("ID: " + id + "does not match product with ID: " + product.ProductID);
			if (!ProductExists(product.ProductID)) return NotFound("Product with ID: " + product.ProductID + " was not found!");

			// ELSE UPDATE PRODUCT
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Update, Service.FunctionName.Product, product))
				return Ok(product.ProductID + " Updated");

			// WE SHOULD NOT BE GETTING HERE
			return BadRequest();
		}

		/// <summary>
		/// Deletes a Product from the system.
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns>Status Result</returns>
		[HttpPost]
		[Route("Delete/{id}")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			// FIND PRODUCT PER ID -> IF NONE FOUND RETURN NOT FOUND MESSAGE
			var product = (Service.DTO.ProductDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Product, id);
			if (!ProductExists(product.ProductID)) return NotFound("Product with ID: " + product.ProductID + " was not found!");

			// ELSE WE CONTINUE REMOVING PRODUCT
			if ((bool)_siteFunctions.PerformAction(Service.ActionType.Delete, Service.FunctionName.Product, product))
				return Ok("Product Removed!");

			// WE SHOULD NOT BE GETTING HERE
			return BadRequest();
		}

		// Checker for Product existence.
		private bool ProductExists(int id)
		{
			var product = (Service.DTO.ProductDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Product, id);

			if (product != null) return true;
			return false;
		}
	}
}
