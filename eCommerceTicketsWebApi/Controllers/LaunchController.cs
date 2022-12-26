using eCommerceTicketsWebApplication.Data.Repositories;
using eCommerceTicketsWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using XSystem.Security.Cryptography;

namespace eCommerceTicketsWebApplication.Controllers
{
    public class LaunchController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        ILaunchRepository _repository;

        public LaunchController(UserManager<ApplicationUser> userManager, ILaunchRepository repository)
        {
            _repository = repository;
            _userManager = userManager;
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

        public async Task<IActionResult> GameLounch(string token, string privateToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _repository.FillUsersAndTokens(token, userId, privateToken);
            return Ok();
        }
    }
}
