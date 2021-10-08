using Data;
using Service;
using System;
using System.Collections.Generic;

namespace zShopConsole
{
	class Program
	{
		static IDBHandler db = new ZShopContext();
		private static readonly ISiteFunctions _siteFunctions = new SiteFunctions("");

		static void Main(string[] args)
		{
			//InsertCustomer();

			Data.Models.Customer cust = db.GetCustomer(1);

			List<Data.Models.Category> cat = db.GetAllCategorys();

			//bool status = (bool)_siteFunctions.PerformAction(
			//	ActionType.Create,
			//	FunctionName.Customer,
			//	new Data.Models.Customer
			//	{
			//		FirstName = "Jens_test",
			//		LastName = "Burmeister_test",
			//		Address = "Skovvej 22_test",
			//		City = "Sønderborg_test",
			//		Postal = "6400_t",
			//		Country = "Danmark_test",
			//		Phone = "31318859_test",
			//		Email = "jens114x@elevcampus.dk"
			//	});
		}

		static void InsertCustomer()
		{
			Data.Models.Customer cust = new Data.Models.Customer();

			Console.WriteLine(" Opret Ny Kunde: ");
			Console.WriteLine("");
			Console.WriteLine("");

			Console.Write(" - Fornavn: ");
			cust.FirstName = Console.ReadLine();

			Console.Write(" - Efternavn: ");
			cust.LastName = Console.ReadLine();

			Console.Write(" - Adresse: ");
			cust.Address = Console.ReadLine();

			Console.Write(" - City: ");
			cust.City = Console.ReadLine();

			Console.Write(" - Postnummer: ");
			cust.Postal = Console.ReadLine();

			Console.Write(" - Land: ");
			cust.Country = Console.ReadLine();

			Console.Write(" - Telefon: ");
			cust.Phone = Console.ReadLine();

			Console.Write(" - Email: ");
			cust.Email = Console.ReadLine();

			Console.WriteLine(db.CreateCustomer(cust));

			Console.Read();
		}
	}
}
