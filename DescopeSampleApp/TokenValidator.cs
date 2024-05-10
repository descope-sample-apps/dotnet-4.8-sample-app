using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Jose;
using Newtonsoft.Json.Linq;
using System.Net.Http;

public class Config
{
    public static string DescopeProjectId => Environment.GetEnvironmentVariable("DESCOPE_PROJECT_ID") ?? "P2dI0leWLEC45BDmfxeOCSSOWiCt";
}
public class TokenValidator
{
    private readonly HttpClient _httpClient;
    private readonly string _projectId;

    public TokenValidator(string projectId)
    {
        _httpClient = new HttpClient();
        _projectId = Config.DescopeProjectId ?? projectId;
    }

    public async Task<string> ValidateSession(string sessionToken)
    {
        try
        {
            var jwks = await GetPublicKeyAsync(_projectId);

            foreach (var jwk in jwks.Keys)
            {
                try
                {
                    Jwk pubKey = jwk;

                    var payload = JWT.Decode(sessionToken, jwk);
                    return payload;
                }
                catch (Exception)
                {
                    // If decoding fails with this key, try the next one
                }
            }

            throw new Exception("Failed to validate token with any JWK.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing and verifying token: {ex.Message}");
            throw;
        }
    }

    public bool VerifyTokenExpiration(string sessionToken)
    {
        try
        {
            var payload = JWT.Payload<JObject>(sessionToken);
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds((long)payload["exp"]);
            return expirationTime > DateTimeOffset.UtcNow;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error verifying token expiration: {ex.Message}");
            return false;
        }
    }

    private async Task<JwkSet> GetPublicKeyAsync(string projectId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var url = $"https://api.descope.com/{projectId}/.well-known/jwks.json";
            string keys = await client.GetStringAsync(url);
            JwkSet jwks = JwkSet.FromJson(keys, JWT.DefaultSettings.JsonMapper);
            return jwks;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching public key: {ex.Message}");
            throw;
        }
    }
}