using Microsoft.AspNetCore.Mvc;
using BancoKRT.API.Domain.Services.Interfaces;
using BancoKRT.API.Domain.Models;
using BancoKRT.API.Middlewares;
using System.Net;

namespace BancoKRT.API.Controllers;

[ApiController]
[Route("/")]
public class ClientController : ControllerBase
{
    [HttpGet]
    [Route("[controller]/Details/")]
    public async Task<IActionResult> Details([FromServices] IClientService _clientService)
    {
        try
        {
            var clientResponse = await _clientService.GetAllClientsAsync();
            return new JsonResult(clientResponse);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }
    }

    [HttpGet]
    [Route("[controller]/Detail/{id}")]
    public async Task<IActionResult> Detail(string id, [FromServices] IClientService _clientService)
    {
        try
        {
            var clientResponse = await _clientService.GetClientByIdAsync(id);
            return new JsonResult(clientResponse);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }
    }

    [HttpPost]
    [Route("[controller]/Create")]
    public async Task<IActionResult> Create([FromBody] Client client, [FromServices] IClientService _clientService)
    {
        try
        {
            var clientResponse = await _clientService.AddClientAsync(client);
            return new JsonResult(clientResponse);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }
    }

    [HttpPut]
    [Route("[controller]/Update")]
    public async Task<IActionResult> Update([FromBody] ClientUpdate clientUpdate, [FromServices] IClientService _clientService)
    {
        try
        {
            var clientResponse = await _clientService.UpdateClientAsync(clientUpdate);
            return new JsonResult(clientResponse);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }
    }

    [HttpDelete]
    [Route("[controller]/Delete/{id}")]
    public async Task<IActionResult> Delete(string id, [FromServices] IClientService _clientService)
    {
        try
        {
            await _clientService.DeleteClientAsync(id);
            return Ok();
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }
    }
}
