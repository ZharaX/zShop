using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

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
		#region IDENTITY CLAIMS
		public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
		{
			var identity = currentPrincipal.Identity as ClaimsIdentity;
			if (identity == null)
				return;

			// check for existing claim and remove it
			var existingClaim = identity.FindFirst(key);
			if (existingClaim != null)
				identity.RemoveClaim(existingClaim);

			// add new claim
			identity.AddClaim(new Claim(key, value));
		}

		public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
		{
			var identity = currentPrincipal.Identity as ClaimsIdentity;
			if (identity == null)
				return null;

			var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
			return claim.Value;
		}
		#endregion
	}
}
