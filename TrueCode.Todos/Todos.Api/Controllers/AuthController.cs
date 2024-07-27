using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Todos.DataAccess.Identity;
using TrueCode.Todos.Auth;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AuthController(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager, 
        JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings;
    }
    
    //todo: returns 204 in swagger-ui instead of 201
    /// <summary>
    /// Simple one-step registration
    /// </summary>
    /// <param name="request"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CustomRegisterRequest request, [FromServices] IValidator<CustomRegisterRequest> validator)
    {
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            return BadRequest(result.ToDictionary());
        
        var user = new AppUser
        {
            Email = request.Email,
            NormalizedEmail =  _userManager.KeyNormalizer.NormalizeEmail(request.Email),
            UserName = request.Email,
            NormalizedUserName = _userManager.KeyNormalizer.NormalizeName(request.Email),
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
        };
        
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
        var creationResult = await _userManager.CreateAsync(user);
        if (!creationResult.Succeeded)
            return BadRequest($"Failed creation of the new user:\n{creationResult.Errors}");

        var roles = new[] { "User" };
        
        var roleAddResult = await _userManager.AddToRolesAsync(user, roles);
        if (!roleAddResult.Succeeded)
            return BadRequest($"Failed roles assigning to the new user:\n{roleAddResult.Errors}");
        
        return Created();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest userLogin)
    { 
        var user = await _userManager.FindByEmailAsync(userLogin.Email);
        if (user is null)
            return BadRequest("Login or password is incorrect");
            
        var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);
        if (!result.Succeeded)
            return BadRequest("Login or password is incorrect");
        
        var userRoles = await _userManager.GetRolesAsync(user);
        
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        foreach (var userRole in userRoles)
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));

        var token = CreateToken(authClaims);
        return Ok(new AuthResponse(token, new UserProfile(user.Id, user.UserName!, user.Email)));
    }
    
    private string CreateToken(List<Claim> userClaims)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.PrivateKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(userClaims),
            Expires = DateTime.UtcNow.Add(_jwtSettings.AccessTokenExpiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer, // Add this line
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }
}