using Service.DTO;
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
		public static IQueryable<CustomerDTO> ToCustomerDTO(this IQueryable<Data.Models.Customer> customer)
		{
			return customer
				.Select(c => new CustomerDTO
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
		/// Maps Data Order -> DTO Order
		/// <para>Also attaches ProductDTO(s) to OrderDTO</para>
		/// </summary>
		/// <param name="order">Data Order Model</param>
		/// <param name="customer">Data Customer Model</param>
		/// <returns>OrderDTO</returns>
		public static IQueryable<OrderDTO> ToOrderDTO(this IQueryable<Data.Models.Order> order, Data.Models.Customer customer)
		{
			foreach (Data.Models.Product prod in order.SelectMany(op => op.Products).Select(p => p.Product))
			{
				order.Select(o => new ProductDTO
				{
					ProductID = prod.ID,
					Name = prod.Name,
					Description = prod.Description,
					Price = prod.Price,
					Image = prod.Image,
					Category = prod.Category.Name
				});
			}

			System.Collections.Generic.List<ProductDTO> pr = new System.Collections.Generic.List<ProductDTO>();
			pr = ToProductDTO(order.SelectMany(o => o.Products).AsQueryable().Select(p => p.Product)).ToList();

			return order
				.Where(o => o.Customer == customer)
				.Select(o => new OrderDTO(o.ID, o.Amount, o.Discount, o.Date, o.CompletedOn, o.IsProcessed, pr));
		}

		/// <summary>
		/// Maps Data Product -> DTO Product
		/// </summary>
		/// <param name="product">Data Product Model</param>
		/// <returns>ProductDTO</returns>
		public static IQueryable<ProductDTO> ToProductDTO(this IQueryable<Data.Models.Product> product)
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
				});
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
			//return new Data.Models.Order
			//{
			//	Amount = order.Amount,
			//	Discount = order.Discount,
			//	Date = order.Date,
			//	CompletedOn = order.CompletedOn,
			//	IsProcessed = order.IsProcessed,
			//	Products = FromProductDTO(order.Products)
			//};

			return null;
		}

		/// <summary>
		/// Maps Data Product -> DTO Product
		/// </summary>
		/// <param name="product">Data Product Model</param>
		/// <returns>ProductDTO</returns>
		public static System.Collections.Generic.List<Data.Models.Product> FromProductDTO(System.Collections.Generic.List<ProductDTO> product)
		{
			System.Collections.Generic.List<Data.Models.Product> p = new System.Collections.Generic.List<Data.Models.Product>();

			foreach(ProductDTO pdto in product)
			{
				p.Add(new Data.Models.Product
				{
					ID = pdto.ProductID,
					Name = pdto.Name,
					Description = pdto.Description,
					Price = pdto.Price,
					Stock = pdto.Stock,
					Image = pdto.Image
					//Category = p.Category.Name
				});
			}

			return p;
		}
		#endregion
	}
}
