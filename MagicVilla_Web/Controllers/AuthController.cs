using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.ApplicationUserDTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginRequestDTO());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            if (!ModelState.IsValid) return View(model);
            var res = await _authService.Login<APIResponse>(model);
            if (res != null && res.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<LoginResponseDTO>(JsonConvert.SerializeObject(res.Result));
                if (data != null && data.User != null && !string.IsNullOrEmpty(data.Token))
                {
                    var user = data.User;
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString(SD.SessionToken, data.Token);
                    return RedirectToAction("Index", "Home");
                }
            }
            if (res != null && res.IsSuccess == false)
            {
                res.ErrorMessages.ForEach(x =>
                {
                    ModelState.AddModelError("Authentiaction", x);
                });
            }
            return View();
        }

        // REGISTER
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterRequestDTO());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                var res = await _authService.Register<APIResponse>(model);
                if (res != null && res.IsSuccess)
                {
                    return RedirectToAction(nameof(Login));
                }
                else if(res != null)
                {
                    res.ErrorMessages.ForEach(err => { ModelState.AddModelError("Register", err); });
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult DeniedAccess()
        {
            return View();
        }
    }
}
