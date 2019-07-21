using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ids4.server
{
	public static class Config
	{
		public static List<TestUser> GetUsers()
		{
			return new List<TestUser>
			{
				new TestUser
				{
					SubjectId = "1",
					Username = "a",
					Password = "1",

					Claims = new []
					{
						new Claim("name", "Alice"),
						new Claim("website", "https://alice.com"),

						// add custom claim
						new Claim("guid", Guid.NewGuid().ToString()),
						new Claim("testkey", "i am test key"),
					}
				},
				new TestUser
				{
					SubjectId = "2",
					Username = "bob",
					Password = "password",

					Claims = new []
					{
						new Claim("name", "Bob"),
						new Claim("website", "https://bob.com")
					}
				}
			};
		}

		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			var custom = new IdentityResource("custom", "custom", new[] { "guid", "testkey" });
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				custom
			};
		}

		public static IEnumerable<ApiResource> GetApis()
		{
			return new List<ApiResource>
			{
				new ApiResource("api1", "My API")
			};
		}

		public static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{
				//new Client
				//{
				//	ClientId = "client",

    //                // no interactive user, use the clientid/secret for authentication
    //                AllowedGrantTypes = GrantTypes.ClientCredentials,

    //                // secret for authentication
    //                ClientSecrets =
				//	{
				//		new Secret("secret".Sha256())
				//	},

    //                // scopes that client has access to
    //                AllowedScopes = { "api1" }
				//},
    //            // resource owner password grant client
    //            new Client
				//{
				//	ClientId = "ro.client",
				//	AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

				//	ClientSecrets =
				//	{
				//		new Secret("secret".Sha256())
				//	},
				//	AllowedScopes = { "api1" }
				//},

                // OpenID Connect implicit flow client (MVC)
                new Client
				{
					ClientId = "mvc",
					ClientName = "MVC Client",
					AllowedGrantTypes = GrantTypes.Implicit,
					

                    // where to redirect to after login
                    RedirectUris = {
						"http://localhost:31000/signin-oidc",
						"http://localhost:32000/signin-oidc",
						"http://a.localtest.cc/signin-oidc",
						"http://b.localtest.cc/signin-oidc",
						"http://c.localtest.cc/signin-oidc",
					},

                    // where to redirect to after logout
                    PostLogoutRedirectUris = {
						"http://localhost:31000/signout-callback-oidc",
						"http://localhost:32000/signout-callback-oidc",
						"http://a.localtest.cc/signout-callback-oidc",
						"http://b.localtest.cc/signout-callback-oidc",
						"http://c.localtest.cc/signout-callback-oidc",
					},

					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"custom"
					}
				},

				new Client
				{
					Enabled = true,
					ClientId = "mvc.hybrid",
					ClientName = "MVC Hybrid Client",
					AllowedGrantTypes = GrantTypes.Hybrid,

					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},

					AllowOfflineAccess = true,
                
                    // where to redirect to after login
                    RedirectUris = {
						"http://localhost:35000/signin-oidc",
						"http://localhost:36000/signin-oidc",
						"http://a.localtest.cc/signin-oidc",
						"http://b.localtest.cc/signin-oidc",
						"http://c.localtest.cc/signin-oidc",
					},

                    // where to redirect to after logout
                    PostLogoutRedirectUris = {
						"http://localhost:35000/signout-callback-oidc",
						"http://localhost:36000/signout-callback-oidc",
						"http://a.localtest.cc/signout-callback-oidc",
						"http://b.localtest.cc/signout-callback-oidc",
						"http://c.localtest.cc/signout-callback-oidc",
					},

					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"custom"
					}
				},
			};
		}
	}
}