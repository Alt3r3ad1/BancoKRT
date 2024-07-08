using Amazon.DynamoDBv2.DataModel;
using BancoKRT.API.Domain.Models;
using BancoKRT.API.Infrastructure.Repositories.Interfaces;

namespace BancoKRT.Infrastructure.Repository;

public class PIXRepository : IPIXRepository
{
    private readonly IDynamoDBContext _context;

    public PIXRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PIX>> GetAllAsync()
    {
        var conditions = new List<ScanCondition>();
        return await _context.ScanAsync<PIX>(conditions).GetRemainingAsync();
    }

    public async Task<PIX> GetByIdAsync(string id)
    {
        return await _context.LoadAsync<PIX>(id);
    }

    public async Task AddAsync(PIX pix)
    {
        await _context.SaveAsync(pix);
    }
}

