using System;
using Xunit;
using Service;

namespace zShop_UnitTester
{
	public class ServiceTests
	{
		private readonly ISiteFunctions _siteFunctions = new SiteFunctions(""); // EMPTY CONNECTIONSTRING UNTIL RAZOR IS INSTALLED

		// SERVICE -> DATABASE TEST #1
		// TESTING CREATION OF A NEW CUSTOMER THROUGH SERVICE LAYERS 'SiteFunctions'
		[Fact]
		public void Service_interface_to_database_functionality()
		{
			// ARRANGE -> CREATE CUSTOMER OBJECT
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

			// ACT -> THE DB RETURN STATUS RECEIVED ON SUCCESSFULL CREATION
			bool status =
				(bool)_siteFunctions.PerformAction(
					ActionType.Create,
					FunctionName.Customer,
					cust
				);

			// ASSERT -> TEST FOR THIS STATUS
			Assert.True(status);
		}

		// SERVICE -> DATABASE TEST #2
		// CHECKING RETRIEVAL OF CUSTOMER FROM SERVICE -> DATABASE
		[Fact]
		public void Customer_retrieval_from_service_functionality()
		{
			// ARRANGE -> CREATE CUSTOMER HANDLER REFERENCE WITH A DBCONTEXT
			CustomerHandler ch = new CustomerHandler(new Data.ZShopContext());

			// PREPARE CUSTOMER DTO OBJECT
			Service.DTO.CustomerDTO custDTO = new Service.DTO.CustomerDTO();

			// ACT -> RETURN CUSTOMER AS DTO
			custDTO = ch.ReturnCustomerDTO("");

			// ASSERT
			Assert.Equal("Jens", custDTO.FirstName);
		}

		// DIRECT DATABASE TEST
		// CHECKING RETRIEVAL OF CUSTOMER FROM DATABASE DIRECTLY
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
