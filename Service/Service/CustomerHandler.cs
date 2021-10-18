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
		DTO.CustomerDTO AddUserForSession(string[] cred);

		/// <summary>
		/// When user is Authenticated, user will have an SID to validate against.
		/// </summary>
		/// <param name="sID">SESSION ID</param>
		/// <returns>CustomerDTO Object</returns>
		DTO.CustomerDTO ReturnCustomerDTO(string sID, Data.Models.Customer cust);

		bool UpdateUserTimer(string sID, bool userAction);
	}

	public class CustomerHandler : ICustomerHandler
	{
		private readonly ISiteFunctions _siteFunctions;

		private List<ActiveUsers.UserData> _users = new List<ActiveUsers.UserData>();

		// ON INSTANTIATION: Starts Timer Event which checks for user activity (Auto-Logout Functionality)
		public CustomerHandler(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; CheckUserStatus(); }


		#region USER / CUSTOMER AUTHENTICATION
		public DTO.CustomerDTO AddUserForSession(string[] cred)
		{
			// CHECK IF SID EXISTS ALREADY
			if (_users.FirstOrDefault(c => c.SID == cred[0]) == null)
			{
				Data.Models.Customer cust = LoginCustomer(cred);

				// ADD TO USER STORE IF USER WAS LOGGED IN
				if (cust != null)
				{
					// USER WAS AUTHENTICATED -> ADD TO STORE
					ActiveUsers.UserData user = new ActiveUsers.UserData(cust.ID);
					_users.Add(user);

					// RETURN THE GENERATED SID USED FOR AUTHENTICATION
					return ReturnCustomerDTO(user.SID, cust);
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
		private Data.Models.Customer LoginCustomer(string[] cred)
		{
			// GET CUSTOMER OBJECT
			//Data.Models.Customer cust = _dbContext.LoginCustomer(cred);

			Data.Models.Customer cust = (Data.Models.Customer)_siteFunctions.PerformAction(ActionType.Login, FunctionName.Customer, cred);

			// RETURN CUSTOMER ID
			if (cust != null) return cust;

			return null; // NO CUSTOMER LOGIN
		}

		/// <summary>
		/// Finds a Customer with SessionID (Authorized)
		/// </summary>
		/// <param name="sID">SessionID</param>
		/// <returns>Data.Models.Customer Object</returns>
		public DTO.CustomerDTO ReturnCustomerDTO(string sID, Data.Models.Customer cust)
		{
			// NEEDED FOR NOW FOR TEST PRESENTATION -> ABOVE IS THE ACTUAL IMPLEMENTATION
			DTO.CustomerDTO custDTO = new DTO.CustomerDTO(sID);

			// RETURN CUSTOMER ID
			if (cust != null) return new DTO.CustomerDTO(sID)
			{
				FirstName = cust.FirstName,
				LastName = cust.LastName,
				Address = cust.Address,
				City = cust.City,
				Postal = cust.Postal,
				Country = cust.Country,
				Phone = cust.Phone,
				Email = cust.Email
			};

			return null; // NO CUSTOMER DATA TO TRANSFER
		}
		#endregion

		/// <summary>
		/// Finds a User with SessionID (Authorized)
		/// </summary>
		/// <param name="sID">SessionID</param>
		/// <returns>Data.Storage.StorageData Object</returns>
		public bool UpdateUserTimer(string sID, bool userAction)
		{
			if (_users.Exists(u => u.SID == sID))
			{
				ActiveUsers.UserData user = _users.Find(u => u.SID == sID);

				if (userAction)
					user.Expires = System.DateTime.Now.AddMinutes(5);

				return true;
			}

			return false;
		}

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
