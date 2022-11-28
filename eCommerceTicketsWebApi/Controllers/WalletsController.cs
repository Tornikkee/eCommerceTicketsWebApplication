using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Data.Static;
using eCommerceTicketsWebApplication.Data.ViewModels;
using eCommerceTicketsWebApplication.DTOS;
using eCommerceTicketsWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Data.SqlTypes;
using System.Security.Claims;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class WalletsController : Controller
    {
        IWalletsRepository _repository;
        UserManager<ApplicationUser> _userManager;

        public WalletsController(IWalletsRepository repository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var balance = await _repository.GetBalanceAsync(userId);
            return Json(new { balance = balance });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWalletDTO walletDTO)
        {
            var user = await _userManager.FindByEmailAsync(walletDTO.Email);
            Wallet? wallet = await _repository.GetWalletByUserId(user.Id);
            
            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, walletDTO.Password);
                if (passwordCheck)
                {
                    if (wallet == null)
                    {
                        await _repository.CreateWallet(user.Id);
                        return View("WalletCreationCompleted");
                    }
                    else
                    {
                        return View("WalletExists");
                    }
                }
            }
            return View();
        }
    }
}
