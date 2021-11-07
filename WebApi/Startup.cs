using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Net.Http.Headers;

namespace WebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<Service.ICustomerHandler, Service.CustomerHandler>();

			string connectionString = Configuration.GetConnectionString("S21DMH3B11_zShopDBContext2");
			services.AddDbContext<Data.IZShopContext, Data.ZShopContext>(
				options => options.UseSqlServer(connectionString)
			);

			services.AddScoped<Service.ISiteFunctions, Service.SiteFunctions>();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie("Cookies", options =>
				{
					options.AccessDeniedPath = "/Error";
					options.Cookie.Name = "AuthCookie";
					options.Cookie.HttpOnly = true;
					options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
					options.LoginPath = "/Users/User";
					options.ReturnUrlParameter = "ReturnUrl";
					options.SlidingExpiration = true;
				});

			//services.AddControllers().AddXmlDataContractSerializerFormatters(); // XML
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
			}

			app.UseCors(policy =>
			policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowAnyOrigin());

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
