using System.ComponentModel.DataAnnotations;

namespace BancoKRT.API.Domain.Models;

public class Client
{
    [Required(ErrorMessage = "CPF is required.")]
    [RegularExpression(@"\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "CPF must be in the format 000.000.000-00.")]
    public string CPF { get; set; } = string.Empty;
    [Required(ErrorMessage = "Agency number is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Agency number must be valid.")]
    public int Agency { get; set; }
    [Required(ErrorMessage = "Account number is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Account number must be valid.")]
    public int Account { get; set; }
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Limit cannot be negative.")]
    public decimal LimitPIX { get; set; }
    public DateTime RegistrationDate { get; set; }
}
