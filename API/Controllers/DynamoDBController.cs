using Microsoft.AspNetCore.Mvc;
using BancoKRT.API.Infrastructure.Services;

namespace BancoKRT.API.Controllers;

[ApiController]
[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class DynamoDBController : Controller
{
    [HttpPost]
    [Route("[controller]/CreateTables")]
    public async Task<IActionResult> CreateTables(DynamoDBTableService dynamoDBTableService)
    => Ok(await dynamoDBTableService.CreateTableAsync());
}
