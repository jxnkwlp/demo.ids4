using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ids4.server
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

			services.AddIdentityServer()
				// .AddRedirectUriValidator<>()  // RedirectUri 自定义验证
				// .AddProfileService<>() // user信息获取自定义 
				.AddDeveloperSigningCredential()
				.AddInMemoryIdentityResources(Config.GetIdentityResources())
				.AddInMemoryApiResources(Config.GetApis())
				.AddInMemoryClients(Config.GetClients())
				.AddTestUsers(Config.GetUsers());

			services.AddAuthentication("Cookies")
				.AddCookie("Cookies");
			//.AddGoogle("Google", options =>
			//{
			//	options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

			//	options.ClientId = "<insert here>";
			//	options.ClientSecret = "<inser here>";
			//}).AddOpenIdConnect("demoidsrv", "IdentityServer", options =>
			//{
			//	options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
			//	options.SignOutScheme = IdentityServerConstants.SignoutScheme;

			//	options.Authority = "https://demo.identityserver.io/";
			//	options.ClientId = "implicit";
			//	options.ResponseType = "id_token";
			//	options.SaveTokens = true;
			//	options.CallbackPath = new PathString("/signin-idsrv");
			//	options.SignedOutCallbackPath = new PathString("/signout-callback-idsrv");
			//	options.RemoteSignOutPath = new PathString("/signout-idsrv");

			//	options.TokenValidationParameters = new TokenValidationParameters
			//	{
			//		NameClaimType = "name",
			//		RoleClaimType = "role"
			//	};
			//});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();

			app.UseIdentityServer();

			app.UseMvcWithDefaultRoute();
		}
	}
}
