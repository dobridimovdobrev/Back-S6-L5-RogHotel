using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RogHotel.Models.Entity;
using RogHotel.Services;

namespace RogHotel.Controllers
{
    [Authorize(Roles = "Admin,Dipendente")]
    public class ClienteController : Controller
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var clienti = await _clienteService.GetClientiAsync();
            return View(clienti);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            var result = await _clienteService.CreateClienteAsync(cliente);

            if (result)
                return RedirectToAction(nameof(Index));

            return View(cliente);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var cliente = await _clienteService.GetClienteAsync(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Cliente cliente)
        {
            if (id != cliente.ClienteId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(cliente);

            var result = await _clienteService.UpdateClienteAsync(cliente);

            if (result)
                return RedirectToAction(nameof(Index));

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _clienteService.DeleteClienteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}