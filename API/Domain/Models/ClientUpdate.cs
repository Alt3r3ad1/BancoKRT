using System.ComponentModel.DataAnnotations;

namespace BancoKRT.API.Domain.Models;

public class ClientUpdate
{
    [Required(ErrorMessage = "CPF is required.")]
    [RegularExpression(@"\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "CPF must be in the format 000.000.000-00.")]
    public string? CPF { get; set; } = string.Empty;
    [Range(0, double.MaxValue, ErrorMessage = "Limit cannot be negative.")]
    public double LimitPIX { get; set; }
}
