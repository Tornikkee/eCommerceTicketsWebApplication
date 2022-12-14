using eCommerceTicketsWebApplication.Data.Cart;
using eCommerceTicketsWebApplication.Data.DTOS;
using eCommerceTicketsWebApplication.Data.Enums;
using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.DTOS;
using eCommerceTicketsWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Unicode;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class TransactionsController : Controller
    {
        ITransactionsRepository _repository;
        UserManager<ApplicationUser> _userManager;
        IWalletsRepository _walletsRepository;
        ShoppingCart _shoppingCart;

        public TransactionsController(ITransactionsRepository repository, UserManager<ApplicationUser> userManager, IWalletsRepository walletsRepository, ShoppingCart shoppingCart)
        {
            _repository = repository;
            _userManager = userManager;
            _walletsRepository = walletsRepository;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(DepositDTO depositDTO)
        {
            var user = await _userManager.FindByEmailAsync(depositDTO.Email);

            if (user == null || depositDTO.Email != user.Email)
            {
                return View("WrongEmail");
            }

            var wallet = await _walletsRepository.GetWalletByUserId(user.Id);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, depositDTO.Password);

                if (passwordCheck)
                {
                    if (depositDTO.Amount > 0)
                    {
                        await _repository.RecordTransaction(user.Id, depositDTO.Amount, wallet.Balance, TransactionType.Deposit, TransactionStatus.Pending);
                        int transactionId = await _repository.GetLastTransactionId();
                        var baseAddress = string.Format("https://localhost:44373/Payment/Deposit");
                        var newAddress = string.Format("{0}?amount={1}&&transactionId={2}", baseAddress, depositDTO.Amount.ToString(), transactionId);
                        return Redirect(newAddress);
                    }
                    else
                    {
                        return View("NegativeAmount");
                    }
                }
                else
                {
                    return View("WrongPassword");
                }
            }
            return View();
        }

        public IActionResult SuccessfulDeposit()
        {
            return View();
        }

        public IActionResult SuccessfulWithdrawal()
        {
            return View();
        }

        public IActionResult SuccessfulPayment()
        {
            return View();
        }

        public IActionResult TransactionFailed()
        {
            return View();
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawDTO withdrawDTO)
        {
            var user = await _userManager.FindByEmailAsync(withdrawDTO.Email);

            if (user == null || withdrawDTO.Email != user.Email)
            {
                return View("WrongEmail");
            }

            var wallet = await _walletsRepository.GetWalletByUserId(user.Id);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, withdrawDTO.Password);

                if (passwordCheck)
                {
                    if (withdrawDTO.Amount <= wallet.Balance)
                    {
                        if (withdrawDTO.Amount > 0)
                        {
                            await _repository.WithdrawById(withdrawDTO.Amount, user.Id);
                            await _repository.RecordTransaction(user.Id, withdrawDTO.Amount, wallet.Balance - withdrawDTO.Amount, TransactionType.Withdrawal, TransactionStatus.Pending);
                            int transactionId = await _repository.GetLastTransactionId();
                            var baseAddress = string.Format("https://localhost:44373/Payment/Withdraw");
                            var newAddress = string.Format("{0}?amount={1}&&transactionId={2}", baseAddress, withdrawDTO.Amount.ToString(), transactionId);
                            return Redirect(newAddress);
                        }
                        else
                        {
                            return View("NegativeAmount");
                        }
                    }
                    else
                    {
                        return View("NotEnoughBalance1");
                    }
                }
                else
                {
                    return View("WrongPassword");
                }
            }
            return View();
        }

        public IActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Pay(PayDTO payDTO)
        {
            var user = await _userManager.FindByEmailAsync(payDTO.Email);

            if (user == null || payDTO.Email != user.Email)
            {
                return View("WrongEmail");
            }

            var wallet = await _walletsRepository.GetWalletByUserId(user.Id);
            var amount = (decimal)_shoppingCart.GetShoppingCartTotal();



            if (user != null)
            {
                bool passwordCheck = await _userManager.CheckPasswordAsync(user, payDTO.Password);

                if (passwordCheck)
                {
                    if (wallet.Balance >= amount)
                    {
                        await _repository.RecordTransaction(user.Id, amount, wallet.Balance, TransactionType.Payment, TransactionStatus.Pending);
                        int transactionId = await _repository.GetLastTransactionId();
                        var baseAddress = string.Format("https://localhost:44373/Payment/Payment");
                        var newAddress = string.Format("{0}?amount={1}&&transactionId={2}", baseAddress, amount.ToString(), transactionId);
                        return Redirect(newAddress);
                    }
                    else
                    {
                        return View("NotEnoughBalance");
                    }
                }
                else
                {
                    return View("WrongPassword");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TransactionHistory()
        {
            var transactions = await _repository.TransactionHistory(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Json(new { data = transactions });
        }

        [HttpGet]
        public IActionResult TransactionsHistoryTable()
        {
            return View();
        }
    }
}
