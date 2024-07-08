using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.Pages.Shared;

namespace BancoKRT.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string? Message { get; set; }

        public async Task<IActionResult> OnGet()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                string clientIPAddress = HttpContext.Connection.RemoteIpAddress!.ToString();

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Add("X-Forwarded-For", clientIPAddress);
                    var content = new StringContent(string.Empty, System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(Helpers.GetApiUrl(Request, "/DynamoDB/CreateTables"), content);

                    if (response.IsSuccessStatusCode)
                    {
                        Message = "Database successfully synchronized.";
                    }
                    else
                    {
                        Message = "Database synchronization failure.";
                    }
                }
            }

            return Page();
        }
    }
}
