namespace Service.DTO
{
	public class CustomerDTO
	{
		public string SID { get; set; } // TODO: FIX A WAY TO PROTECT SID

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Postal { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }

		public System.Collections.Generic.List<OrderDTO> Orders { get; set; }

		public CustomerDTO() { }
		public CustomerDTO(string sID) { SID = sID; }
	}
}
