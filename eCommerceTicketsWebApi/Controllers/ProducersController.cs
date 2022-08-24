using eCommerceTicketsWebApi.Data;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class ProducersController : Controller
    {
        private readonly IProducersRepository _repository;

        public ProducersController(IProducersRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var allProducers = await _repository.GetAllAsync();
            return View(allProducers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await _repository.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")]Producer producer)
        {
            if (!ModelState.IsValid) return View(producer);

            await _repository.AddAsync(producer);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producerDetails = await _repository.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Producer producer)
        {
            if (!ModelState.IsValid) return View(producer);
            if(id == producer.Id)
            {
                await _repository.UpdateAsync(id, producer);
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var producerDetails = await _repository.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producerDetails = await _repository.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");

            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
