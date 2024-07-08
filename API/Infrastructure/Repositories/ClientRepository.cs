using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using BancoKRT.API.Domain.Models;
using BancoKRT.API.Infrastructure.Repositories.Interfaces;

namespace BancoKRT.Infrastructure.Repository;

public class ClientRepository : IClientRepository
{
    private readonly IDynamoDBContext _context;
    private readonly IAmazonDynamoDB _dynamoDB;

    public ClientRepository(IDynamoDBContext context, IAmazonDynamoDB dynamoDB)
    {
        _context = context;
        _dynamoDB = dynamoDB;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        var conditions = new List<ScanCondition>();
        return await _context.ScanAsync<Client>(conditions).GetRemainingAsync();
    }

    public async Task<Client> GetByIdAsync(string? id)
    {
        return await _context.LoadAsync<Client>(id);
    }

    public async Task AddAsync(Client client)
    {
        await _context.SaveAsync(client);
    }

    public async Task UpdateItemAsync(UpdateItemRequest request)
    {
        await _dynamoDB.UpdateItemAsync(request);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.DeleteAsync<Client>(id);
    }
}

