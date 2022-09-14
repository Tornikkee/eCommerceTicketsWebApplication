using eCommerceTicketsWebApi.Data;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;
        private readonly IMoviesRepository _repository;

        public MoviesController(IMoviesService service, IMoviesRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var allMovies = await _repository.GetAllAsync();
            return View(allMovies);
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allMovies.Where(n => n.Name.Contains(searchString) || n.Description.Contains(searchString)).ToList();
                return View("Index", filteredResult);
            }

            return View("Index", allMovies);
        }

        public async Task<IActionResult> Details(int id)
        {
            //var movieDetails = await _service.GetMovieByIdAsync(id);
            var movieDetails = await _repository.GetMovieByIdAsync(id);
            return View(movieDetails);
        }

        public async Task<IActionResult> Create()
        {
            var movieDropdownsData = await _repository.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {
            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _repository.GetNewMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

                return View(movie);
            }

            //await _service.AddNewMovieAsync(movie);
            await _repository.AddAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _repository.GetMovieByIdAsync(id);
            if (movieDetails == null) return View("NotFound");

            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                ImageURL = movieDetails.ImageURL,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                //ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList(),
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate
            };

            var movieDropdownsData = await _repository.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM movie)
        {
            if (id != movie.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _repository.GetNewMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

                return View(movie);
            }

            //await _service.UpdateMovieAsync(movie);
            await _repository.UpdateAsync(id, movie);
            return RedirectToAction(nameof(Index));
        }
    }
}
