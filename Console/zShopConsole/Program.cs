﻿using Data;
using System;

namespace zShopConsole
{
	class Program
	{
		static IDBHandler db = new ZShopContext();

		static void Main(string[] args)
		{
			InsertCustomer();
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