using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data.DBManager
{
	/// <summary>
	/// CRUD Implementation <para>Generic Operation</para>
	/// </summary>
	/// <typeparam name="T">Generic Model</typeparam>
	public interface IDBManager<T> where T : Models.BaseModel
	{
		bool Create(T model);
		T Retrieve(object id);
		bool Update(T model);
		bool Delete(T model);
		IQueryable<T> Table { get; }
	}

	public class DBManager<T> : IDBManager<T> where T : Models.BaseModel
	{
		private readonly ZShopContext _dbContext;
		private DbSet<T> _models;

		public DBManager(ZShopContext dbcontext) { _dbContext = dbcontext; }

		#region CRUD GENERIC IMPLEMENTATION
		/// <summary>
		/// DBSet Find : T is model to Find
		/// </summary>
		/// <param name="id">Primary ID Key(s)</param>
		/// <returns>Model Object(s)</returns>
		public T Retrieve(object id) { return this.Models.Find(id); }

		/// <summary>
		/// DBSet Add : T is model to Create
		/// </summary>
		/// <param name="model">The Model Class</param>
		/// <returns>Success/Failure Boolean</returns>
		public bool Create(T model)
		{
			try
			{
				if (model == null) { throw new ArgumentNullException("model"); }

				if(model is Data.Models.Customer)
				{
					var c = model as Data.Models.Customer;

					var newCustomer = new Data.Models.Customer
					{
						FirstName = c.FirstName,
						LastName = c.LastName,
						Address = c.Address,
						City = c.City,
						Postal = c.Postal,
						Country = c.Country,
						Phone = c.Phone,
						Email = c.Email
					};

					this._dbContext.Customers.Add(newCustomer);
					return _dbContext.SaveChanges() > 0;
				}

				return false;
			}
			catch (DbUpdateException e)
			{
				var eMsg = string.Empty;

				foreach (var exceptions in e.Entries)
				{
					eMsg += string.Format("", exceptions);
				}

				var status = new Exception(eMsg, e);
				throw status;
			}
		}

		/// <summary>
		/// DBSet Update : T is model to Update
		/// </summary>
		/// <param name="model">The Model Class</param>
		/// <returns>Success/Failure Boolean</returns>
		public bool Update(T model)
		{
			try
			{
				if (model == null) { throw new ArgumentNullException("model"); }

				this.Models.Update(model);
				if (this._dbContext.SaveChanges() == 1)
					return true;

				return false;
			}
			catch (DbUpdateException e)
			{
				var eMsg = string.Empty;

				foreach (var exceptions in e.Entries)
				{
					eMsg += string.Format("", exceptions);
				}

				var status = new Exception(eMsg, e);
				throw status;
			}
		}

		/// <summary>
		/// DBSet Remove : T is model to Remove
		/// </summary>
		/// <param name="model">The Model Class</param>
		/// <returns>Success/Failure Boolean</returns>
		public bool Delete(T model)
		{
			try
			{
				if (model == null) { throw new ArgumentNullException("model"); }

				this.Models.Remove(model);
				if (this._dbContext.SaveChanges() == 1)
					return true;

				return false;
			}
			catch (DbUpdateException e)
			{
				var eMsg = string.Empty;

				foreach (var exceptions in e.Entries)
				{
					eMsg += string.Format("", exceptions);
				}

				var status = new Exception(eMsg, e);
				throw status;
			}
		}

		/// <summary>
		/// IQueryable : T is Model as Query
		/// </summary>
		/// <returns>IQueryable Model</returns>
		public IQueryable<T> Table { get { return this.Models; } }

		/// <summary>
		/// Private Table (DBSet) : T is Model to retrieve
		/// </summary>
		private DbSet<T> Models
		{
			get
			{
				if (_models == null) _models = _dbContext.Set<T>();
				return _models;
			}
		}
		#endregion
	}
}