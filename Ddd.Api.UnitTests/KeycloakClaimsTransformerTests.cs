using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Ddd.App.Api.UnitTests
{
    public class KeycloakClaimsTransformerTests
    {
        [Fact]
        public void ValidateTransformer()
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentities(new List<ClaimsIdentity>
            {
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim("realm_access", "{\"roles\":[\"realm_role1\",\"realm_role2\"]}"),
                    new Claim("resource_access", "{\"client1\":{\"roles\":[\"client1_role1\",\"client1_role2\"]},\"client2\":{\"roles\":[\"client2_role1\"]}}")
                })
            });
            ClaimsPrincipal transformedClaimsPrincipal = KeycloakClaimsTransformer.Transform(claimsPrincipal);
            Assert.Contains(transformedClaimsPrincipal.Claims, c => c.Type == ClaimTypes.Role && c.Value == "realm_role1");
            Assert.Contains(transformedClaimsPrincipal.Claims, c => c.Type == ClaimTypes.Role && c.Value == "realm_role2");
            Assert.Contains(transformedClaimsPrincipal.Claims, c => c.Type == ClaimTypes.Role && c.Value == "client1_role1");
            Assert.Contains(transformedClaimsPrincipal.Claims, c => c.Type == ClaimTypes.Role && c.Value == "client1_role2");
        }
    }
}
