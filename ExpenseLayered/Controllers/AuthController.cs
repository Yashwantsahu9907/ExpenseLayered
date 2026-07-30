using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseLayeredApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromBody]LoginDto loginDto)
    {
        var result = await _authService.LoginUser(loginDto);
        return StatusCode(result.StatusCode, result);
    }
}
