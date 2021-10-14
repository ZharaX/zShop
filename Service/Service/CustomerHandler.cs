using System.Collections.Generic;
using System.Linq;

namespace Service
{
	/// <summary>
	/// Customer Handler Interface implementation
	/// <para>USE THIS TO ACCESS SITE REPOSITORY</para>
	/// </summary>
	public interface ICustomerHandler
	{
		/// <summary>
		/// User Store -> Stores a User for 5 Minutes, this timer restarts with every action taken.
		/// </summary>
		/// <param name="cred">SID[0] | User[1] | Pass[2]</param>
		/// <returns>SESSION ID (GUID)</returns>
		///string AddUserForSession(string[] cred);

		/// <summary>
		/// When user is Authenticated, user will have an SID to validate against.
		/// </summary>
		/// <param name="sID">SESSION ID</param>
		/// <returns>CustomerDTO Object</returns>
		//DTO.CustomerDTO ReturnCustomerDTO(string sID);
	}

	public class CustomerHandler : ICustomerHandler
	{
		// REFERENCE FOR DB MANAGER
		//private readonly Data.IDBHandler _dbContext;
		private List<ActiveUsers.UserData> _users = new List<ActiveUsers.UserData>();

		// ON INSTANTIATION: Starts Timer Event which checks for user activity (Auto-Logout Functionality)
		//public CustomerHandler(Data.IDBHandler dbContext) { _dbContext = dbContext; CheckUserStatus(); }

		#region USER / CUSTOMER AUTHENTICATION
		public string AddUserForSession(string[] cred)
		{
			// CHECK IF SID EXISTS ALREADY
			if (_users.FirstOrDefault(c => c.SID == cred[0]) == null)
			{
				int cID = 0; //LoginCustomer(cred);

				// ADD TO USER STORE IF USER WAS LOGGED IN
				if (cID != -1)
				{
					// USER WAS AUTHENTICATED -> ADD TO STORE
					ActiveUsers.UserData user = new ActiveUsers.UserData(cID);
					_users.Add(user);

					// RETURN THE GENERATED SID USED FOR AUTHENTICATION
					return user.SID;
				}
			}

			// NONE FOUND
			return null;
		}

		/// <summary>
		/// Finds a Customer with SessionID (Authorized)
		/// </summary>
		/// <param name="sID">SessionID</param>
		/// <returns>Data.Models.Customer Object</returns>
		//private int LoginCustomer(string[] cred)
		//{
		//	// GET CUSTOMER OBJECT
		//	//Data.Models.Customer cust = _dbContext.LoginCustomer(cred);

		//	Data.Models.Customer cust = _dbContext.GetCustomer(1);

		//	// RETURN CUSTOMER ID
		//	if (cust != null) return cust.ID;

		//	return -1; // NO CUSTOMER LOGIN
		//}

		/// <summary>
		/// Finds a Customer with SessionID (Authorized)
		/// </summary>
		/// <param name="sID">SessionID</param>
		/// <returns>Data.Models.Customer Object</returns>
		//public DTO.CustomerDTO ReturnCustomerDTO(string sID)
		//{
		//	// GET CUSTOMER OBJECT
		//	//Data.Models.Customer cust = _dbContext.GetCustomer(_users.FirstOrDefault(c => c.SID == sID).CID);

		//	// NEEDED FOR NOW FOR TEST PRESENTATION -> ABOVE IS THE ACTUAL IMPLEMENTATION
		//	Data.Models.Customer cust = _dbContext.GetCustomer(1);

		//	// RETURN CUSTOMER ID
		//	if (cust != null) return new DTO.CustomerDTO
		//	{
		//		FirstName = cust.FirstName,
		//		LastName = cust.LastName,
		//		Address = cust.Address,
		//		City = cust.City,
		//		Postal = cust.Postal,
		//		Country = cust.Country,
		//		Phone = cust.Phone,
		//		Email = cust.Email
		//	};

		//	return null; // NO CUSTOMER DATA TO TRANSFER
		//}
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
