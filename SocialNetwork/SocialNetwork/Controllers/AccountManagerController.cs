using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Data.Models;
using SocialNetwork.Models;
using SocialNetwork.ViewModels.Account;
using System.Reflection.Metadata.Ecma335;

namespace SocialNetwork.Controllers
{
    public class AccountManagerController : Controller
	{
		public IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
		}
		[Route("Login")]
		[HttpGet]
		public IActionResult Login()
		{
			return View("Home/Login");
		}
		[HttpGet]
		public IActionResult Login(string returnUrl=null)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}
		[Route("Login")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = _mapper.Map<User>(model);

				var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);

				if (result.Succeeded)
				{
					if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
					{
						return Redirect(model.ReturnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "Неправильный логин и (или) пароль");
				}
			}
			return View("Views/Home/Index.cshtml");
		}

		[Route("Logout")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		[Route("MyPage")]
		[Authorize]
		[HttpGet]
		public IActionResult MyPage()
		{
			var user = User;

			var result=_userManager.GetUserAsync(user);

			return View("User", new UserViewModel(result.Result));
		}
	}
}



