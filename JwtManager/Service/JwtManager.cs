namespace EndPointProject.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
public class JwtManager
{
    private const string Secret = "B12AJ293CRajd129das";
    private readonly IConfiguration _configuration;

    public JwtManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(string username, string hashedPassword)
    {
        var claims = new List<Claim>{
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("Password", hashedPassword)
        };


        var token = new JwtSecurityToken(
            issuer: _configuration["Authentication:Authority"],
            audience: _configuration["Authentication:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(Secret)),
                    SecurityAlgorithms.HmacSha256
                )
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public string GenerateTokenjwk(string username, string hashedPassword)
    {
        var pemFile = "private_key.pem";
        var pem = File.ReadAllText(pemFile);
        var rsa = new RSACryptoServiceProvider();
        rsa.ImportFromEncryptedPem(pem, _configuration["Key"]);
        var claims = new[] {
            new Claim(ClaimTypes.Name, username),
            new Claim("password", hashedPassword)
            };
        var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Authentication:Authority"],
            audience: _configuration["Authentication:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );
        var jwtHandler = new JwtSecurityTokenHandler();
        var signedJwt = jwtHandler.WriteToken(token);
        return signedJwt;
    }

    public string VerifyTokenJwk(string jwt)
    {
        var message = String.Empty;
        var pemFile = "public_key.pem";
        var pem = File.ReadAllText(pemFile);
        var rsa = new RSACryptoServiceProvider();
        rsa.ImportFromPem(pem);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Authentication:Authority"],
            ValidAudience = _configuration["Authentication:Audience"],
            IssuerSigningKey = new RsaSecurityKey(rsa)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;
        try
        {
            tokenHandler.ValidateToken(jwt, validationParameters, out validatedToken);
            message = "Token is valid";
        }
        catch (SecurityTokenException ex)
        {
            Console.WriteLine("Token is not valid: " + ex.Message);
            message = "Token is not valid";
        }
        return message;
    }

    public string Jwks()
    {
        var pemFile = "public_key.pem";
        var pem = File.ReadAllText(pemFile);
        var rsa = new RSACryptoServiceProvider();
        rsa.ImportFromPem(pem);
        var parameters = rsa.ExportParameters(false);
        var jwk = new JsonWebKey
        {
            Kty = "RSA",
            Use = "sig",
            Kid = "1",
            Alg = "RS256",
            E = Base64UrlEncoder.Encode(parameters.Exponent),
            N = Base64UrlEncoder.Encode(parameters.Modulus)
        };
        List<JsonWebKey> jwks = new List<JsonWebKey>();

        jwks.Add(jwk);

        return JsonConvert.SerializeObject(new { Keys = jwks });
    }



}