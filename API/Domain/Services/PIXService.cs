using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;
using BancoKRT.API.Domain.Services.Interfaces;
using BancoKRT.API.Infrastructure.Repositories.Interfaces;
using System.Net;
using BancoKRT.API.Middlewares;

namespace BancoKRT.API.Domain.Services;

public class PIXService : IPIXService
{
    private readonly IPIXRepository _pixRepository;
    private readonly IClientService _clientService;

    public PIXService(IPIXRepository pixRepository, IClientService clientService)
    {
        _pixRepository = pixRepository;
        _clientService = clientService;
    }

    public async Task<IEnumerable<PIXViewModel>?> GetAllPIXsAsync()
    {
        try
        {
            var pix = await _pixRepository.GetAllAsync();
            if (pix.Count() > 0)
                return pix.Select(s => new PIXViewModel
                {
                    Id = s.Id,
                    ClientCPF = s.ClientCPF,
                    Date = s.Date,
                    Value = s.Value,
                }).ToList();
            else
                throw new HttpException(HttpStatusCode.NotFound, "Not Found PIXs");
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }

    public async Task<PIXViewModel?> GetPIXByIdAsync(string id)
    {
        try
        {
            var pix = await _pixRepository.GetByIdAsync(id);
            if (pix is not null)
                return new PIXViewModel
                {
                    Id = pix.Id,
                    ClientCPF = pix.ClientCPF,
                    Date = pix.Date,
                    Value = pix.Value,
                };
            else
                throw new HttpException(HttpStatusCode.NotFound, "PIX Not Found");
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }

    public async Task<PIXViewModelReturn?> AddPIXAsync(PIX pix)
    {
        try
        {
            var client = await _clientService.GetClientByIdAsync(pix.ClientCPF);
            if (client is not null)
            {
                var tuple = await _clientService.ChangeLimitClientAsync(pix.ClientCPF, pix.Value);
                pix.Id = Guid.NewGuid().ToString();

                await _pixRepository.AddAsync(pix);
                return new PIXViewModelReturn
                {
                    Id = pix.Id,
                    PreviousBalance = tuple.Item1,
                    Balance = tuple.Item2
                };
            }
            else
            {
                return null;
            }
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }
}
