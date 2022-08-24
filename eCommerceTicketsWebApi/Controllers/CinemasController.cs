using eCommerceTicketsWebApi.Data;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemasRepository _repository;

        public CinemasController(ICinemasRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var allCinemas = await _repository.GetAllAsync();
            return View(allCinemas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")]Cinema cinema)
        {
            if (!ModelState.IsValid) return View(cinema);
            await _repository.AddAsync(cinema);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var cinemaDetails = await _repository.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cinemaDetails = await _repository.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid) return View(cinema);
            await _repository.UpdateAsync(id, cinema);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cinemaDetails = await _repository.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinemaDetails = await _repository.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");

            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
