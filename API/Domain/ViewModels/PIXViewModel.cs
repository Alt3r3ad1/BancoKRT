namespace BancoKRT.API.Domain.ViewModels;

public class PIXViewModel
{
    public string? Id { get; set; }
    public string ClientCPF { get; set; } = string.Empty;
    public DateTime? Date { get; set; } = DateTime.Now;
    public double Value { get; set; } = 0.00;
}
