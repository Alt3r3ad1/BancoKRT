using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;
using BancoKRT.Pages.Shared;
using Newtonsoft.Json;
using BancoKRT.API.Middlewares;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BancoKRT.Pages.PIXs
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public PIXViewModel? pixViewModel { get; set; } = new PIXViewModel();
        public ExceptionViewModel? exceptionViewModel { get; private set; }
        public List<SelectListItem>? clients { get; set; }

        public async Task<IActionResult> OnGetAsync()
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

                        var response = await client.GetAsync(Helpers.GetApiUrl(Request, $"Client/Details/"));
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var items = JsonConvert.DeserializeObject<IEnumerable<ClientViewModel>>(responseData);
                            clients = new List<SelectListItem>();
                            if (items is not null)
                                foreach (var item in items)
                                {
                                    clients.Add(new SelectListItem { Value = item.CPF, Text = item.CPF });
                                }
                        }
                        else
                        {
                            exceptionViewModel = Helpers.TreatmentException(responseData);
                        }
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw new HttpException(HttpStatusCode.BadRequest, $"{ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostAsync()
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
                        var content = new StringContent(JsonConvert.SerializeObject(new PIX
                        {
                            Id = pixViewModel.Id,
                            ClientCPF = pixViewModel.ClientCPF,
                            Date = pixViewModel.Date,
                            Value = pixViewModel.Value,

                        }), System.Text.Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(Helpers.GetApiUrl(Request, $"PIX/Create/"), content);
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            pixViewModel = JsonConvert.DeserializeObject<PIXViewModel>(responseData);
                        }
                        else
                        {
                            exceptionViewModel = Helpers.TreatmentException(responseData);

                            return await OnGetAsync();
                        }
                    }
                }

                return RedirectToPage("Index");
            }
            catch (HttpException ex)
            {
                exceptionViewModel = new ExceptionViewModel
                {
                    message = ex.Message,
                    traceId = ex.StackTrace
                };
                throw new HttpException(HttpStatusCode.BadRequest, $"{ex.Message}");
            }
        }
    }
}
