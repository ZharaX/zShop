using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace WebApi.Controllers
{
	public class AuthController : ControllerBase
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly Service.ISiteFunctions _siteFunctions;
		#endregion

		public AuthController(Service.ISiteFunctions siteFunction) { _siteFunctions = siteFunction; }

		[HttpPost]
		[Route("/")]
		public async Task<IActionResult> LoginUser(string returnUrl = null, string username = null, string userpass = null, string btnCancel = null)
		{
			if (btnCancel == "Cancel") return null;

			returnUrl ??= "~/";

			Service.DTO.CustomerDTO Customer = (Service.DTO.CustomerDTO)_siteFunctions.PerformAction(Service.ActionType.Retrieve, Service.FunctionName.Customer, new string[] { "", username, userpass });
			
			if (Customer == null) return Redirect(returnUrl);
			else
			{
				if (userpass == "12345") // TODO: HARD CODED SO FAR -> NO USER DB ATTACHED
				{
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

			return Ok(Customer.SID);
		}

		[HttpGet]
		[Route("/")]
		public async Task<IActionResult> Logout(string returnUrl = null)
		{
			// IF NO SUCH USER ID WAS FOUND -> SEND BACK TO LOGIN PAGE
			returnUrl ??= Url.Content("/");
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			//return LocalRedirect(returnUrl);
			return Ok("Logged Out");
		}
	}
}
