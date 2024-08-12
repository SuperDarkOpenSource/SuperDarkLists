namespace SuperdarkLists.Api.v1.Controllers;

[ApiController]
[Route("/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
}