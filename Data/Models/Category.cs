namespace Data.Models
{
	/// <summary>
	/// Context Class for Category
	/// </summary>
	public class Category
	{
		public int CategoryID { get; set; }

		public Product Product { get; set; }
	}
}
