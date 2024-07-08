using Microsoft.AspNetCore.Mvc;
using BancoKRT.API.Domain.Services.Interfaces;
using BancoKRT.API.Domain.Models;
using BancoKRT.API.Middlewares;
using System.Net;

namespace BancoKRT.API.Controllers;

[ApiController]
[Route("/")]
public class PIXController : ControllerBase
{
    [HttpGet]
    [Route("[controller]/Details/")]
    public async Task<IActionResult> Details([FromServices] IPIXService _pixService)
    {
        try
        {
            var pixResponse = await _pixService.GetAllPIXsAsync();
            return new JsonResult(pixResponse);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }
    }

    [HttpGet]
    [Route("[controller]/Detail/{id}")]
    public async Task<IActionResult> Detail(string id, [FromServices] IPIXService _pixService)
    {
        try
        {
            var pixResponse = await _pixService.GetPIXByIdAsync(id);
            return new JsonResult(pixResponse);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }
    }

    [HttpPost]
    [Route("[controller]/Create")]
    public async Task<IActionResult> Create([FromBody] PIX pix, [FromServices] IPIXService _pixService)
    {
        try
        {
            var pixResponse = await _pixService.AddPIXAsync(pix);
            return new JsonResult(pixResponse);
        }
        catch (HttpException ex)
        {
            throw (ex is not null ? ex : new HttpException(HttpStatusCode.InternalServerError));
        }
    }
}
