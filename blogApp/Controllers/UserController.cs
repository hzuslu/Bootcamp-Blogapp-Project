using System.Security.Claims;
using blogApp.Data;
using blogApp.Datacontext;
using blogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
	public class UserController : Controller
	{

		private readonly DataContext _context;

		public UserController(DataContext dataContext)
		{
			_context = dataContext;
		}


		[HttpGet]
		public IActionResult Login()
		{
			if (User.Identity!.IsAuthenticated)
			{
				return RedirectToAction("Index", "Post");
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				var isUser = _context.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

				if (isUser != null)
				{
					var userClaims = new List<Claim>
					{
						new (ClaimTypes.NameIdentifier, isUser.UserId.ToString()),
						new (ClaimTypes.Name, isUser.UserName ?? ""),
						new (ClaimTypes.GivenName, isUser.UserSurname ?? ""),
						new (ClaimTypes.UserData, isUser.Image ?? "")
					};

					if (isUser.Email == "a@a")
					{
						userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
					}

					var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

					var authProperties = new AuthenticationProperties
					{
						IsPersistent = model.Remember
					};

					await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity),
						authProperties);

					return RedirectToAction("Index", "Post");
				}
				else
				{
					ModelState.AddModelError("", "Invalid email or password");
				}
			}

			return View(model);
		}



		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Post");
		}

		[HttpGet]
		public IActionResult Register()
		{

			if (User.Identity!.IsAuthenticated)
			{
				return RedirectToAction("Index", "Post");
			}
			else
				return View();

		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel registerModel)
		{
			if (ModelState.IsValid)
			{
				var existingEmailUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerModel.Email);
				var existingUserNameUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == registerModel.UserName);
				var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerModel.Email && u.Password == registerModel.Password);

				if (existingUser != null)
				{
					ModelState.AddModelError("", "Bu e-posta ve şifre kombinasyonu ile bir kullanıcı zaten var.");
					return View(registerModel);
				}

				if (existingUserNameUser != null)
				{
					ModelState.AddModelError("", "Bu kullanıcı adı zaten alınmış.");
					return View(registerModel);
				}

				if (existingEmailUser != null)
				{
					ModelState.AddModelError("", "Bu e-posta adresi zaten kullanılmış.");
					return View(registerModel);
				}


				var newUser = new User
				{
					UserName = registerModel.UserName,
					UserSurname = registerModel.UserSurname,
					Email = registerModel.Email,
					Password = registerModel.Password,
					Phone = registerModel.Phone,
					RegisterDate = DateTime.Now
				};

				_context.Users.Add(newUser);
				await _context.SaveChangesAsync();

				return RedirectToAction("Login");
			}

			return View(registerModel);
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Profile(string? url)
		{
			if (url == null)
			{
				return NotFound();
			}

			var user = await _context.Users
										.Include(u => u.Post)
										.Include(t => t.Likes)
										.Include(u => u.Comments)
										.FirstOrDefaultAsync(u => u.UserName == url);



			if (user == null)
			{
				return NotFound();
			}


			if (user.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value))
			{

				return View(user);
			}

			return RedirectToAction("MyProfile");
		}


		[HttpGet]
		[Authorize]
		public IActionResult MyProfile()
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
			var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

			if (user == null)
				return NotFound();

			var userEditModel = new UserEditModel
			{
				UserFirstName = user.UserFirstName,
				UserSurname = user.UserSurname,
				Phone = user.Phone,
				Image = user.Image,
				Email = user.Email,
				Address = user.Address,
				Country = user.Country,
				City = user.City,
				ZipCode = user.ZipCode,
				Education = user.Education
			};

			return View(userEditModel);


		}


		[HttpPost]
		[Authorize]
		public async Task<IActionResult> MyProfile(UserEditModel userEditModel, IFormFile? imageFile)
		{
			List<string> allowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".psd", ".pdf", ".eps", ".ai", ".indd", ".raw" };
			var check = User.FindFirst(ClaimTypes.Role) != null;
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

			if (userEditModel == null)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return View(userEditModel);
			}

			if (imageFile != null)
			{
				var extension = Path.GetExtension(imageFile.FileName);
				if (!allowedExtensions.Contains(extension))
				{
					ViewBag.ImageFileError = "This extension is not allowed";
					return View(userEditModel);
				}

				var randomFileName = $"{Guid.NewGuid()}{extension}";
				var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/pp", randomFileName);

				using (var stream = new FileStream(imagePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				user!.Image = randomFileName;
				userEditModel.Image = randomFileName;
			}



			if (user == null)
				return NotFound();

			user.UserFirstName = userEditModel.UserFirstName;
			user.UserSurname = userEditModel.UserSurname;
			user.Phone = userEditModel.Phone;
			user.Email = userEditModel.Email;
			user.Address = userEditModel.Address;
			user.Country = userEditModel.Country;
			user.City = userEditModel.City;
			user.ZipCode = userEditModel.ZipCode;
			user.Education = userEditModel.Education;

			var userClaims = new List<Claim>
								{
									new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
									new(ClaimTypes.Name, user.UserName ?? ""),
									new(ClaimTypes.GivenName, user.UserSurname ?? ""),
									new(ClaimTypes.UserData, user.Image ?? "")
								};

			if (check)
				userClaims.Add(new Claim(ClaimTypes.Role, "admin"));

			var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

			_context.Update(user);
			await _context.SaveChangesAsync();

			return View(userEditModel);
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> UserList()
		{

			var role = User.FindFirst(ClaimTypes.Role);

			if (role != null)
			{
				var users = await _context.Users.Include(u => u.Post).Include(u => u.Comments).ToListAsync();
				return View(users);
			}

			return RedirectToAction("Index", "Post");
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{

			if (id == null)
				return NotFound();

			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

			if (user == null)
				return NotFound();

			_context.Remove(user);
			await _context.SaveChangesAsync();
			return RedirectToAction("UserList");

		}

	}
}
