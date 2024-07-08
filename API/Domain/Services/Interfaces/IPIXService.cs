using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;

namespace BancoKRT.API.Domain.Services.Interfaces;

public interface IPIXService
{
    Task<IEnumerable<PIXViewModel>?> GetAllPIXsAsync();
    Task<PIXViewModel?> GetPIXByIdAsync(string id);
    Task<PIXViewModelReturn?> AddPIXAsync(PIX pix);
}
