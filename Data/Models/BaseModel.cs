using System;

namespace Data.Models
{
	/// <summary>
	/// Services as our Base Model Class <para>This abstract class holds our common property ID</para>
	/// </summary>
	public abstract class BaseModel
	{
		public int ID { get; set; }
	}
}
