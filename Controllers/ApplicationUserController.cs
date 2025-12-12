using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RogHotel.Models.Dto;
using RogHotel.Models.Entity;

namespace RogHotel.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [AllowAnonymous]
        public async Task<IActionResult> InitializeRoles()
        {
            List<string> roles = new List<string> { "Admin", "Dipendente", "LetturaPrenotazioni" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }

            return Content("Ruoli creati con successo!");
        }

        // register metodo get
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // register 
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Nome = request.Nome,
                Cognome = request.Cognome,
                EmailConfirmed = true,
                DataCreazione = DateTime.Now,
                IsDeleted = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, "Dipendente");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(request);
        }

        // login metodo get
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        //login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email o password non validi");
                return View(request);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                request.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Email o password non validi");
            return View(request);
        }

        // logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}