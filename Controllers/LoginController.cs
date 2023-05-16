using Microsoft.AspNetCore.Mvc;
using rep;
using Models;

namespace Login.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    Repository repo;
    private IConfiguration config;
    public LoginController(IConfiguration _config)
    {
        config = _config;
        repo = new Repository(config);
    }
    [HttpPost]
    public IActionResult Login([FromBody] UserLogin user){
        if(repo.Authenticate(user)){
            return Ok(repo.GenerateToken(user));
        }
        return Unauthorized("user not authorised");
    }
}