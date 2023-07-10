using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;

namespace PracticalNineteen.Controllers
{
    public class AccountController : Controller
    {
        readonly IAccountRepository _accountRepository;
        readonly SignInManager<UserIdentityModel> _signInManager;
        readonly IMapper _mapper;

        public AccountController(IAccountRepository accountRepository, SignInManager<UserIdentityModel> signInManager, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isSucceeded = await _accountRepository.RegisterUserAsync(user);
                    if (isSucceeded)
                    {
                        await _signInManager.SignInAsync(_mapper.Map<UserIdentityModel>(user), false);
                        TempData["UserName"] = user.UserName;
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View(user);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new UserCredential());
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserCredential creds)
        {
            if (ModelState.IsValid)
            {
                var res = await _signInManager.PasswordSignInAsync(creds.UserName, creds.Password, creds.RememberMe, false);

                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid UserName/Password.");
                    return View(creds);
                }
            }
            return View(creds);
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
