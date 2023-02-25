using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notes.Identity.Data;
using Notes.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Identity
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var connectionString = Configuration.GetConnectionString("MSSQL");
			services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

			services.AddIdentity<AppUser, IdentityRole>(config =>
			{
				config.Password.RequiredLength = 4;
				config.Password.RequireNonAlphanumeric = false;
				config.Password.RequireDigit = false;
				config.Password.RequireUppercase = false;
			})
				.AddEntityFrameworkStores<AuthDbContext>()
				.AddDefaultTokenProviders();

			services.AddIdentityServer()
				.AddAspNetIdentity<AppUser>()
				.AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
				.AddInMemoryIdentityResources(IdentityServerConfiguration.IdentityResources)
				.AddInMemoryApiScopes(IdentityServerConfiguration.ApiScopes)
				.AddInMemoryClients(IdentityServerConfiguration.Clients)
				.AddDeveloperSigningCredential();

			services.ConfigureApplicationCookie(config =>
			{
				config.Cookie.Name = "NotesIdentityCookie";
				config.LoginPath = "/Auth/Login";
				config.LogoutPath = "/Auth/Logout";
			});

			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseRouting();
			app.UseIdentityServer();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
