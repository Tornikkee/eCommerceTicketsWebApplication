using eCommerceTicketsWebApi.Data;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Data.Services;
using eCommerceTicketsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class ActorsController : Controller
    {
        IActorsRepository _repository;

        public ActorsController(IActorsRepository repository)
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
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _repository.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await _repository.GetByIdAsync(id);

            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actorDetails = await _repository.GetByIdAsync(id);

            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("FullName,ProfilePictureURL,Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _repository.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await _repository.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _repository.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");

            await _repository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
