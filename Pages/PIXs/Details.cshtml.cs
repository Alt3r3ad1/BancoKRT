using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.API.Domain.ViewModels;
using BancoKRT.Pages.Shared;
using Newtonsoft.Json;

namespace BancoKRT.Pages.PIXs
{
    public class DetailsModel : PageModel
    {
        public PIXViewModel? pixViewModel { get; private set; }
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

                    var response = await client.GetAsync(Helpers.GetApiUrl(Request, $"PIX/Detail/{id}"));
                    var responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        pixViewModel = JsonConvert.DeserializeObject<PIXViewModel>(responseData);
                    }
                    else
                    {
                        exceptionViewModel = Helpers.TreatmentException(responseData);
                    }
                }
            }

            if (pixViewModel == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
