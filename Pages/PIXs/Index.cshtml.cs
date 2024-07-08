using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.Pages.Shared;
using Newtonsoft.Json;
using BancoKRT.API.Domain.ViewModels;

namespace BancoKRT.Pages.PIXs
{
    public class IndexModel : PageModel
    {
        public IEnumerable<PIXViewModel>? pixViewModel { get; private set; }
        public ExceptionViewModel? exceptionViewModel { get; private set; }

        public async Task OnGetAsync()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                string clientIPAddress = HttpContext.Connection.RemoteIpAddress!.ToString();

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Add("X-Forwarded-For", clientIPAddress);

                    var response = await client.GetAsync(Helpers.GetApiUrl(Request, "PIX/Details"));
                    var responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        pixViewModel = JsonConvert.DeserializeObject<IEnumerable<PIXViewModel>>(responseData);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        pixViewModel = new List<PIXViewModel>();
                    }
                    else
                    {
                        exceptionViewModel = Helpers.TreatmentException(responseData);
                    }
                }
            }
        }
    }
}
