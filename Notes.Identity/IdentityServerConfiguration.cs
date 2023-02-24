using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections;
using System.Collections.Generic;

namespace Notes.Identity
{
	public static class IdentityServerConfiguration
	{
		public static IEnumerable<ApiScope> ApiScopes =>
			new List<ApiScope>
			{
				new ApiScope("NotesWebApi", "Web API")
			};

		public static IEnumerable<ApiResource> ApiResources =>
			new List<ApiResource>
			{
				new ApiResource("NotesWebApi", "Web API", new [] { JwtClaimTypes.Name })
				{
					Scopes = {"NotesWebApi"},
				}
			};

		public static IEnumerable<IdentityResource> IdentityResources =>
			new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile()
			};

		public static IEnumerable<Client> Clients =>
			new List<Client>
			{
				new Client
				{
					ClientId = "notes-api",
					ClientName = "Notes Web API",
					AllowedGrantTypes = GrantTypes.Code,
					RequireClientSecret = false,
					RequirePkce = true,
					RedirectUris =
					{
						"http://.../signin-oidc"
					},
					AllowedCorsOrigins =
					{
						"http://..."
					},
					PostLogoutRedirectUris =
					{
						"http://.../singout-oidc"
					},
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"NotesWebApi"
					},
					AllowAccessTokensViaBrowser = true
				}
			};
	}
}
