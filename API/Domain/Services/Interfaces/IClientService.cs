using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;

namespace BancoKRT.API.Domain.Services.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientViewModel>?> GetAllClientsAsync();
    Task<ClientViewModel?> GetClientByIdAsync(string? id);
    Task<ClientViewModel?> AddClientAsync(Client client);
    Task<ClientViewModel?> UpdateClientAsync(ClientUpdate clientUpdate);
    Task DeleteClientAsync(string id);
    Task<Tuple<decimal, decimal>> ChangeLimitClientAsync(string id, decimal valueDecrease);
}
