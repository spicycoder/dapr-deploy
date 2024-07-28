using Dapr.Client;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Publisher.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatesController(
    DaprClient daprClient,
    ILogger<PublishController> logger) : ControllerBase
{
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] Hero hero)
    {
        logger.LogInformation("Saving: {Hero}", hero);

        await daprClient.SaveStateAsync(
            "statestore",
            hero.Name,
            hero);

        return Ok();
    }

    [HttpGet("read")]
    public async Task<ActionResult<Hero>> Read([FromQuery] string name)
    {
        var hero = await daprClient.GetStateAsync<Hero>(
            "statestore",
            name);

        logger.LogInformation("Reading hero - {Name}: {Identity}", name, hero.Identity);

        return Ok(hero);
    }
}
