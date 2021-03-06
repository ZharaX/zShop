using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace zShopWeb.Pages.Orders
{
	/// <summary>
	/// Orders 
	/// </summary>
	public class OrdersModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly ISiteFunctions _siteFunctions;
		#endregion

		public List<Service.DTO.OrderDTO> Orders { get; set; }  // LIST OF  ALL ORDERS

		#region NEW ORDER PROPERTIES
		public Service.DTO.CustomerDTO Customer { get; set; }   // CUSTOMER

		[BindProperty]
		public Service.DTO.OrderDTO NewOrder { get; set; }      // THIS NEW ORDER

		[BindProperty]
		public List<Service.DTO.ProductDTO> OrderProducts { get; set; }// THIS NEW ORDERS PRODUCTS
		#endregion

		// DISPLAY BOOLS
		public bool DisplayOrder { get; set; }


		public OrdersModel(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		/// <summary>
		/// OnGet checks if user is logged in, if not he shouldn't even be on this page.
		/// Redirect to Index
		/// </summary>
		/// <param name="sID">SessionID</param>
		public async Task<IActionResult> OnGetAsync(string sID)
		{
			// GET CUSTOMER FROM TEMPDATA
			if (TempData.ContainsKey("Customer"))
				Customer = TempData.Get<Service.DTO.CustomerDTO>("Customer");

			// IF CUSTOMER WAS NOT FOUND -> CONTINUE TO LOGIN
			if(Customer == null)
				Customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Customer, sID);

			// IF CUSTOMER IS STILL NULL, HE CANNOT STAY ON THIS PAGE -> REDIRECT TO /INDEX
			if (Customer == null) await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			// CUSTOMER WAS FOUND SOMEHOW, SAVE HIM
			TempData.Set("Customer", Customer);

			// GET NEW ORDER DETAILS (LIST OF PRODUCT IDs)
			if(TempData.ContainsKey("NewOrder"))
			{
				// SET MODEL CLASS
				Models.NewOrder no = TempData.Get<Models.NewOrder>("NewOrder");

				// INSTANTIATE NEW ORDER DTO CLASS
				NewOrder = new Service.DTO.OrderDTO();

				// RETRIEVE PRODUCTS
				var products = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);

				// PREPARE PRODUCT COLLECTION IN ORDER DTO
				NewOrder.Products = new List<Service.DTO.ProductDTO>();

				// GROUP MULTIPLE ID INTO KEY,VALUE
				var sameID = no.OrderProducts.GroupBy(i => i);

				// LOOP CUSTOMER PRODUCT CHOICES
				foreach (int pID in no.OrderProducts)
				{
					// GET PRODUCT
					var foundProduct = products.Where(p => p.ProductID == pID).Single();

					// DUPLICATES ARE NOT TOLERATED (WE SET AMOUNT IN DTO INSTEAD)
					if (!NewOrder.Products.Contains(foundProduct))
						NewOrder.Products.Add(foundProduct);
				}

				// IDENTICAL PRODUCTS IN ORDER REQUEST ARE SET AS AMOUNT
				foreach (var id in sameID)
				{
					NewOrder.Products.Find(p => p.ProductID == id.Key).Amount = id.Count();
				}

				// THIS IS FOR BINDING PURPOSES
				OrderProducts = NewOrder.Products;
			}

			// DONE RETURN PAGE
			return Page();
		}

		/// <summary>
		/// OnPost user is continuing with Order Creation
		/// </summary>
		public async Task<IActionResult> OnPostAsync()
		{
			// CHECKS VALIDATION
			if (!ModelState.IsValid) return Page();

			/// RETRIEVE CUSTOMER
			Customer = TempData.Get<Service.DTO.CustomerDTO>("Customer");
			TempData.Set("Customer", Customer);

			// ASSOCIATE CUSTOMER TO NEW ORDER
			NewOrder.Customer = Customer;
			NewOrder.Products = new List<Service.DTO.ProductDTO>();

			// GET PRODUCTS AND ASSOCIATE TO NEW ORDER
			for(int i = 0; i < OrderProducts.Count; i++)
			{
				NewOrder.Products.Add(OrderProducts[i]);
			}

			// PERFORM SERVICE CALL FOR CREATION IN DB
			if ((bool)_siteFunctions.PerformAction(ActionType.Create, FunctionName.Order, NewOrder))
			{
				// SUCCESS! REMOVE NEW ORDER AND REDIRECT
				TempData.Remove("NewOrder");
				RedirectToPage("./Details");
			}

			// NO! SOMETHING WENT WRONG!
			return RedirectToPage("/Index");
		}

		public async Task<IActionResult> OnGetOrdersAsync()
		{
			if (TempData.ContainsKey("Customer"))
				Customer = TempData.Get<Service.DTO.CustomerDTO>("Customer");

			if (Customer == null)
				Customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Customer, Customer.SID);
				
			if(Customer != null)
				Orders = (List<Service.DTO.OrderDTO>)_siteFunctions.PerformAction(ActionType.Retrieve, FunctionName.Order, 1);

			if (Customer == null) await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			TempData.Set("Customer", Customer);
			DisplayOrder = true;
			return Page();
		}

		public IActionResult OnGetRemoveFromCart(int id)
		{
			Models.NewOrder newOrder = null;

			if (TempData.ContainsKey("NewOrder"))
				newOrder = TempData.Get<Models.NewOrder>("NewOrder");

			if (id != 0)
			{
				var productList = (List<Service.DTO.ProductDTO>)_siteFunctions.PerformAction<List<Service.DTO.ProductDTO>>(ActionType.Query, FunctionName.Product, null);
				var total = 0.00m;

				newOrder.OrderProducts.RemoveAll(p => p.Equals(id));

				foreach (int idx in newOrder.OrderProducts)
				{
					total += productList.Where(p => p.ProductID == idx).Select(p => p.Price).Single();
				}

				newOrder.OrderTotalPrice = total;
			}

			TempData.Set("NewOrder", newOrder);
			OnGetAsync("");

			OrderProducts.RemoveAll(p => p.ProductID == id);
			DisplayOrder = true;

			if(OrderProducts.Count == 0)
			{
				DisplayOrder = false;
				TempData.Remove("NewOrder");
			}

			return Page();
		}
	}
}
