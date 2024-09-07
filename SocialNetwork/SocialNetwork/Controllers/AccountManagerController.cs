using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using SocialNetwork.Data.Models;
using SocialNetwork.Data.Repository;
using SocialNetwork.Data.UOW;
using SocialNetwork.Models;
using SocialNetwork.ViewModels;
using SocialNetwork.ViewModels.Account;
using SocialNetwork.ViewModels.Extensions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialNetwork.Controllers
{
	public class AccountManagerController : Controller
	{
		public IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IUnitOfWork _unitOfWork;
		public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		[Route("Login")]
		[HttpGet]
		public IActionResult Login()
		{
			return View("Shared/Login");
		}
		[HttpGet]
		public IActionResult Login(string returnUrl = null)
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
					return RedirectToAction("MyPage", "AccountManager");
				}
				else
				{
					ModelState.AddModelError("", "Неправильный логин и (или) пароль");
				}
			}
			foreach (var item in ModelState)
			{
				foreach (var error in item.Value.Errors)
				{
					Console.WriteLine(error.ErrorMessage);
				}
			}
			return RedirectToAction("Index", "Home");


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
		public async Task<IActionResult> MyPage()
		{
			var user = User;

			var result = await _userManager.GetUserAsync(user);

			var model = new UserViewModel(result);

			var userFriends = await GetAllFriend(model.User);

			model.Friends = await GetAllFriend(model.User);

			return View("User", model);
		}
		[Route("Edit")]
		[Authorize]
		[HttpGet]
		public IActionResult Edit()
		{
			var user = User;

			var result = _userManager.GetUserAsync(user);

			var editmodel = _mapper.Map<UserEditViewModel>(result.Result);

			return View("Edit", editmodel);
		}
		[Authorize]
		[Route("Update")]
		[HttpPost]
		public async Task<IActionResult> Update(UserEditViewModel? model)
		{
			Console.WriteLine(model.FirstName);
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.UserId);
				user.Convert(model);
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction("MyPage", "AccountManager");
				}
				else
				{
					return RedirectToAction("Edit", "AccountManager");
				}
			}
			else
			{
				ModelState.AddModelError("", "Некорректные значения");
				string errorMessages = "";
				foreach (var item in ModelState)
				{
					if (item.Value.ValidationState == ModelValidationState.Invalid)
					{
						errorMessages = $"{errorMessages}\nОшибки для свойства {item.Key}:\n";
						foreach (var error in item.Value.Errors)
						{
							errorMessages = $"{errorMessages}{error.ErrorMessage}\n";
						}
					}
				}
				Console.WriteLine(errorMessages);
				return View("Edit", model);
			}
		}
		[Route("UserList")]
		[HttpGet]
		public async Task<IActionResult> UserList(string search)
		{
			var model = await CreateSearch(search);

			return View("UserList", model);
		}
		private async Task<SearchViewModel> CreateSearch(string search)
		{
			var currentUser = User;
			var result = await _userManager.GetUserAsync(currentUser);

			var list = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList();
			var withFriend = await GetAllFriend();

			var data = new List<UserWithFriendExt>();
			list.ForEach(x =>
			{
				var t = _mapper.Map<UserWithFriendExt>(x);
				t.IsFriendWithCurrent = withFriend.Where(y => y.Id == x.Id || x.Id == result.Id).Count() != 0;
				data.Add(t);
			});
			var model = new SearchViewModel()
			{
				UserList = data
			};
			return model;
		}
		private async Task<List<User>> GetAllFriend(User user)
		{
			var repository = _unitOfWork.GetRepository<Friend>() as FriendRepository;

			return repository.GetFriendsByUser(user);
		}
		private async Task<List<User>> GetAllFriend()
		{
			var user = User;

			var result = await _userManager.GetUserAsync(user);

			var repository = _unitOfWork.GetRepository<Friend>() as FriendRepository;

			return repository.GetFriendsByUser(result);
		}
		[Route("AddFriend")]
		[HttpPost]
		public async Task<IActionResult> AddFriend(string id)
		{
			var currentUser = User;

			var result = await _userManager.GetUserAsync(currentUser);

			var friend = await _userManager.FindByIdAsync(id);

			var repository = _unitOfWork.GetRepository<Friend>() as FriendRepository;

			repository.AddFriend(result, friend);

			return RedirectToAction("MyPage", "AccountManager");
		}
		[Route("DeleteFriend")]
		[HttpPost]
		public async Task<IActionResult> DeleteFriend(string id)
		{
			var currentUser = User;

			var result = await _userManager.GetUserAsync(currentUser);

			var friend = await _userManager.FindByIdAsync(id);

			var repository = _unitOfWork.GetRepository<Friend>() as FriendRepository;

			repository.DeleteFriend(result, friend);

			return RedirectToAction("MyPage", "AccountManager");
		}
		[Route("Chat")]
		[HttpPost]
		public async Task<IActionResult> Chat(string id)
		{
			var currentUser = User;
			var result = await _userManager.GetUserAsync(currentUser);
			var friend = await _userManager.FindByIdAsync(id);

			var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;
			var mess = repository.GetMessages(result, friend);
			var model = new ChatViewModel()
			{
				You = result,
				ToWhom = friend,
				History = mess.OrderBy(x => x.Id).ToList(),
			};
			return View("Chat", model);

		}
		[Route("NewMessage")]
		[HttpPost]
		public async Task<IActionResult>NewMessage(string id,ChatViewModel chat)
		{
			var currentUser = User;
			var result = await _userManager.GetUserAsync(currentUser);
			var friend=await _userManager.FindByIdAsync(id);

			var repository=_unitOfWork.GetRepository<Message>() as MessageRepository;

			var item = new Message()
			{
				Sender = result,
				Recipient=friend,
				Text=chat.NewMessage.Text,
			};
			repository.Create(item);

			var mess=repository.GetMessages(result,friend);
			var model = new ChatViewModel()
			{
				You = result,
				ToWhom = friend,
				History = mess.OrderBy(x => x.Id).ToList(),
			};
			return View("Chat", model);
		}
		[Route("NewMessage")]
		[HttpGet]
		public async Task<IActionResult> NewMessage(string id)
		{
			var model = await GenerateChat(id);
			return View("Chat", model);
		}
		private async Task<ChatViewModel>GenerateChat(string id)
		{
			var currentUser = User;
			var result=await _userManager.GetUserAsync(currentUser);
			var friend=await _userManager.FindByIdAsync(id);

			var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;
			var mess=repository.GetMessages(result, friend);
			var model = new ChatViewModel()
			{
				You = result,
				ToWhom = friend,
				History = mess.OrderBy(x => x.Id).ToList(),
			};
			return model;
		}
		[Route("Chat")]
		[HttpGet]
		public async Task<IActionResult> Chat()
		{
			var id=Request.Query["id"];

			var model=await GenerateChat(id);
			return View("Chat",model);
		}
    }

}



