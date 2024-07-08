using Amazon.DynamoDBv2.Model;
using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;

namespace BancoKRT.API.Infrastructure.Repositories.Interfaces;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client> GetByIdAsync(string? id);
    Task AddAsync(Client client);
    Task UpdateItemAsync(UpdateItemRequest request);
    Task DeleteAsync(string id);
}
