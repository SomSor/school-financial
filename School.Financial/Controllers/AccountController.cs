using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace School.Financial.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserInfoDac userInfoDac;

        public AccountController(IUserInfoDac userInfoDac)
        {
            this.userInfoDac = userInfoDac;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserInfo model)
        {
            var user = userInfoDac.Get(model.Username);
            if (user != null)
            {
                TempData["errorMessage"] = "ชื่อผู้ใช้นี้มีคนใช้แล้ว";
                return View(model);
            }

            var password = model.Password;
            model.Password = Helpers.HashHelper.GetStringSha256Hash(model.Password);
            userInfoDac.Insert(model);
            return SignIn(new SignInViewModel { Username = model.Username, Password = password });
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel model)
        {
            model.Password = Helpers.HashHelper.GetStringSha256Hash(model.Password);
            var user = userInfoDac.Get(model.Username, model.Password);
            if (user == null)
            {
                TempData["errorMessage"] = "เข้าสู่ระบบผิดพลาด";
                return View(model);
            }

            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("Id", user.Id.ToString(), ClaimValueTypes.Integer32),
                new Claim(ClaimTypes.Email, user.Username),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.User = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)).Wait();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Authorize]
        public ActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}
