using Dapr.Client;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Publisher.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PublishController(
    DaprClient daprClient,
    ILogger<PublishController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Publish([FromBody] Hero hero)
    {
        await daprClient.PublishEventAsync(
            "pubsub",
            "hero",
            hero);

        logger.LogInformation("Publishing: {Hero}", hero);

        return Ok();
    }
}
