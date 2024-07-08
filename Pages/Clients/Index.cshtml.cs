using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.Pages.Shared;
using Newtonsoft.Json;
using BancoKRT.API.Domain.ViewModels;

namespace BancoKRT.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public IEnumerable<ClientViewModel>? clientViewModel { get; private set; }
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

                    var response = await client.GetAsync(Helpers.GetApiUrl(Request, "Client/Details"));
                    var responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        clientViewModel = JsonConvert.DeserializeObject<IEnumerable<ClientViewModel>>(responseData);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        clientViewModel = new List<ClientViewModel>();
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
