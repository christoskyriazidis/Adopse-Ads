using IdentityModel;
using IdentityServer.Models;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name="credentials",
                    UserClaims =
                    {
                        "username",ClaimTypes.Email,"Mobile","role",ClaimTypes.Role,ClaimTypes.NameIdentifier,"lastName",ClaimTypes.StreetAddress,"name","lastName"
                    }
                }
                //new IdentityResources.Email
            };

        //gia api identify
        public static IEnumerable<ApiResource> GetApis() =>
            //kai kala kanoume register ena api pou exei onoma apione google
            //edw vazoume claims gia to access token..
            new List<ApiResource> {
                new ApiResource("ResourceServer","myClaims", new string[]{"username",ClaimTypes.Email,"Mobile","role",ClaimTypes.Role,ClaimTypes.NameIdentifier,"lastName",ClaimTypes.StreetAddress,"name","lastName"}) ,
                new ApiResource("ApiTwoTest") ,
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId="client_id_js",

                    AllowedGrantTypes=GrantTypes.Implicit,

                    RedirectUris ={"https://localhost:44366/home/signin"},
                    PostLogoutRedirectUris ={"https://localhost:44366/Home/Index"},
                    AllowedCorsOrigins={"https://localhost:44364"},
                    AllowedScopes =
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        "ResourceServer",
                        "credentials"
                    },

                    AccessTokenLifetime=1,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                }
            };
    }
}
