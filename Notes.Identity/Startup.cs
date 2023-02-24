using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Identity
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentityServer()
				.AddInMemoryApiResources(Configuration.ApiResources)
				.AddInMemoryIdentityResources(Configuration.IdentityResources)
				.AddInMemoryApiScopes(Configuration.ApiScopes)
				.AddInMemoryClients(Configuration.Clients)
				.AddDeveloperSigningCredential();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseIdentityServer();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Hello World!");
				});
			});
		}
	}
}
