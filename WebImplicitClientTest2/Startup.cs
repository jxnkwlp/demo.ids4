using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(WebImplicitClientTest2.Startup))]
namespace WebImplicitClientTest2
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

			app.UseCookieAuthentication(new CookieAuthenticationOptions()
			{
				AuthenticationType = "Cookies",
				//LoginPath = new PathString("/account/login"),
			});

			app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions("Cookies")
			{
				ClientId = "mvc",
				Authority = "http://localhost:51000/",
				RequireHttpsMetadata = false,
				RedirectUri = "http://localhost:32000/signin-oidc",

				ResponseType = "id_token",
				Scope = "openid profile custom",

				TokenValidationParameters = new TokenValidationParameters()
				{
					NameClaimType = "name", // 修改默认的 NameType
				},

				Notifications = new OpenIdConnectAuthenticationNotifications()
				{
					// 在重定向的时候做些处理
					RedirectToIdentityProvider = (n) =>
					{
						if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
						{
							var url = n.Request.Uri.GetComponents(System.UriComponents.SerializationInfoString, System.UriFormat.Unescaped);

							// 改变 RedirectUri 的host, 可以解决一个站点多个域名的问题
							n.ProtocolMessage.RedirectUri = url + "signin-oidc";
						}

						// if signing out, add the id_token_hint
						if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
						{
							var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");
							if (idTokenHint != null)
							{
								n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
							}
						}

						return Task.FromResult(0);
					},
				},

				// 放到最后
				SignInAsAuthenticationType = "Cookies",
			});
		}
	}
}
