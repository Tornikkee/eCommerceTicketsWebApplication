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
using System.Drawing.Drawing2D;
using Betsolutions.Casino.SDK;
using Betsolutions.Casino.SDK.TableGames.Seka.Enums;
using Betsolutions.Casino.SDK.Enums;
using StatusCodes = Betsolutions.Casino.SDK.StatusCodes;
using AuthInfo = Payment.Models.AuthInfo;
using XAct.Users;

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
            var userId = await _casinoRepository.GetUserIdWithToken(publicToken);

            if (userId == null)
            {
                return StatusCode((int)StatusCodes.UserNotFound);
            }
            var token = await _launchRepository.GetTokenByUserId(userId);
            //string hashedToken = GetSha256(token);

            if (token == publicToken)
            {
                string privateToken = await _casinoRepository.GetPrivateToken(userId);
                var authInfo = new AuthInfo { PrivateToken = privateToken };
                var response = new AuthSuccess { StatusCode = (int)StatusCodes.Success, Data = authInfo };
                return Ok(response);
            }
            else if (token != publicToken)
            {
                return StatusCode((int)StatusCodes.InvalidHash);
            }
            return StatusCode(statusCode: (int)Betsolutions.Casino.SDK.StatusCodes.GeneralError);
        }

        [Route("get-player-info")]
        [HttpPost]
        public async Task<IActionResult> GetPlayerInfo(string token, string hash)
        {
            var userId = await _casinoRepository.GetUserIdWithToken(token);

            if (userId == null)
            {
                return StatusCode((int)StatusCodes.UserNotFound, new UserNotFound { StatusCode = (int)StatusCodes.UserNotFound, ErrorMessage = "UserNotFound" });
            }

            var hashedToken = GetSha256(token);

            if (hashedToken == hash)
            {
                var user = await _casinoRepository.GetUser(userId);
                var response = new SuccessfulResponse { StatusCode = (int)StatusCodes.Success, Data = user };
                return Ok(response);
            }
            return StatusCode(statusCode: (int)Betsolutions.Casino.SDK.StatusCodes.GeneralError);
        }

        [Route("get-balance")]
        [HttpPost]
        public async Task<IActionResult> GetBalance(string token, string hash)
        {
            var userId = await _casinoRepository.GetUserIdWithToken(token);
            var hashedToken = GetSha256(token);
            if (hashedToken == hash)
            {
                var balance = await _walletsRepository.GetBalanceAsync(userId);
                var currBalance = new Balance { CurrentBalance = balance };
                var response = new SuccessfulBalanceResponse { StatusCode = (int)StatusCodes.Success, Data = currBalance };//change
                return Ok(response);
            }
            return StatusCode(statusCode: (int)Betsolutions.Casino.SDK.StatusCodes.GeneralError);
        }

        [Route("Bet")]
        [HttpPost]
        public IActionResult Bet(string token, decimal amount, string transactionId, BetType betType, int gameId, int productId, int roundId, string currency, int campaignId, string campaignName, string hash)
        {
            var rawHash = $"{token}|{amount}|{transactionId}|{betType}|{gameId}|{productId}|{roundId}|{currency}|{campaignId}|{campaignName}";
            var hash1 = GetSha256(rawHash);
            var userId = _casinoRepository.GetUserIdWithToken(token).Result;

            if (userId == null)
            {
                return StatusCode((int)StatusCodes.UserNotFound);
            }

            if (amount <= 0)
            {
                return StatusCode((int)StatusCodes.InvalidAmount);
            }

            //if (hash != hash1)
            //{
            //    return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            //}

            var statusCode = _casinoRepository.Bet(amount, transactionId, betType, currency, userId, out var currentBalance, out var internalTransactionId);

            if (statusCode == 0)
            {
                var betInfo = new BetInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccessfulBetResponse { StatusCode = (int)StatusCodes.Success, Data = betInfo };
                return Ok(response);
            }
            else if (statusCode == -1)
            {
                var betInfo = new BetInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccessfulBetResponse { StatusCode = (int)StatusCodes.AlreadyProcessedTransaction, Data = betInfo };
                return Ok(response);
            }
            else
            {
                return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            }


        }

        [Route("Win")]
        [HttpPost]
        public IActionResult Win(string token, decimal amount, string transactionId, WinType winType, int gameId, int productId, Int64 roundId, string hash, string currency, int campaignId, string campaignName)
        {
            var rawHash = $"{token}|{amount}|{transactionId}|{winType}|{gameId}|{productId}|{roundId}|{currency}|{campaignId}|{campaignName}";
            var hash1 = GetSha256(rawHash);
            var userId = _casinoRepository.GetUserIdWithToken(token).Result;

            if (userId == null)
            {
                return StatusCode((int)StatusCodes.UserNotFound);
            }

            if (amount <= 0)
            {
                return StatusCode((int)StatusCodes.InvalidAmount);
            }

            //if (hash != hash1)
            //{
            //    return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            //}

            var statusCode = _casinoRepository.Win(amount, transactionId, winType, currency, userId, out var currentBalance, out var internalTransactionId);

            if(statusCode == 0)
            {
                var winInfo = new WinInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccessfulWinResponse { StatusCode = (int)StatusCodes.Success, Data = winInfo };
                return Ok(response);
            }
            else if(statusCode == -1)
            {
                var winInfo = new WinInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccessfulWinResponse { StatusCode = (int)StatusCodes.AlreadyProcessedTransaction, Data = winInfo };
                return Ok(response);
            }
            else if(statusCode == -2)
            {
                return Ok(new {StatusCode = (int)StatusCodes.GeneralError });
            }
            else
            {
                return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            }
        }

        [Route("CancelBet")]
        [HttpPost]
        public IActionResult CancelBet(string token, decimal amount, string transactionId, BetType betType, int gameId, int productId, Int64 roundId, string hash, string currency, string betTransactionId)
        {
            var rawHash = $"{token}|{amount}|{transactionId}|{betType}|{gameId}|{productId}|{roundId}|{currency}|{betTransactionId}";
            var hash1 = GetSha256(rawHash);
            var userId = _casinoRepository.GetUserIdWithToken(token).Result;

            if (userId == null)
            {
                return StatusCode((int)StatusCodes.UserNotFound);
            }

            if (amount <= 0)
            {
                return StatusCode((int)StatusCodes.InvalidAmount);
            }

            //if (hash != hash1)
            //{
            //    return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            //}

            var statusCode = _casinoRepository.CancelBet(amount, transactionId, betType, currency, userId, betTransactionId, out var currentBalance, out var internalTransactionId);

            if(statusCode == 0)
            {
                var cancelBetInfo = new CancelBetInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccesfulCancelBetResponse { StatusCode = (int)StatusCodes.Success, Data = cancelBetInfo };
                return Ok(response);
            }
            else if(statusCode == -1)
            {
                var cancelBetInfo = new CancelBetInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccesfulCancelBetResponse { StatusCode = (int)StatusCodes.AlreadyProcessedTransaction, Data = cancelBetInfo };
                return Ok(response);
            }
            else if (statusCode == -2)
            {
                var cancelBetInfo = new CancelBetInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccesfulCancelBetResponse { StatusCode = (int)StatusCodes.Success, Data = cancelBetInfo };
                return Ok(response);
            }
            else
            {
                return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            }
        }

        [Route("ChangeWin")]
        [HttpPost]
        public IActionResult ChangeWin(string token, decimal amount, decimal previousAmount, string transactionId, string previousTransactionId, WinType changeWinType, int gameId, int productId, Int64 roundId, string hash, string currency)
        {
            var rawHash = $"{token}|{amount}|{previousAmount}|{transactionId}|{previousTransactionId}|{changeWinType}|{gameId}|{productId}|{roundId}|{currency}";
            var hash1 = GetSha256(rawHash);
            var userId = _casinoRepository.GetUserIdWithToken(token).Result;

            if (userId == null)
            {
                return StatusCode((int)StatusCodes.UserNotFound);
            }

            if (amount <= 0 || previousAmount <= 0)
            {
                return StatusCode((int)StatusCodes.InvalidAmount);
            }

            //if (hash != hash1)
            //{
            //    return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            //}

            var statusCode = _casinoRepository.ChangeWin(amount, previousAmount, transactionId, previousTransactionId, userId, currency, out var currentBalance, out var internalTransactionId);

            if (statusCode == 0)
            {
                var changeWinInfo = new ChangeWinInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccesfulChangeWinResponse { StatusCode = (int)StatusCodes.Success, Data = changeWinInfo };
                return Ok(response);
            }
            else if (statusCode == -1)
            {
                var changeWinInfo = new ChangeWinInfo { TransactionId = internalTransactionId, CurrentBalance = currentBalance };
                var response = new SuccesfulChangeWinResponse { StatusCode = (int)StatusCodes.AlreadyProcessedTransaction, Data = changeWinInfo };
                return Ok(response);
            }
            else if (statusCode == -2)
            {
                return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            }
            else
            {
                return Ok(new { StatusCode = (int)StatusCodes.GeneralError });
            }
        }
    }
}
