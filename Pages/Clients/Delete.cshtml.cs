using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.API.Domain.ViewModels;
using BancoKRT.Pages.Shared;
using Newtonsoft.Json;
using System.Net;
using BancoKRT.API.Middlewares;

namespace BancoKRT.Pages.Clients
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public ClientViewModel? clientViewModel { get; set; }
        public ExceptionViewModel? exceptionViewModel { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
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
            catch (Exception ex)
            {
                throw new HttpException(HttpStatusCode.BadRequest, $"{ex.Message}");
            }

        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                    string clientIPAddress = HttpContext.Connection.RemoteIpAddress!.ToString();

                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.DefaultRequestHeaders.Add("X-Forwarded-For", clientIPAddress);

                        var response = await client.DeleteAsync(Helpers.GetApiUrl(Request, $"Client/Delete/{id}"));
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
