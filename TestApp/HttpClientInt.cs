namespace TestApp.Controllers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

public class HttpClientInt
{
    static readonly string url
        = "https://localhost:7251/api/dumb/validrequest";
    static readonly HttpClient client = new HttpClient();
    public async Task<bool> HttpClientChecker(string token)
    {
        using (var httpClient = new HttpClient())
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.SendAsync(req);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
        }
        return true;
    }

    public async Task<string> GetPublicKeys(string jwt)
    {

        using (var httpClient = new HttpClient())
        {
            var message = String.Empty;
            string jwksJson = String.Empty;
            var req = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7251/api/dumb/jwks");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var response = await httpClient.SendAsync(req);
            if (response.IsSuccessStatusCode)
            {
                jwksJson = response.Content.ReadAsStringAsync().Result;
            }
            List<JsonWebKey> listJwk = new List<JsonWebKey>();
            var jwks = new Jwks();

            var jsonWebKeySet = JsonConvert.DeserializeObject<Jwks>(jwksJson);
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = "https://localhost:7251",
                ValidAudience = "https://localhost:3000",
                IssuerSigningKeys = jsonWebKeySet.Keys
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(jwt, validationParameters, out validatedToken);
                if (validatedToken != null)
                {
                    message = "Token is valid";
                }
            }
            catch (SecurityTokenException ex)
            {
                message = "Token is not valid";
                Console.WriteLine("Token is not valid: " + ex.Message);
            }
            return message;
        }

    }
}

public class Jwks
{
    public List<JsonWebKey> Keys { get; set; }
}