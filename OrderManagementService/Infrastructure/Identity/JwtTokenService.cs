using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderManagementService.Infrastructure.Persistance;

namespace OrderManagementService.Infrastructure.Identity;

public sealed class JwtTokenService
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;

    public JwtTokenService(
        ApplicationDbContext db,
        IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<string> CreateTokenAsync(
        Guid userId)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
        {
            throw new InvalidOperationException(
                "User not found.");
        }

        var roleNames =
            await (
                from userRole in _db.UserRoles
                join role in _db.Roles
                    on userRole.RoleId equals role.Id
                where userRole.UserId == userId
                select role.Name!
            )
            .Distinct()
            .ToListAsync();

        var permissions =
            await (
                from userRole in _db.UserRoles

                join rolePermission in _db.RolePermissions
                    on userRole.RoleId
                    equals rolePermission.RoleId

                where userRole.UserId == userId

                select rolePermission.Permission
            )
            .Distinct()
            .ToListAsync();

        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new(
                ClaimTypes.Name,
                user.UserName ?? string.Empty)
        };
        
        foreach (var role in roleNames)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    role));
        }

        foreach (var permission in permissions)
        {
            claims.Add(
                new Claim(
                    PermissionClaimTypes.Permission,
                    permission.ToString()));
        }
        
        var key =
            _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "Jwt:Key is not configured.");
        }

        var issuer =
            _configuration["Jwt:Issuer"];

        var audience =
            _configuration["Jwt:Audience"];

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);
        
        var token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}