using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NetMind.Data;
using NetMind.Models;
using System.Security.Cryptography;
using NetMind;

namespace NetMind.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
        private DummyUserCountService _userCount;
        public AccountController(ApplicationContext context, DummyUserCountService userCount)
        {
            db = context;
            _userCount = userCount;
        }
        
        [HttpGet]
        [Route("Login", Name = "Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null && CheckPassword(user, model.Password))
                {
                    await Authenticate(model.Email, model.RememberMe); // аутентификация

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToRoute("Home");
                    }
                }
                ModelState.AddModelError("", "Nepareizs lietotājs vai parole");
            }
            return View(model);
        }
        
        [HttpGet]
        [Route("Register", Name = "Register")]
        public IActionResult Register(DummyUserCountService userCount)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                if (!userCount.HasUser())
                {
                    return Redirect("Home");
                }
                else
                {
                    return View();
                }
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    
                    var rng = RandomNumberGenerator.Create();
                    var saltBytes = new byte[16];
                    rng.GetBytes(saltBytes);
                    var saltText = Convert.ToBase64String(saltBytes);
                    var saltedhashedPassword = SaltAndHashPassword(model.Password, saltText);

                    db.Users.Add(new User { Email = model.Email, PasswordHash = saltedhashedPassword, Salt = saltText, UserName = model.UserName });
                    
                    await db.SaveChangesAsync();

                    _userCount.SetChecked(true);

                    return RedirectToRoute("Login");
                }
                else
                    ModelState.AddModelError("", "Nepareizs lietotājs vai parole");
            }
            return View(model);
        }

        private static string SaltAndHashPassword (string password, string salt )
        {
            var sha = SHA256.Create();
            var saltedPassword = password + salt;
            return Convert.ToBase64String(sha.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword)));
        }

        public static bool CheckPassword(User user, string password)
        {
            // re-generate the salted and hashed password
            var saltedhashedPassword = SaltAndHashPassword(password, user.Salt);
            return (saltedhashedPassword == user.PasswordHash);
        }


        private async Task Authenticate(string userName, bool rememberMe)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), rememberMe ?  
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                    } : null);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToRoute("Login");
        }
    }
}
