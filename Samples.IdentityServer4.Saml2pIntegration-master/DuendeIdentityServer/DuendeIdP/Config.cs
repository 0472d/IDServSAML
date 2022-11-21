using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using Rsk.Saml;
using Rsk.Saml.Models;
using ServiceProvider = Rsk.Saml.Models.ServiceProvider;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityModel;

namespace DuendeIdP;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("custom", new List<string>{ ClaimTypes.Name })
        };
    }

    public static IEnumerable<ApiResource> GetApis()
    {
        return new ApiResource[]
        {
            new ApiResource("api1", "My API #1")
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new[]
        {
            new Client
            {
                ClientId = "EntityID",
                ClientName = "RSK SAML2P Test Client - Multiple SP",
                ProtocolType = IdentityServerConstants.ProtocolTypes.Saml2p,
                AllowedScopes = {"openid", "profile", "custom"}
            }
        };
    }

    public static IEnumerable<ServiceProvider> GetServiceProviders()
    {
        return new[]
        {
                new Rsk.Saml.Models.ServiceProvider
                {
                    EntityId = "EntityID",
                    SigningCertificates = { new X509Certificate2("saml.cer") },
                    AssertionConsumerServices = { new Service(SamlConstants.BindingTypes.HttpPost, "SamlUrls") },
                    SingleLogoutServices = { new Service(SamlConstants.BindingTypes.HttpPost, "SamlUrls") },
                    ClaimsMapping = new Dictionary<string, string>()
                    {
                        {ClaimTypes.Name, ClaimTypes.Name },
                        {JwtClaimTypes.GivenName, ClaimTypes.GivenName },
                        {JwtClaimTypes.FamilyName, ClaimTypes.Surname },
                    }
                }
        };

    }
}
