using Service.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Service.Querys
{
	/// <summary>
	/// Extension Class : Extends IQueryable Data Models into Data Transferable Objects (DTO)
	/// </summary>
	public static class QueryManager
	{
		#region TO DTO CONVERSION
		/// <summary>
		/// Maps Data Customer -> DTO Customer
		/// </summary>
		/// <param name="customer">Data Customer Model</param>
		/// <returns>CustomerDTO</returns>
		public static IQueryable<CustomerDTO> ToCustomerDTO(this IQueryable<Data.Models.Customer> customer, string sID)
		{
			return customer
				.Select(c => new CustomerDTO(sID)
				{
					FirstName = c.FirstName,
					LastName = c.LastName,
					Address = c.Address,
					Postal = c.Postal,
					City = c.City,
					Country = c.Country,
					Phone = c.Phone,
					Email = c.Email
				});
		}

		/// <summary>
		/// Builds List of OrderDTO from Data OrderProduct found for specific customer
		/// <para>Also attaches ProductDTO(s)</para>
		/// </summary>
		/// <param name="orderProduct">Query DB OrderProduct</param>
		/// <param name="customer">For Customer?</param>
		/// <returns>List of OrderDTO</returns>
		public static List<OrderDTO> ToOrdersDTO(this IQueryable<Data.Models.OrderProduct> orderProduct, Data.Models.Customer customer)
		{
			// QUERY CUSTOMER ORDERS
			// TODO: IF CUSTOMER IS NULL RETURN COMPLETE LIST OF ORDERS (ADMIN)
			var customerOrders = orderProduct.Where(op => op.Order.Customer == customer).Select(o => o.Order).Distinct().ToList();

			// CUSTOMER ORDERS
			List<OrderDTO> orders = new List<OrderDTO>();

			// LOOP ORDERS -> CREATE NEW ORDER DTO -> ATTACH PRODUCTS AS DTO
			foreach (var o in customerOrders)
			{
				var orderProducts = orderProduct.Where(op => op.OrderID == o.ID).Select(p => p.Product);

				orders.Add(new OrderDTO(o.ID, o.Discount, o.Date, o.CompletedOn, o.IsProcessed, ToProductDTO(orderProducts)));
			}

			// ASSIGN PRODUCT AMOUNT
			foreach (OrderDTO o in orders)
			{
				foreach (ProductDTO p in o.Products)
				{
					p.Amount = orderProduct.Where(op => op.OrderID == o.OrderID && op.ProductID == p.ProductID).Select(p => p.ProductAmount).First();
				}
			}

			// RETURN ORDER(S)
			return orders;
		}

		/// <summary>
		/// Maps Data Order -> DTO Order
		/// </summary>
		/// <param name="order">Data Order Model</param>
		/// <param name="products">DTO Products for Order</param>
		/// <returns>OrderDTO</returns>
		public static OrderDTO ToOrderDTO(this IQueryable<Data.Models.Order> order, List<ProductDTO> products)
		{
			return order
				.Select(o => new OrderDTO(
					o.ID,
					o.Discount,
					o.Date,
					o.CompletedOn,
					o.IsProcessed,
					products
				))
				.Single();
		}

		/// <summary>
		/// Maps Data Product -> DTO Product
		/// </summary>
		/// <param name="product">Data Product Model</param>
		/// <returns>ProductDTO</returns>
		public static List<ProductDTO> ToProductDTO(this IQueryable<Data.Models.Product> product)
		{
			return product
				.Select(p => new ProductDTO
				{
					ProductID = p.ID,
					Name = p.Name,
					Description = p.Description,
					Price = p.Price,
					Stock = p.Stock,
					Image = p.Image,
					Category = p.Category.Name
				})
				.ToList();
		}
		#endregion
		#region FROM DTO CONVERSION
		/// <summary>
		/// Maps DTO Customer -> Data Customer
		/// </summary>
		/// <param name="customer">Data Customer Model</param>
		/// <returns>CustomerDTO</returns>
		public static Data.Models.Customer FromCustomerDTO(CustomerDTO customer)
		{
			return new Data.Models.Customer
			{
				ID = 1, // TODO:
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				Address = customer.Address,
				Postal = customer.Postal,
				City = customer.City,
				Country = customer.Country,
				Phone = customer.Phone,
				Email = customer.Email,
				//Orders = FromOrderDTO(customer.Orders, customer)
			};
		}

		/// <summary>
		/// Maps Data Order -> DTO Order
		/// <para>Also attaches ProductDTO(s) to OrderDTO</para>
		/// </summary>
		/// <param name="order">Data Order Model</param>
		/// <param name="customer">Data Customer Model</param>
		/// <returns>OrderDTO</returns>
		public static Data.Models.Order FromOrderDTO(OrderDTO order, CustomerDTO customer)
		{
			return new Data.Models.Order
			{
				Discount = order.Discount,
				Date = order.Date,
				CompletedOn = order.CompletedOn,
				IsProcessed = order.IsProcessed,
				Products = FromProductDTO(order.Products)
			};
		}

		/// <summary>
		/// Maps Data Product -> DTO Product
		/// </summary>
		/// <param name="product">Data Product Model</param>
		/// <returns>List of OrderProducts</returns>
		public static List<Data.Models.OrderProduct> FromProductDTO(List<ProductDTO> product)
		{
			List<Data.Models.OrderProduct> op = new List<Data.Models.OrderProduct>();

			foreach (ProductDTO pdto in product)
			{
				op.Add(new Data.Models.OrderProduct
				{
					ProductID = pdto.ProductID
				});
			}

			return op;
		}

		public static Data.Models.Product FromProductDTO(ProductDTO product)
		{
			Data.Models.Product p = new Data.Models.Product
			{
				ID = product.ProductID,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Stock = product.Stock,
				Image = product.Image,
				Active = product.Active
			};

			return p;
		}
		#endregion
	}
}
