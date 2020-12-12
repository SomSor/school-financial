using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace School.Financial.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private static HttpClient httpClient = new HttpClient();

        public AccountController(WebConfiguration webConfiguration) { }

        [HttpGet("signin-google")]
        public async Task<IActionResult> SigninGoogleCallback(string code)
        {
            var requestContent = new StringContent(
                $"code={code}&client_id={WebConfiguration.GoogleAuthClientId}&" +
                $"client_secret={WebConfiguration.GoogleAuthClientSecret}&" +
                $"redirect_uri={WebConfiguration.GoogleAuthRedirectUrl}&" +
                $"grant_type=authorization_code", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return Ok();
        }


        public class ApplicationUser : IdentityUser
        {
        }
    }
}
