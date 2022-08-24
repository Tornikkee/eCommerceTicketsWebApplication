using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class DirectorsController : Controller
    {
        IDirectorsRepository _repository;

        public DirectorsController(IDirectorsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _repository.GetAllAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Director director)
        {
            if (!ModelState.IsValid)
            {
                return View(director);
            }
            await _repository.AddAsync(director);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var directorDetails = await _repository.GetByIdAsync(id);

            if (directorDetails == null) return View("NotFound");
            return View(directorDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var directorDetails = await _repository.GetByIdAsync(id);

            if (directorDetails == null) return View("NotFound");
            return View(directorDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Director director)
        {
            if (!ModelState.IsValid)
            {
                return View(director);
            }
            await _repository.UpdateAsync(id, director);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var directorDetails = await _repository.GetByIdAsync(id);
            if (directorDetails == null) return View("NotFound");
            return View(directorDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var directorDetails = await _repository.GetByIdAsync(id);
            if (directorDetails == null) return View("NotFound");

            await _repository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
