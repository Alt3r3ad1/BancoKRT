using System.Text.Json.Serialization;

namespace BancoKRT.API.Domain.ViewModels;

public class ExceptionViewModel
{
    public string? message { get; set; }
    public Dictionary<string, IEnumerable<string>> errors { get; set; }
    public string? traceId { get; set; }
}