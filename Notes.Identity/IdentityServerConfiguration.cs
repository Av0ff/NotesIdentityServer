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
				new ApiResource("NotesWebApi", "Web API")
				{
					Scopes = {"NotesWebApi"}
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
					RedirectUris =
					{
						"https://localhost:5001/signin-oidc"
					},
					AllowedCorsOrigins =
					{
						"https://localhost:5001"
					},
					PostLogoutRedirectUris =
					{
						"https://localhost:5001/singout-oidc"
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
