using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace zShopWeb.Areas
{
	[AllowAnonymous]
	public class UserModel : PageModel
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly Service.ISiteFunctions _siteFunctions;
		#endregion

		public Service.DTO.CustomerDTO user { get; set; }
		[Required] public string User { get; set; } // DO NOT BELONG HERE
		[Required] [DataType(DataType.Password)] public string Password { get; set; } // DO NOT BELONG HERE

		public UserModel(Service.ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }


		public async Task OnGetAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null, string username = null, string userpass = null)
		{
			returnUrl ??= Url.Content("~/");

			user = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Customer, new string[] { username, userpass });

			if (user == null)
			{
				return Page();
			}
			else
			{
				if (userpass == "12345") // TODO: HARD CODED SO FAR -> NO USER DB ATTACHED
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, user.SID),
						new Claim(ClaimTypes.Name, user.FirstName),
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
					returnUrl = "/Index";
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

			return LocalRedirect(returnUrl);
		}
	}
}
