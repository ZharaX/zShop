using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace zShopWeb.Pages
{
	[Authorize(AuthenticationSchemes = "Cookies"), AllowAnonymous]
	public class IndexModel : PageModel
	{
		#region SESSION
		public const string SessionKeyName = "_Name";
		public const string SessionKeyAge = "_Age";
		public const string SessionKeyCurrentTime = "_CurrentTime";
		public const string SessionKeyEndTime = "_EndTime";
		const string SessionKeyTime = "_Time";

		public string SessionInfo_Name { get; private set; }
		public string SessionInfo_Age { get; private set; }
		public string SessionInfo_CurrentTime { get; private set; }
		public string SessionInfo_EndTime { get; private set; }
		public string SessionInfo_MiddlewareValue { get; private set; }
		#endregion

		private readonly Service.ISiteFunctions _siteFunctions;
		private readonly Service.ICustomerHandler _customerHandler;
		private readonly ILogger<IndexModel> _logger;

		public bool DisplayLoginWindow { get; set; }
		[Required] public string UserName { get; set; } // DO NOT BELONG HERE
		[Required] [DataType(DataType.Password)] public string Password { get; set; } // DO NOT BELONG HERE

		public Service.DTO.CustomerDTO Customer { get; set; }

		public string LastTime { get; set; }

		public IndexModel(Service.ISiteFunctions siteFunctions, ILogger<IndexModel> logger)
		{
			_siteFunctions = siteFunctions;
			_customerHandler = new Service.CustomerHandler(siteFunctions);
			_logger = logger;
		}

		#region OnGetAsync() PAGE INITIALIZER : CHECKS CUSTOMER AUTH
		/// <summary>
		/// DUNNO IF THIS BELONGS HERE
		/// Method which loads Page View Data if a User is Authenticated
		/// </summary>
		public async Task<IActionResult> OnGetAsync(string returnUrl = null, bool userAction = true, bool doLogin = false)
		{
			Customer = TempData.Get<Service.DTO.CustomerDTO>("Customer");
			TempData.Set("Customer", Customer);

			//TempData.Remove("NewOrder");

			#region TO BE REMOVED (H3 ASSIGNMENTS)
			TempData.Set("Test", "Hello TempData");

			SessionInfo_Name = HttpContext.Session.GetString(SessionKeyName);
			SessionInfo_Age = HttpContext.Session.GetString(SessionKeyAge);
			SessionInfo_CurrentTime = HttpContext.Session.GetString(SessionKeyCurrentTime);
			SessionInfo_EndTime = HttpContext.Session.GetString(SessionKeyEndTime);
			#endregion

			// CUSTOMER CLICKED LOGIN BUTTON -> DISPLAY PARTIAL
			DisplayLoginWindow = doLogin;

			// RETRIEVE USERS SESSION ID (NAMEIDENTIFIER)
			var sessionUID = HttpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			// CHECKS IF THE USER AUTHENTICATED
			if (_customerHandler.UpdateUserTimer(sessionUID, userAction))
			{
				// IF NO SUCH USER ID WAS FOUND -> SEND BACK TO LOGIN PAGE
				returnUrl ??= Url.Content("~/");
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return LocalRedirect(returnUrl);
			}
			return Page();
		}

		/// <summary>
		/// Performs Logout of Customer
		/// </summary>
		/// <param name="returnUrl">Index</param>
		/// <returns>Redirect to Index</returns>
		public async Task<IActionResult> OnGetLogoutAsync(string returnUrl = null)
		{
			// IF NO SUCH USER ID WAS FOUND -> SEND BACK TO LOGIN PAGE
			returnUrl ??= Url.Content("/");
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			//return LocalRedirect(returnUrl);
			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostLoginUserAsync(string returnUrl = null, string username = null, string userpass = null, string btnCancel = null)
		{
			if (btnCancel == "Cancel") return RedirectToPage();

			returnUrl ??= Url.Content("~/");

			Customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Customer, new string[] { "", username, userpass });
			TempData.Set("Customer", Customer);

			if (Customer == null)
			{
				return Page();
			}
			else
			{
				if (userpass == "12345") // TODO: HARD CODED SO FAR -> NO USER DB ATTACHED
				{
					if (TempData.ContainsKey("returnUrl"))
						returnUrl = TempData.Get<string>("returnUrl");

					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, Customer.SID),
						new Claim(ClaimTypes.Name, Customer.FirstName),
						new Claim(ClaimTypes.Role, "Admin"),
						new Claim("UserDefined", "whatever"),
					};

					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						principal,
						new AuthenticationProperties { IsPersistent = true });
				}
				else
				{
					if (TempData.ContainsKey("returnUrl"))
						returnUrl = TempData.Get<string>("returnUrl");

					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, ""),
						new Claim(ClaimTypes.Name, "User"),
						new Claim(ClaimTypes.Role, "User"),
						new Claim("UserDefined", "whatever"),
					};

					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						principal,
						new AuthenticationProperties { IsPersistent = true });
				}
			}

			return Redirect(returnUrl);
		}
		#endregion
		#region H# ASSIGNMENTS (TO BE REMOVED)
		public IActionResult OnPost(string name, string color)
		{
			if(name != null)
				Response.Cookies.Append("Cookiez", $"Velkommen {name}, {DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")}");

			if(color != null)
				Response.Cookies.Append("Cookiez2", color);

			return RedirectToPage();
		}

		public IActionResult OnPostSessionTest(string name, string age)
		{
			HttpContext.Session.SetString(SessionKeyName, name);
			HttpContext.Session.SetString(SessionKeyAge, age);
			HttpContext.Session.SetString(SessionKeyCurrentTime, DateTime.Now.ToString());
			HttpContext.Session.SetString(SessionKeyEndTime, DateTime.Now.AddMinutes(2).ToString());

			return RedirectToPage();
		}
		#endregion
	}
}
