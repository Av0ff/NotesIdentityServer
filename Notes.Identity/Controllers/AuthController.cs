using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;
using System.Threading.Tasks;

namespace Notes.Identity.Controllers
{
	public class AuthController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IIdentityServerInteractionService _interaction;

		public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IIdentityServerInteractionService interaction)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_interaction = interaction;
		}

		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			var model = new LoginViewModel
			{
				ReturnUrl = returnUrl,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByNameAsync(model.Name);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "User not found");
				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, false, false);

			if (result.Succeeded)
			{
				if (model.ReturnUrl != null)
					return Redirect(model.ReturnUrl);
				else
					return View(model);
			}

			ModelState.AddModelError(string.Empty, "Login Error");
			return View(model);
		}

		[HttpGet]
		public IActionResult Register(string returnUrl)
		{
			var model = new RegisterViewModel
			{
				ReturnUrl = returnUrl,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = new AppUser
			{
				UserName = model.UserName
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, false);
				if (model.ReturnUrl != null)
					return Redirect(model.ReturnUrl);
				else
					return View(model);
			}
			ModelState.AddModelError(string.Empty, "Error occurred");
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Logout(string logoutId)
		{
			await _signInManager.SignOutAsync();
			var logoutRequest = await _interaction.GetLogoutContextAsync(logoutId);
			if (logoutRequest.PostLogoutRedirectUri != null)
				return Redirect(logoutRequest.PostLogoutRedirectUri);
			else
				return RedirectToAction("Login");
		}
	}
}
