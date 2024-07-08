using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;

namespace BancoKRT.API.Infrastructure.Repositories.Interfaces;

public interface IPIXRepository
{
    Task<IEnumerable<PIX>> GetAllAsync();
    Task<PIX> GetByIdAsync(string id);
    Task AddAsync(PIX pix);
}
