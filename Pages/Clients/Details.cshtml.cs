using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.API.Domain.ViewModels;
using BancoKRT.Pages.Shared;
using Newtonsoft.Json;

namespace BancoKRT.Pages.Clients
{
    public class DetailsModel : PageModel
    {
        public ClientViewModel? clientViewModel { get; private set; }
        public ExceptionViewModel? exceptionViewModel { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                string clientIPAddress = HttpContext.Connection.RemoteIpAddress!.ToString();

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Add("X-Forwarded-For", clientIPAddress);

                    var response = await client.GetAsync(Helpers.GetApiUrl(Request, $"Client/Detail/{id}"));
                    var responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        clientViewModel = JsonConvert.DeserializeObject<ClientViewModel>(responseData);
                    }
                    else
                    {
                        exceptionViewModel = Helpers.TreatmentException(responseData);

                        return Page();
                    }
                }
            }

            if (clientViewModel == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
