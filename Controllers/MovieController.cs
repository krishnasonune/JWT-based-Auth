using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rep;

namespace Authorization.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    Repository repo;
    private IConfiguration _config;
    public MovieController(IConfiguration config)
    {
        _config = config;
        repo = new Repository(_config);
    }
    [HttpGet]
    public IActionResult GetMovies(){
        return Ok(repo.getMovies());
    }
}