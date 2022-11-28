using eCommerceTicketsWebApplication.Data.Enums;
using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eCommerceTicketsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly ITransactionsRepository _repository;
        private readonly IWalletsRepository _walletsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatusesController(ITransactionsRepository repository, IWalletsRepository walletsRepository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _walletsRepository = walletsRepository;
            _userManager = userManager;
        }

        //userId, amount, transactionId, status
        [Route("get-deposit-data")]
        [HttpGet]
        public async Task<IActionResult> GetDepositData(int transactionId, decimal amount, string status)
        {
            string userId = await _repository.GetUserId();
            var currentBalance = await _walletsRepository.GetBalanceAsync(userId);
            if (status == "success")
            {
                await _repository.DepositById(amount, userId);
                await _repository.RecordTransaction(userId, amount, currentBalance + amount, TransactionType.Deposit, TransactionStatus.Success);
                return Ok();
            }
            await _repository.RecordTransaction(userId, amount, currentBalance, TransactionType.Deposit, TransactionStatus.Failed);
            return Ok();
        }

        [Route("get-withdraw-data")]
        [HttpGet]
        public async Task<IActionResult> GetWithdrawData(int transactionId, decimal amount, string status)
        {
            string userId = await _repository.GetUserId();
            var currentBalance = await _walletsRepository.GetBalanceAsync(userId);
            if (status == "success")
            {
                await _repository.RecordTransaction(userId, amount, currentBalance, TransactionType.Withdrawal, TransactionStatus.Success);
                return Ok();
            }
            await _repository.DepositById(amount, userId);
            await _repository.RecordTransaction(userId, amount, currentBalance + amount, TransactionType.Withdrawal, TransactionStatus.Failed);
            return Ok();
        }

        [Route("get-payment-data")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentData(int transactionId, decimal amount, string status)
        {
            string userId = await _repository.GetUserId();
            var currentBalance = await _walletsRepository.GetBalanceAsync(userId);
            if (status == "success")
            {
                await _repository.PayById(amount, userId);
                await _repository.RecordTransaction(userId, amount, currentBalance - amount, TransactionType.Payment, TransactionStatus.Success);
                return Ok();
            }
            await _repository.RecordTransaction(userId, amount, currentBalance, TransactionType.Payment, TransactionStatus.Failed);
            return Ok();
        }
    }
}
