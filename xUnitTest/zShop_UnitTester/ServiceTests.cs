using System;
using Xunit;
using Service;

namespace zShop_UnitTester
{
	public class ServiceTests
	{
		private readonly ISiteFunctions _siteFunctions = new SiteFunctions("");

		// TESTING CREATION OF A NEW CUSTOMER THROUGH SERVICE LAYERS 'SiteFunctions'
		[Fact]
		public void Service_interface_to_database_functionality()
		{
			// ARRANGE
			Data.Models.Customer cust = new Data.Models.Customer
			{
				FirstName = "Jens_test",
				LastName = "Burmeister_test",
				Address = "Skovvej 22_test",
				City = "Sønderborg_test",
				Postal = "6400_t",
				Country = "Danmark_test",
				Phone = "31318859_test",
				Email = "jens114x@elevcampus.dk"
			};

			// ACT
			bool status =
				(bool)_siteFunctions.PerformAction(
					ActionType.Create,
					FunctionName.Customer,
					cust
				);

			// ASSERT
			Assert.True(status);
		}

		// CHECKING RETRIEVAL OF CUSTOMER FROM DATABASE
		[Fact]
		public void Customer_retrieval_from_database_functionality()
		{
			// ARRANGE
			Data.Models.Customer cust = new Data.Models.Customer();

			// ACT
			cust = new Data.ZShopContext().GetCustomer(1);

			// ASSERT
			Assert.Equal("Jens", cust.FirstName);
		}
	}
}
