namespace BancoKRT.API.Domain.ViewModels;

public class ClientViewModel
{
    public string? CPF { get; set; } = string.Empty;
    public int Agency { get; set; } = 0;
    public int Account { get; set; } = 0;
    public double LimitPIX { get; set; } = 0.00;
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}
