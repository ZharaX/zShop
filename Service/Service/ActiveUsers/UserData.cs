using System;

namespace Service.ActiveUsers
{
	public class UserData
	{
		public int CID { get; private set; }    // SESSION ID
		public string SID { get; private set; } // CUSTOMER ID
		public DateTime Expires { get; set; }   // LAST ACTIVE TIME (USED FOR LOGGING OUT AFTER X-MINUTES)


		public UserData(int cID)
		{
			SID = Guid.NewGuid().ToString();
			CID = cID;
			Expires = DateTime.Now.AddMinutes(5);
		}
	}
}
