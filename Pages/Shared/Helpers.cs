using BancoKRT.API.Domain.ViewModels;
using Newtonsoft.Json;

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
        var exceptionViewModel = new ExceptionViewModel();
        exceptionViewModel = JsonConvert.DeserializeObject<ExceptionViewModel>(responseData);

        if (exceptionViewModel is not null)
        {
            if (exceptionViewModel.message is null)
                if (exceptionViewModel.errors.Count() > 0)
                {
                    exceptionViewModel.message = string.Join("; ", exceptionViewModel.errors.Select(s => string.Concat(s.Key, ": ", s.Value.First())));
                }
        }

        return exceptionViewModel;
    }
}
