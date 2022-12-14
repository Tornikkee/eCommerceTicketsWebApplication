using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Payment.Controllers
{
    public class PaymentController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Deposit(string transactionId, decimal amount)
        {
            ViewBag.TransactionId = transactionId;
            ViewBag.Amount = amount;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendAcceptDepositData(int transactionId, decimal amount)
        {
            using (var client = new HttpClient())
            {
                string status = "success";
                var baseAddress = string.Format("https://localhost:44304/api/Statuses/get-deposit-data");
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var newAddress = string.Format("{0}?transactionId={1}&&amount={2}&&status={3}", baseAddress, transactionId, amount.ToString(), status);

                HttpResponseMessage response = await client.GetAsync(newAddress);
                response.EnsureSuccessStatusCode();

                return Redirect("https://localhost:44304/Transactions/SuccessfulDeposit");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SendRejectDepositData(int transactionId, decimal amount)
        {
            using (var client = new HttpClient())
            {
                string status = "failed";
                var baseAddress = string.Format("https://localhost:44304/api/Statuses/get-deposit-data");
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var newAddress = string.Format("{0}?transactionId={1}&&amount={2}&&status={3}", baseAddress, transactionId, amount.ToString(), status);

                HttpResponseMessage response = await client.GetAsync(newAddress);
                response.EnsureSuccessStatusCode();

                return Redirect("https://localhost:44304/Transactions/TransactionFailed");
            }
        }

        [HttpGet]
        public IActionResult Withdraw(int transactionId, decimal amount)
        {
            ViewBag.TransactionId = transactionId;
            ViewBag.Amount = amount;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendAcceptWithdrawData(int transactionId, decimal amount)
        {
            using (var client = new HttpClient())
            {
                string status = "success";
                var baseAddress = string.Format("https://localhost:44304/api/Statuses/get-withdraw-data");
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var newAddress = string.Format("{0}?transactionId={1}&&amount={2}&&status={3}", baseAddress, transactionId, amount.ToString(), status);

                HttpResponseMessage response = await client.GetAsync(newAddress);
                response.EnsureSuccessStatusCode();

                return Redirect("https://localhost:44304/Transactions/SuccessfulWithdrawal");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SendRejectWithdrawData(int transactionId, decimal amount)
        {
            using (var client = new HttpClient())
            {
                string status = "failed";
                var baseAddress = string.Format("https://localhost:44304/api/Statuses/get-withdraw-data");
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var newAddress = string.Format("{0}?transactionId={1}&&amount={2}&&status={3}", baseAddress, transactionId, amount.ToString(), status);

                HttpResponseMessage response = await client.GetAsync(newAddress);
                response.EnsureSuccessStatusCode();

                return Redirect("https://localhost:44304/Transactions/TransactionFailed");
            }
        }

        [HttpGet]
        public IActionResult Payment(int transactionId, decimal amount)
        {
            ViewBag.TransactionId = transactionId;
            ViewBag.Amount = amount;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendAcceptPaymentData(int transactionId, decimal amount)
        {
            using (var client = new HttpClient())
            {
                string status = "success";
                var baseAddress = string.Format("https://localhost:44304/api/Statuses/get-payment-data");
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var newAddress = string.Format("{0}?transactionId={1}&&amount={2}&&status={3}", baseAddress, transactionId, amount.ToString(), status);

                HttpResponseMessage response = await client.GetAsync(newAddress);
                response.EnsureSuccessStatusCode();

                return Redirect("https://localhost:44304/Transactions/SuccessfulPayment");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SendRejectPaymentData(int transactionId, decimal amount)
        {
            using (var client = new HttpClient())
            {
                string status = "failed";
                var baseAddress = string.Format("https://localhost:44304/api/Statuses/get-payment-data");
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var newAddress = string.Format("{0}?transactionId={1}&&amount={2}&&status={3}", baseAddress, transactionId, amount.ToString(), status);

                HttpResponseMessage response = await client.GetAsync(newAddress);
                response.EnsureSuccessStatusCode();

                return Redirect("https://localhost:44304/Transactions/TransactionFailed");
            }
        }
    }
}
