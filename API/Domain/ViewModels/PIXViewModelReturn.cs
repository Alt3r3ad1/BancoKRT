namespace BancoKRT.API.Domain.ViewModels;

public class PIXViewModelReturn
{
    public string? Id { get; set; }
    public double PreviousBalance { get; set; } = 0.00;
    public double Balance { get; set; } = 0.00;
}
