using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;
using BancoKRT.Pages.Shared;
using Newtonsoft.Json;

namespace BancoKRT.Pages.Clients
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public ClientViewModel? clientViewModel { get; set; } = new ClientViewModel();
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

        public async Task<IActionResult> OnPostAsync()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                string clientIPAddress = HttpContext.Connection.RemoteIpAddress!.ToString();

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Add("X-Forwarded-For", clientIPAddress);
                    var content = new StringContent(JsonConvert.SerializeObject(new ClientUpdate
                    {
                        CPF = clientViewModel.CPF.Trim(),
                        LimitPIX = clientViewModel.LimitPIX

                    }), System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(Helpers.GetApiUrl(Request, $"Client/Update/"), content);
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

            return RedirectToPage("Index");
        }
    }
}
