using System.Collections.Generic;
using System.Linq;

namespace Service
{
	/// <summary>
	/// Site Repository Interface implementation
	/// <para>USE THIS TO ACCESS SITE REPOSITORY</para>
	/// </summary>
	public interface ISiteRepository
	{
		//string AddUserForSession(string[] cred);

		/// <summary>
		/// Validating/Retreiving Customer
		/// </summary>
		/// <returns>Customer Object</returns>
		//Data.Models.Customer GetCustomer(string uid, bool userAction);

		/// <summary>
		///Retreiving all Products
		/// </summary>
		/// <returns>List of Product Objects</returns>
		List<Data.Models.Product> GetAllProducts();

		/// <summary>
		/// Method returning Categorys
		/// </summary>
		/// <returns>List of Categorys</returns>
		List<string> GetAllCategorys();

		//List<Data.Log.LogData> GetAllLogs(int ownerID);
	}

	public class SiteRepository : ISiteRepository
	{
		// REFERENCE FOR DB MANAGER
		private Data.ZShopContext _dbContext;
		private List<ActiveUsers.UserData> _users = new List<ActiveUsers.UserData>();

		// ON CLASS INSTANTIATE SET DB MANAGER
		public SiteRepository(Data.ZShopContext dbContext) { _dbContext = dbContext; CheckUserStatus(); }

        public List<Data.Models.Product> GetAllProducts() { return _dbContext.Products.ToList(); }

		public List<string> GetAllCategorys() { return _dbContext.GetCategorys(); }

		public List<Data.Models.Logs> GetUserLogs(int cID) { return _dbContext.GetAllLogs(cID); }


        #region USER / CUSTOMER AUTHENTICATION
        /// <summary>
        /// User Store -> Stores a User for 5 Minutes, this timer restarts with every action taken.
        /// </summary>
        /// <param name="cred">User[0] | Pass[1]</param>
        /// <returns>SESSION ID (GUID)</returns>
        public string AddUserForSession(string[] cred)
		{
			// RETRIEVE CUSTOMER OBJECT ON SUCCESSFULL LOGIN
			Data.Models.Customer cust = _dbContext.GetCustomer(/*cred[0]*/0);

			// IF WE GOT AN OWNER
			if (cust != null)
			{
				// ADD TO USER STORE -> ADD OWNERS CATEGORYS AND COLORS
				_users.Add(new ActiveUsers.UserData(cust.CustomerID));

				// RETURNS A GENERATED SID
				return _users.FirstOrDefault(c => c.CID == cust.CustomerID).SID;
			}

			// NONE FOUND
			return null;
		}

		/// <summary>
		/// Finds a Customer with SessionID (Authorized)
		/// </summary>
		/// <param name="sID">SessionID</param>
		/// <returns>Data.Models.Customer Object</returns>
		public Data.Models.Customer GetCustomer(string sID, bool userAction)
		{
			if (_users.Exists(u => u.SID == sID)) // IF USER EXISTS (ACTIVE)
			{
				// GET CUSTOMER OBJECT
				Data.Models.Customer cust = _dbContext.GetCustomer(_users.FirstOrDefault(u => u.SID == sID).CID);

				// USER DID SOMETHING -> RESTART AUTO LOGOUT TIMER
				if (userAction)
					_users.FirstOrDefault(u => u.SID == sID).Expires = System.DateTime.Now.AddMinutes(5);

				// RETURN CUSTOMER OBJECT
				return cust;
			}

			// RETURN NULL (INACTIVE / LOGGED OUT)
			return null;
		}
        #endregion
        #region TIMER FUNCTION FOR CHECKING EXPIRED USERS
        public void CheckUserStatus()
		{
			System.Timers.Timer timer = new System.Timers.Timer(60000);
			timer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsed);
			timer.AutoReset = true;
			timer.Start();
		}

		private void OnElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			ActiveUsers.UserData u = null;
			foreach (ActiveUsers.UserData user in _users)
			{
				if (System.DateTime.Now >= user.Expires)
				{
					((System.Timers.Timer)sender).Stop();
					u = user;
				}
			}

			_users.Remove(u);
			((System.Timers.Timer)sender).Start();
		}
		#endregion
	}
}
