using Dapr;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Subscriber.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscribeController(ILogger<SubscribeController> logger) : ControllerBase
{
    [HttpPost]
    [Topic("pubsub", "hero")]
    public async Task<IActionResult> Subscribe([FromBody] Hero hero)
    {
        logger.LogInformation("Received: {Hero}", hero);
        return Ok();
    }
}
