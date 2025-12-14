using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                return View(request);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                request.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim("Nome", user.Nome ?? ""),
                    new System.Security.Claims.Claim("Cognome", user.Cognome ?? "")
                };

                await _signInManager.SignOutAsync();
                await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

                return RedirectToAction("Index", "Home");
            }

            return View(request);
        }

        // logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // access denied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // lista dipendenti
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();

            // aguingere ruolo al dipendente
            var usersWithRoles = new List<DipendenteLista>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                usersWithRoles.Add(new DipendenteLista
                {
                    IdDipendente = user.Id,
                    Nome = user.Nome,
                    Cognome = user.Cognome,
                    Email = user.Email,
                    Ruolo = roles.FirstOrDefault() ?? "Nessun ruolo"
                });
            }

            return View(usersWithRoles);
        }

        // metodo Get per il ruolo
        private async Task LoadRuoliAsync(string selected = null)
        {
            var ruoli = new List<string> { "Admin", "Dipendente", "LetturaPrenotazioni" };

            foreach (var r in ruoli)
            {
                if (!await _roleManager.RoleExistsAsync(r))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = r });
                }
            }

            ViewBag.Ruoli = new SelectList(ruoli, selected);
        }

        // metodo Get create form partial ajax
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDipendentePartial()
        {
            await LoadRuoliAsync();
            return PartialView("_FormDipendenteCreate", new CreateDipendenteRequest());
        }

        // salva nuovo dipendente ajax
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDipendente(CreateDipendenteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dati non validi" });
            }

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

            if (!result.Succeeded)
            {
                return Json(new { success = false, message = "Errore durante la creazione del dipendente" });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, request.Ruolo);

            if (!roleResult.Succeeded)
            {
                return Json(new { success = false, message = "Errore durante l'assegnazione del ruolo" });
            }

            return Json(new { success = true, message = "Dipendente creato con successo" });
        }

        //metodo Get modifica partial form ajax
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDipendentePartial(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault() ?? "Dipendente";

            await LoadRuoliAsync(currentRole);

            var model = new EditDipendenteRequest
            {
                IdDipendente = user.Id,
                Nome = user.Nome,
                Cognome = user.Cognome,
                Email = user.Email,
                Ruolo = currentRole
            };

            return PartialView("_FormDipendenteEdit", model);
        }

        // modifica dipendente ajax
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDipendente(string id, EditDipendenteRequest request)
        {
            if (id != request.IdDipendente)
            {
                return Json(new { success = false, message = "ID non corrispondente" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dati non validi" });
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null || user.IsDeleted)
            {
                return Json(new { success = false, message = "Utente non trovato" });
            }

            user.Nome = request.Nome;
            user.Cognome = request.Cognome;
            user.Email = request.Email;
            user.UserName = request.Email;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return Json(new { success = false, message = "Errore durante l'aggiornamento del dipendente" });
            }

            // rimuovi ruoli esistenti
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            // aggiungi nuovo ruolo
            var addRoleResult = await _userManager.AddToRoleAsync(user, request.Ruolo);

            if (!addRoleResult.Succeeded)
            {
                return Json(new { success = false, message = "Errore durante l'aggiornamento del ruolo" });
            }

            return Json(new { success = true, message = "Dipendente aggiornato con successo" });
        }

        // delete dipendetne
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDipendente(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return Json(new { success = false, message = "Utente non trovato" });

            // soft delete
            user.IsDeleted = true;

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;

            await _userManager.UpdateAsync(user);

            return Json(new { success = true, message = "Dipendente disattivato" });
        }
    }
}
