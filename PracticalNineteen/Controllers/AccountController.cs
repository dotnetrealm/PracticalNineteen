using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;
using PracticalNineteen.Domain.DTO;

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
            ModelState.AddModelError(String.Empty, resMessage.Error);
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            //if (_signInManager.IsSignedIn(User))
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            return View(new CredentialModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(CredentialModel creds)
        {
            if (ModelState.IsValid)
            {
                var res = await _httpClient.PostAsJsonAsync<CredentialModel>("identity/login", creds);
                
                if (res.IsSuccessStatusCode)
                {
                    ResponseModel data = await res.Content.ReadFromJsonAsync<ResponseModel>();
                    HttpContext.Response.Cookies.Append("token", data.Data, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                        Secure = true,
                        IsEssential = true,
                        HttpOnly = true,
                        SameSite = SameSiteMode.None
                    });
                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid UserName/Password.");
                    return View(creds);
                }
            }
            return View(creds);
        }

        //public async Task<IActionResult> LogoutAsync()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Login", "Account");
        //}
    }
}
