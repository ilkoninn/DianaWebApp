
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Primitives;
using NuGet.Packaging.Signing;
using System.Runtime.CompilerServices;

namespace DianaWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _http;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext db, LinkGenerator linkGenerator, IHttpContextAccessor http)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
            _linkGenerator = linkGenerator;
            _http = http;
        }

        // <-- Register Section -->
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser appUser = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
            };

            var resultEmail = await _userManager.FindByEmailAsync(registerVM.Email);

            if (resultEmail == null)
            {
                var result = await _userManager.CreateAsync(appUser, registerVM.Password);
                await _userManager.AddToRoleAsync(appUser, "Member");
                
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                string url = _linkGenerator.GetUriByAction(_http.HttpContext, action: "ConfirmEmail", controller: "Account",
                    values: new {token, appUser.Id} );

                SendMailService.SendMessage(toUser: appUser.Email, userName: appUser.Name, url: url);


                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(registerVM);
                }

                return Redirect(url);
            }
            else
            {
                ModelState.AddModelError("Email", "This email used before, please try another email!");
                return View();
            }
        }

        // <-- Login Section -->
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);

                if (user == null)
                {
                    ModelState.AddModelError("", "Username/Email or password is not valid!");
                    return View();
                }

            }

            if (user != null)
            {
                var result = _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, true).Result;

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username/Email or password is not valid!");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Please, try again later!");
                }
                if(!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "You should be confirmed your Email!");
                }

                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _signInManager.SignInAsync(user, loginVM.RememberMe);

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username/Email or password is not valid!");
                return View();
            }
        }

        // <-- Logout Section -->
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        // <-- Create Role Section -->
        public async Task<IActionResult> CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                var roleExist = await _roleManager.RoleExistsAsync(item.ToString());
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = item.ToString()
                    });
                }
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        // <-- Subscription Section -->
        [HttpPost]
        public async Task<IActionResult> Subscription(SubscriptionVM subscriptionVM,string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(returnUrl);
            }
            
            var oldSubscribe = await _db.Subscriptions.FirstOrDefaultAsync(x => x.Email == subscriptionVM.Email);

            if(oldSubscribe == null)
            {
                Subscription newSubscription = new()
                {
                    Email = subscriptionVM.Email,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                await _db.Subscriptions.AddAsync(newSubscription);
                await _db.SaveChangesAsync();
                SendMailService.SendMessage(toUser: subscriptionVM.Email, userName: "Diana Team");
            }


            if (returnUrl != null)
            {
                Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        // <-- Confirm Email Section -->
        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            var user = await _userManager.FindByIdAsync(Id);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                ViewBag.IsSuccess = true;
            }
            else
            {
                ViewBag.IsSuccess = false;
            }

            return View();
        }

    }
}
