using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManagementService.Infrastructure.Identity;

namespace OrderManagementService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser>
        _userManager;

    private readonly SignInManager<ApplicationUser>
        _signInManager;

    private readonly JwtTokenService
        _jwtTokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var user =
            await _userManager
                .FindByNameAsync(request.Username);

        if (user is null)
        {
            return Unauthorized();
        }

        var result =
            await _signInManager
                .CheckPasswordSignInAsync(
                    user,
                    request.Password,
                    lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var token =
            await _jwtTokenService
                .CreateTokenAsync(user.Id);

        return Ok(new
        {
            accessToken = token
        });
    }
}

public sealed record LoginRequest(
    string Username,
    string Password);