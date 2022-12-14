using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using XSystem.Security.Cryptography;
using Betsolutions.Casino.SDK;
using Betsolutions.Casino.SDK.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Betsolutions.Casino.SDK;
using Betsolutions.Casino.SDK.Services;
using Betsolutions.Casino.SDK.DTO;
using Betsolutions.Casino.SDK.DTO.Game;
using Betsolutions.Casino.SDK.Internal.Repositories;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using XSystem.Security.Cryptography;
using BetSolutions.Models;
using System.Net;
using StatusCodes = Betsolutions.Casino.SDK.StatusCodes;
using System.Xml.Linq;

namespace BetSolutions.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BSController : ControllerBase
    {
        
        private readonly ILogger<BSController> _logger;
        private readonly MerchantAuthInfo _authInfo;
        private readonly Game _game;

        public BSController(ILogger<BSController> logger, MerchantAuthInfo authInfo, Game game)
        {
            _logger = logger;
            _game = game;
            _authInfo = authInfo;
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

        [Route("Auth1")]
        [HttpPost]
        public async Task<IActionResult> Auth()
        {
            AuthModel authModel = new AuthModel();

            authModel.MerchantId = _authInfo.MerchantId;
            authModel.Lang = "ka-GE";
            authModel.GameId = _game.GameId;
            authModel.ProductId = _game.ProductId;
            authModel.IsFreePlay = 1;
            authModel.Platform = "desktop";

            using (HttpClient client = new HttpClient())
            {
                var baseAddress = string.Format("https://auth-staging.betsolutions.com/auth/auth?Token={0}&&MerchantId={1}&&Lang={2}&&GameId={3}&&ProductId={4}&&IsFreePlay={5}&&Platform={6}", null, authModel.MerchantId, authModel.Lang, authModel.GameId, authModel.ProductId, authModel.IsFreePlay, authModel.Platform);
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(authModel);
                var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

                var result = await client.PostAsync(baseAddress, content);

                if (result.IsSuccessStatusCode)
                {
                    return Ok(result);
                }

                return StatusCode((int)StatusCodes.GeneralError);
            }
        }

        [HttpPost]
        [Route("Auth2")]
        public async Task<IActionResult> Auth(string token)
        {
            string privateToken = GetSha256(token);
            using(HttpClient client = new HttpClient())
            {
                var baseAddress = string.Format("https://auth-staging.betsolutions.com/auth/auth?token={0}", privateToken);
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(privateToken);
                var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

                var result = await client.PostAsync(baseAddress, content);

                if (result.IsSuccessStatusCode)
                {
                    return Ok(result);
                }

                return StatusCode((int)StatusCodes.GeneralError);
            }
        }

        //public async Task<IActionResult> GetPlayerInfo(string token, string hash)
        //{

        //}
    }
}