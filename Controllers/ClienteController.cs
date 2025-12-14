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

        // lista clienti
        public async Task<IActionResult> Index()
        {
            var clienti = await _clienteService.GetClientiAsync();
            return View(clienti);
        }

        // metodo Get create form partial ajax
        [HttpGet]
        public IActionResult CreatePartial()
        {
            var cliente = new Cliente();
            return PartialView("_FormCliente", cliente);
        }

        // salva nuovo cliente ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dati non validi" });
            }

            var result = await _clienteService.CreateClienteAsync(cliente);

            if (result)
            {
                return Json(new { success = true, message = "Cliente creato con successo" });
            }

            return Json(new { success = false, message = "Errore durante la creazione" });
        }

        //metodo Get modifica partial form ajax
        [HttpGet]
        public async Task<IActionResult> EditPartial(Guid id)
        {
            var cliente = await _clienteService.GetClienteAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return PartialView("_FormCliente", cliente);
        }

        // modifica cliente ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return Json(new { success = false, message = "ID non corrispondente" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dati non validi" });
            }

            var result = await _clienteService.UpdateClienteAsync(cliente);

            if (result)
            {
                return Json(new { success = true, message = "Cliente aggiornato con successo" });
            }

            return Json(new { success = false, message = "Errore durante l'aggiornamento" });
        }

        // elimina cliente ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _clienteService.DeleteClienteAsync(id);

            if (result)
            {
                return Json(new { success = true, message = "Cliente eliminato con successo" });
            }

            return Json(new { success = false, message = "Errore durante l'eliminazione" });
        }
    }
}
