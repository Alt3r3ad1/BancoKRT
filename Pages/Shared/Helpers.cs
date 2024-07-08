using BancoKRT.API.Domain.ViewModels;
using BancoKRT.API.Middlewares;
using Newtonsoft.Json;
using System.Net;

namespace BancoKRT.Pages.Shared;

public static class Helpers
{
    public static string GetApiUrl(HttpRequest request, string relativePath)
    {
        string baseUrl = $"{request.Scheme}://{request.Host}";
        Uri baseUri = new Uri(baseUrl);
        Uri combinedUri = new Uri(baseUri, relativePath);
        return combinedUri.ToString();
    }

    public static ExceptionViewModel? TreatmentException(string responseData)
    {
        try
        {
            var exceptionViewModel = new ExceptionViewModel();
            exceptionViewModel = JsonConvert.DeserializeObject<ExceptionViewModel>(responseData);

            if (exceptionViewModel is not null)
            {
                if (exceptionViewModel.message is null)
                    if (exceptionViewModel.errors.Any())
                    {
                        exceptionViewModel.message = string.Join("; ", exceptionViewModel.errors.Select(s => string.Concat(s.Key, ": ", s.Value.First())));
                    }
            }

            return exceptionViewModel;
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }

    }
}
