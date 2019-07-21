using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebHybridClientTest.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> CallApi()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");

			var client = new HttpClient(); 
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			var content = await client.GetStringAsync("http://localhost:51000/api/values");

			return Ok(content);
		}

		public IActionResult Logout()
		{
			// 双双退出
			return SignOut("Cookies", "oidc");
		}
	}
}