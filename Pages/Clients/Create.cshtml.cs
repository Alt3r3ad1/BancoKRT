using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;
using BancoKRT.Pages.Shared;
using Newtonsoft.Json;
using BancoKRT.API.Middlewares;
using System.Net;

namespace BancoKRT.Pages.Clients
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ClientViewModel? clientViewModel { get; set; } = new ClientViewModel();
        public ExceptionViewModel? exceptionViewModel { get; private set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                    string clientIPAddress = HttpContext.Connection.RemoteIpAddress!.ToString();

                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.DefaultRequestHeaders.Add("X-Forwarded-For", clientIPAddress);
                        var content = new StringContent(JsonConvert.SerializeObject(new Client
                        {
                            CPF = clientViewModel.CPF.Trim(),
                            Agency = clientViewModel.Agency,
                            Account = clientViewModel.Account,
                            LimitPIX = clientViewModel.LimitPIX

                        }), System.Text.Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(Helpers.GetApiUrl(Request, $"Client/Create/"), content);
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
            catch (Exception ex)
            {
                throw new HttpException(HttpStatusCode.BadRequest, $"{ex.Message}");
            }
        }
    }
}
