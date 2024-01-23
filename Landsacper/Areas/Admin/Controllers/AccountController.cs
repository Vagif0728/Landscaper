using Landsacper.Areas.Admin.ViewModels;
using Landsacper.Models;
using Landsacper.Utilities.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Landsacper.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
          _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                UserName = registerVM.Username,
                Surname= registerVM.Surname
            };


            IdentityResult result = await _userManager.CreateAsync(user,registerVM.Password);

            if (!result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty,error.Description);
                }
                return View();


            }
            
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index","Service",new { Area =" " });

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
           
            if(user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if(user == null)
                {
                    ModelState.AddModelError(String.Empty, "Username or Password false");
                    return View();
                }

            }

            var results = await _signInManager.PasswordSignInAsync(user, loginVM.Password,loginVM.IsRememberedMe,true);
            if(results.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "You account blocked");
                return View();
            }
            if(!results.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Usernmae or Email,password false");
                return View();
            }

            return RedirectToAction("Index","Home",new {Area =" "});
            
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home",new {Area =" "});
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach(UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if(!(await _roleManager.RoleExistsAsync(role.ToString())))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name= role.ToString() });
                }
            }
            return RedirectToAction("Index", "Home", new { Area = " " });
        }
    }
}
