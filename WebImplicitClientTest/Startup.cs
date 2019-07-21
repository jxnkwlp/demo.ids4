using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace WebImplicitClientTest
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

			services.AddAuthentication(options =>
			{
				options.DefaultScheme = "Cookies";
				options.DefaultChallengeScheme = "oidc";
			})
			   .AddCookie("Cookies")
			   .AddOpenIdConnect("oidc", options =>
			   {
				   options.Authority = "http://localhost:51000/";
				   options.RequireHttpsMetadata = false;
				   options.ClientId = "mvc";
				   options.SaveTokens = true;

				   // add custom scope 
				   options.Scope.Add("custom");

				   options.TokenValidationParameters = new TokenValidationParameters()
				   {
					   NameClaimType = "name", // 改变默认的 ClaimsIdentity NameClaimType

				   };
				   options.Events = new OpenIdConnectEvents()
				   {
					   OnRedirectToIdentityProvider = (context) =>
					   {
						   if (context.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
						   {
							   var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/signin-oidc";

							   // 改变 RedirectUri 的host, 可以解决一个站点多个域名的问题
							   context.ProtocolMessage.RedirectUri = url;
						   }

						   return Task.CompletedTask;
					   },
				   };
			   });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();

			app.UseStaticFiles();
			app.UseMvcWithDefaultRoute();
		}
	}
}
