using System.Security.Claims;
using System.Text.Json;

public static class KeycloakClaimsTransformer
{
    private const string RealmAccessClaim = "realm_access";
    private const string ResourceAccessClaim = "resource_access";
    private const string RolesClaim = "roles";

    public static ClaimsPrincipal Transform(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;
        if (identity is null) return principal;

        // Map realm-level roles
        var realmAccessClaim = identity.FindFirst(RealmAccessClaim);
        if (realmAccessClaim is not null)
        {
            MapRoles(identity, realmAccessClaim.Value);
        }

        // Map client-level roles (resource_access is a JSON object keyed by client_id)
        var resourceAccessClaim = identity.FindFirst(ResourceAccessClaim);
        if (resourceAccessClaim is not null)
        {
            using var doc = JsonDocument.Parse(resourceAccessClaim.Value);
            foreach (var client in doc.RootElement.EnumerateObject())
            {
                if (client.Value.TryGetProperty(RolesClaim, out var roles))
                {
                    foreach (var role in roles.EnumerateArray())
                    {
                        var roleValue = role.GetString();
                        if (!string.IsNullOrWhiteSpace(roleValue))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                        }
                    }
                }
            }
        }

        return principal;
    }

    private static void MapRoles(ClaimsIdentity identity, string json)
    {
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty(RolesClaim, out var roles)) return;

        foreach (var role in roles.EnumerateArray())
        {
            var roleValue = role.GetString();
            if (!string.IsNullOrWhiteSpace(roleValue))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
            }
        }
    }
}