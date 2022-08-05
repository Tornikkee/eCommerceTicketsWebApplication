using eCommerceTicketsWebApi.Data;
using eCommerceTicketsWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signingManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signingManager, AppDbContext context)
        {
            _userManager = userManager;
            _signingManager = signingManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
