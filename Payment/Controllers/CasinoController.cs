using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web.Helpers;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Payment.Repositories;
using Payment.Models;
using eCommerceTicketsWebApplication.Data.Enums;
using XSystem.Security.Cryptography;

namespace Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasinoController : ControllerBase
    {
        private readonly ILaunchRepository _launchRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ICasinoRepository _casinoRepository;
        private readonly IWalletsRepository _walletsRepository;

        public CasinoController(ILaunchRepository launchRepository, ITransactionsRepository transactionsRepository, ICasinoRepository casinoRepository, IWalletsRepository walletsRepository)
        {
            _launchRepository = launchRepository;
            _transactionsRepository = transactionsRepository;
            _casinoRepository = casinoRepository;
            _walletsRepository = walletsRepository;
        }

        private static string GetSha256(string text)
        {
            var utf8Encoding = new UTF8Encoding();
            var message = utf8Encoding.GetBytes(text);

            var hashString = new SHA256Managed();
            var hex = string.Empty;

            var hashValue = hashString.ComputeHash(message);

            return hashValue.Aggregate(hex, (current, bt) => current + $"{bt:x2}");
        }

        [Route("Auth")]
        [HttpPost]
        public async Task<IActionResult> Auth(string publicToken)
        {
            var userId = await _transactionsRepository.GetUserId();
            var token = await _launchRepository.GetTokenByUserId(userId);
            string hashedToken = GetSha256(token);

            if (hashedToken == publicToken)
            {
                return Ok();
            }
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }

        [Route("get-player-info")]
        [HttpPost]
        public async Task<IActionResult> GetPlayerInfo(string token, string hash)
        {
            var userId = await _casinoRepository.GetUserIdWithToken(token);
            var hashedToken = GetSha256(token);
            if(hashedToken == hash)
            {
                var user = await _casinoRepository.GetUser(userId);
                var response = new SuccessfulResponse { StatusCode = (int)StatusCodes.Status200OK, Data = user };//change
                return Ok(response);
            }
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }

        [Route("get-balance")]
        [HttpPost]
        public async Task<IActionResult> GetBalance(string token, string hash)
        {
            var userId = await _casinoRepository.GetUserIdWithToken(token);
            var hashedToken = GetSha256(token);
            if(hashedToken == hash)
            {
                var balance = await _walletsRepository.GetBalanceAsync(userId);
                var currBalance = new Balance { CurrentBalance = balance };
                var response = new SuccessfulBalanceResponse { StatusCode = (int)StatusCodes.Status200OK, Data = currBalance };//change
                return Ok(response);
            }
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }

        [Route("Bet")]
        [HttpPost]
        public async Task<IActionResult> Bet(string token, string hash, decimal amount, string currency)
        {
            var userId = await _casinoRepository.GetUserIdWithToken(token);
            var hashedToken = GetSha256(token);
            if(hashedToken == hash)
            {
                 var balance = await _walletsRepository.GetBalanceAsync(userId);
                if(balance >= amount)
                {
                    //Bet Procedure //Win procedure //CancelBet procedure //unique transaction ensure cancel duplicates

                    await _transactionsRepository.Bet(amount, userId);
                    await _transactionsRepository.RecordCasinoTransaction(userId, amount, balance - amount, TransactionStatus.Success, currency);
                    var currentBalance = await _walletsRepository.GetBalanceAsync(userId);
                    var transactionId = await _transactionsRepository.GetLastTransactionId();
                    var betInfo = new BetInfo { TransactionId = transactionId, CurrentBalance = currentBalance };
                    var response = new SuccessfulBetResponse { StatusCode = (int)StatusCodes.Status200OK, Data = betInfo };
                    return Ok(response);
                }
            }
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
