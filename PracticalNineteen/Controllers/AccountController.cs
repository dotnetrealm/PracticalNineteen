using Microsoft.AspNetCore.Mvc;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Entities;

namespace PracticalNineteen.Controllers
{
    public class AccountController : Controller
    {
        readonly HttpClient _httpClient;
        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("apiController");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserRegistrationModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(UserIdentity user)
        {
            var token =await _httpClient.PostAsJsonAsync("Identity/Register", user);
            if (token is not null)
                return Redirect("/Student/Index");
            ModelState.AddModelError(String.Empty, "Invalid Credentials");
            return View(user);
        }

        //[HttpGet]
        //public IActionResult Login()
        //{
        //    if (_signInManager.IsSignedIn(User))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(new CredentialModel());
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(CredentialModel creds)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var res = await _signInManager.PasswordSignInAsync(creds.Email, creds.Password, creds.RememberMe, false);

        //        if (res.Succeeded)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Invalid UserName/Password.");
        //            return View(creds);
        //        }
        //    }
        //    return View(creds);
        //}

        //public async Task<IActionResult> LogoutAsync()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Login", "Account");
        //}
    }
}
