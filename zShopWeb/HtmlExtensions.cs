using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace zShopWeb
{
	public static class HtmlExtensions
	{
		#region TEMPDATA EXTENSIONS
		public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
		{
			tempData[key] = JsonConvert.SerializeObject(value);
		}
		public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
		{
			tempData.TryGetValue(key, out object o);
			return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
		}
		#endregion
		#region SESSION EXTENSIONS
		public static void Set<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}

		public static T Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default : JsonConvert.DeserializeObject<T>(value);
		}
		#endregion
	}
}
