using BancoKRT.API.Domain.Models;
using BancoKRT.API.Domain.ViewModels;
using BancoKRT.API.Domain.Services.Interfaces;
using BancoKRT.API.Infrastructure.Repositories.Interfaces;
using System.Net;
using BancoKRT.API.Middlewares;
using System.Globalization;
using Amazon.DynamoDBv2.Model;

namespace BancoKRT.API.Domain.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<ClientViewModel>?> GetAllClientsAsync()
    {
        try
        {
            var client = await _clientRepository.GetAllAsync();
            if (client.Count() > 0)
                return client.Select(s => new ClientViewModel
                {
                    CPF = s.CPF,
                    Agency = s.Agency,
                    Account = s.Account,
                    LimitPIX = s.LimitPIX
                }).ToList();
            else
                throw new HttpException(HttpStatusCode.NotFound, "There are no clients");
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }

    public async Task<ClientViewModel?> GetClientByIdAsync(string? id)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client is not null)
                return new ClientViewModel
                {
                    CPF = client.CPF,
                    Agency = client.Agency,
                    Account = client.Account,
                    LimitPIX = client.LimitPIX
                };
            else
                throw new HttpException(HttpStatusCode.NotFound, "Client not found");
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }

    public async Task<ClientViewModel?> AddClientAsync(Client client)
    {
        try
        {

            var clientDB = await _clientRepository.GetByIdAsync(client.CPF);

            if (clientDB is null)
            {
                client.LimitPIX = Convert.ToDecimal(client.LimitPIX.ToString("0.00", CultureInfo.InvariantCulture));
                await _clientRepository.AddAsync(client);
                return await GetClientByIdAsync(client.CPF);
            }
            else
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Client already exists");
            }
            
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }

    public async Task<ClientViewModel?> UpdateClientAsync(ClientUpdate clientUpdate)
    {
        try
        {
            clientUpdate.LimitPIX = Convert.ToDecimal(clientUpdate.LimitPIX.ToString("0.00", CultureInfo.InvariantCulture));
            var request = new UpdateItemRequest
            {
                TableName = "Client",
                Key = new Dictionary<string, AttributeValue>
            {
                { "CPF", new AttributeValue { S = clientUpdate.CPF } }
            },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":newPrice", new AttributeValue { N = clientUpdate.LimitPIX.ToString() } }
            },
                UpdateExpression = "SET LimitPIX = :newPrice"
            };
            await _clientRepository.UpdateItemAsync(request);
            return await GetClientByIdAsync(clientUpdate.CPF);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }

    public async Task DeleteClientAsync(string id)
    {
        var client = await GetClientByIdAsync(id);

        try
        {
            await _clientRepository.DeleteAsync(id);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }

    public async Task<Tuple<decimal, decimal>> ChangeLimitClientAsync(string id, decimal valueDecrease)
    {
        var clientViewModel = await GetClientByIdAsync(id);
        decimal PreviousBalance = Convert.ToDecimal(-1.00);
        decimal NewBalance = Convert.ToDecimal(-1.00);

        try
        {
            if (clientViewModel is not null)
            {
                PreviousBalance = clientViewModel.LimitPIX;
                NewBalance = clientViewModel.LimitPIX - valueDecrease;
                if (NewBalance >= 0)
                {
                    clientViewModel.LimitPIX = NewBalance;
                    var clientUpdate = new ClientUpdate
                    {
                        CPF = clientViewModel.CPF,
                        LimitPIX = clientViewModel.LimitPIX
                    };
                    await UpdateClientAsync(clientUpdate);
                    return Tuple.Create(PreviousBalance, NewBalance);
                }
                else
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Client without sufficient limit");
                }
            }

            return Tuple.Create(PreviousBalance, NewBalance);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.BadRequest));
        }
    }
}
