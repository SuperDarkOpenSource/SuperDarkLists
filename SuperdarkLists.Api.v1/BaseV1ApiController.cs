using Microsoft.AspNetCore.Http.Extensions;

namespace SuperdarkLists.Api.v1;

[Route("/api/v1/[controller]")]
[ApiController]
public class BaseV1ApiController : ControllerBase
{
    protected ActionResult Created<T>(T obj)
    {
        return Created(new Uri(Request.GetDisplayUrl()), obj);
    }
}