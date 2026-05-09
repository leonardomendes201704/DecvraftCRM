using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Platform.Application.Abstractions;
using Platform.Application.Auth;
using Platform.Domain.Catalog;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Auth;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly AppDbContext _dbContext;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public JwtTokenService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LoginResponse> CreateLoginResponseAsync(CurrentUserResponse user, CancellationToken cancellationToken = default)
    {
        var settings = await LoadSettingsAsync(cancellationToken);
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(settings.AccessTokenMinutes);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(KnownAuthClaimTypes.TenantId, user.TenantId.ToString()),
            new(KnownAuthClaimTypes.TenantSlug, user.TenantSlug)
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.AddRange(user.Permissions.Select(permission => new Claim(KnownAuthClaimTypes.Permission, permission)));

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: signingCredentials);

        return new LoginResponse(_tokenHandler.WriteToken(token), expiresAt, user);
    }

    public async Task<ClaimsPrincipal?> ValidateAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return null;
        }

        var settings = await LoadSettingsAsync(cancellationToken);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,
            ValidateAudience = true,
            ValidAudience = settings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        try
        {
            return _tokenHandler.ValidateToken(accessToken, validationParameters, out _);
        }
        catch (SecurityTokenException)
        {
            return null;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    private async Task<JwtSettings> LoadSettingsAsync(CancellationToken cancellationToken)
    {
        var keys = new[]
        {
            KnownSystemConfigurationKeys.JwtSecret,
            KnownSystemConfigurationKeys.JwtIssuer,
            KnownSystemConfigurationKeys.JwtAudience,
            KnownSystemConfigurationKeys.JwtAccessTokenMinutes
        };

        var values = await _dbContext.SystemConfigurations
            .Where(configuration => keys.Contains(configuration.Key))
            .ToDictionaryAsync(configuration => configuration.Key, configuration => configuration.Value, cancellationToken);

        var changed = false;

        changed |= EnsureSetting(
            values,
            KnownSystemConfigurationKeys.JwtSecret,
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(JwtConfigurationDefaults.SecretSizeInBytes)),
            isSecret: true);
        changed |= EnsureSetting(
            values,
            KnownSystemConfigurationKeys.JwtIssuer,
            JwtConfigurationDefaults.Issuer,
            isSecret: false);
        changed |= EnsureSetting(
            values,
            KnownSystemConfigurationKeys.JwtAudience,
            JwtConfigurationDefaults.Audience,
            isSecret: false);
        changed |= EnsureSetting(
            values,
            KnownSystemConfigurationKeys.JwtAccessTokenMinutes,
            JwtConfigurationDefaults.AccessTokenMinutes.ToString(CultureInfo.InvariantCulture),
            isSecret: false);

        if (changed)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var secret = values[KnownSystemConfigurationKeys.JwtSecret];
        var issuer = values[KnownSystemConfigurationKeys.JwtIssuer];
        var audience = values[KnownSystemConfigurationKeys.JwtAudience];
        var accessTokenMinutesValue = values[KnownSystemConfigurationKeys.JwtAccessTokenMinutes];

        if (!int.TryParse(accessTokenMinutesValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var accessTokenMinutes))
        {
            accessTokenMinutes = JwtConfigurationDefaults.AccessTokenMinutes;
        }

        return new JwtSettings(secret, issuer, audience, accessTokenMinutes);
    }

    private bool EnsureSetting(
        IDictionary<string, string> values,
        string key,
        string value,
        bool isSecret)
    {
        if (values.TryGetValue(key, out var existingValue) && !string.IsNullOrWhiteSpace(existingValue))
        {
            return false;
        }

        values[key] = value;
        _dbContext.SystemConfigurations.Add(SystemConfiguration.Create(key, value, isSecret, DateTimeOffset.UtcNow));

        return true;
    }

    private sealed record JwtSettings(string Secret, string Issuer, string Audience, int AccessTokenMinutes);
}
