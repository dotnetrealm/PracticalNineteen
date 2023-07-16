using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PracticalNineteen.Domain.DTO;
using System.Security.Claims;

namespace PracticalNineteen.Controllers
{
    public class AccountController : Controller
    {
        readonly HttpClient _httpClient;
        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }
        [HttpGet]
        public async Task<IActionResult> RegisterAsync()
        {
            IEnumerable<string> roles = await _httpClient.GetFromJsonAsync<IEnumerable<string>>("identity/roles");
            ViewBag.Roles = roles.Select(x => new SelectListItem() { Text = x, Value = x }).ToList();
            return View(new UserRegistrationModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(UserRegistrationModel user)
        {
            IEnumerable<string> roles = await _httpClient.GetFromJsonAsync<IEnumerable<string>>("identity/roles");
            ViewBag.Roles = roles.Select(x => new SelectListItem() { Text = x, Value = x }).ToList();

            if (!ModelState.IsValid) return View(user);

            var res = await _httpClient.PostAsJsonAsync("identity/register", user);

            if (res.IsSuccessStatusCode)
            {
                ResponseModel data = await res.Content.ReadFromJsonAsync<ResponseModel>();
                HttpContext.Response.Cookies.Append("token", data.Data, new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(7),
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });
                return Redirect("/Student/Index");
            }

            ErrorModel resMessage = await res.Content.ReadFromJsonAsync<ErrorModel>();
            ModelState.AddModelError(string.Empty, resMessage.Error);
            return View(user);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            throw new Exception();
            ViewBag.ReturnUrl = returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Student");
            }
            return View(new CredentialModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(CredentialModel creds, string? returnUrl)
        {

            if (ModelState.IsValid)
            {
                var res = await _httpClient.PostAsJsonAsync<CredentialModel>("identity/login", creds);

                if (!res.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Invalid UserName/Password.");
                    return View(creds);
                }

                ResponseModel data = await res.Content.ReadFromJsonAsync<ResponseModel>();
                HttpContext.Response.Cookies.Append("token", data.Data, new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(7),
                    Secure = true,
                    IsEssential = true,
                    HttpOnly = false,
                    SameSite = SameSiteMode.None
                });

                List<Claim> claims = new List<Claim>() {
                                        new Claim(ClaimTypes.Email, creds.Email),
                                        new Claim(ClaimTypes.Name, data.UserInfo.FirstName + " " + data.UserInfo.LastName),
                                        new Claim(ClaimTypes.Role, data.UserInfo.Role),
                                        new Claim("UserId", data.UserInfo.UserId.ToString()),
                                        new Claim("Token", data.Data)
                                    };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "AccountCookie");
                ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    IsPersistent = creds.RememberMe,
                };
                await HttpContext.SignInAsync("AccountCookie", principal, properties);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

                return RedirectToAction("Index", "Student");
            }
            return View(creds);
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync("AccountCookie");
            return RedirectToAction("Login", "Account");
        }
    }
}
