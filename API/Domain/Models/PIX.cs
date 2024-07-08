namespace BancoKRT.API.Domain.Models;

public class PIX
{
    public string? Id { get; set; }
    public string ClientCPF { get; set; }
    public DateTime? Date { get; set; }
    public double Value { get; set; }
}
