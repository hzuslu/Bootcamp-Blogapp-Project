using System.Security.Claims;
using System.Security.Policy;
using blogApp.Data;
using blogApp.Datacontext;
using blogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
	public class PostController : Controller
	{

		private readonly DataContext _context;

		public PostController(DataContext dataContext)
		{
			_context = dataContext;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string? url, string? searchString)
		{
			var posts = await _context.Posts.Where(p => p.IsActive == true)
											.Include(p => p.User)
											.Include(p => p.Tags)
											.ToListAsync();

			if (!string.IsNullOrEmpty(searchString))
				posts = posts.Where(p => p.Content!.ToLower().Contains(searchString.ToLower())).ToList();

			if (!string.IsNullOrEmpty(url))
			{
				posts = posts.Where(p => p.Tags.Any(k => k.Url == url)).ToList();
			}

			return View(posts);
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Details(string url)
		{
			if (string.IsNullOrEmpty(url))
				return NotFound();

			var model = await _context.Posts
									   .Include(p => p.User)
									   .Include(p => p.Tags)
									   .Include(p => p.Likes)
									   .Include(p => p.Comments)
									   .ThenInclude(p => p.User)
									   .FirstOrDefaultAsync(p => p.Url == url);

			if (model != null)
				return View(model);

			return NotFound();
		}

		[HttpPost]
		[Authorize]
		public async Task<JsonResult> Details(int postId, string content)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var username = User.FindFirstValue(ClaimTypes.Name);
			var image = User.FindFirst(ClaimTypes.UserData)!.Value;

			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userId!));
			var name = user!.UserFirstName;

			var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
			var postUrl = post!.Url;

			var comment = new Comment
			{
				PostId = postId,
				Content = content,
				CommentTime = DateTime.Now,
				UserId = int.Parse(userId!)
			};

			var time = comment.CommentTime;

			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();

			var commentId = comment.CommentId;

			return Json(new
			{
				username,
				content,
				time,
				image,
				name,
				commentId
			});
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> CreatePost()
		{

			var tags = await _context.Tags.ToListAsync();
			ViewBag.Tags = tags;

			return View();

		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreatePost(PostCreateModel postCreateModel, IFormFile imageFile, List<int> selectedTags)
		{

			var tags = await _context.Tags.ToListAsync();
			var selectedTagIds = selectedTags.Select(tagId => tagId).ToList();
			var matchingTags = tags.Where(tag => selectedTagIds.Contains(tag.TagId)).ToList();


			ViewBag.Tags = tags;
			List<string> allowExtensions = new() { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".psd", ".pdf", ".eps", ".ai", ".indd", ".raw" };

			if (imageFile != null)
			{

				var extension = Path.GetExtension(imageFile.FileName);
				if (!allowExtensions.Contains(extension))
				{
					ViewBag.ImageFileError = "This extension is not allowed";
					return View(postCreateModel);

				}

			}

			if (imageFile == null)
			{
				ViewBag.ImageFileError = "Please select a file.";
				return View(postCreateModel);
			}

			if (ModelState.IsValid)
			{
				var extension = Path.GetExtension(imageFile.FileName);
				var randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

				using (var stream = new FileStream(path, FileMode.Create))
				{

					await imageFile.CopyToAsync(stream);
				}

				var post = new Post
				{
					Title = postCreateModel.Title,
					Url = postCreateModel.Url,
					Content = postCreateModel.Content,
					Image = randomFileName,
					PostTime = DateTime.Now,
					UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
					IsActive = false,
					Tags = matchingTags,

				};

				_context.Posts.Add(post);
				await _context.SaveChangesAsync();

				return RedirectToAction("List", "Post");
			}

			return View(postCreateModel);

		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> List()
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
			var posts = await _context.Posts.Include(p => p.User).ToListAsync();

			if (User.FindFirst(ClaimTypes.Role) == null)
				posts = await _context.Posts.Include(p => p.User).Where(p => p.UserId == userId).ToListAsync();



			return View(posts);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Edit(string? url)
		{
			if (string.IsNullOrEmpty(url))
				return NotFound();

			var post = await _context.Posts.FirstOrDefaultAsync(i => i.Url == url);
			PostEditModel postEditModel = new()
			{
				PostId = post!.PostId,
				Title = post.Title,
				Url = post.Url,
				Content = post.Content,
				IsActive = post.IsActive,
				Image = post.Image,
			};

			return View(postEditModel);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Edit(PostEditModel postEditModel, IFormFile? imageFile)
		{
			List<string> allowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".psd", ".pdf", ".eps", ".ai", ".indd", ".raw" };

			if (!ModelState.IsValid)
			{
				return View(postEditModel);
			}

			var post = await _context.Posts.FirstOrDefaultAsync(i => i.PostId == postEditModel.PostId);
			if (post == null)
			{
				return NotFound();
			}

			if (imageFile != null)
			{
				var extension = Path.GetExtension(imageFile.FileName);
				if (!allowedExtensions.Contains(extension))
				{
					ViewBag.ImageFileError = "This extension is not allowed";
					return View(postEditModel);
				}

				var randomFileName = $"{Guid.NewGuid()}{extension}";
				var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

				using (var stream = new FileStream(imagePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				post.Image = randomFileName;
			}

			post.Content = postEditModel.Content;
			post.Title = postEditModel.Title;
			post.Url = postEditModel.Url;
			post.IsActive = postEditModel.IsActive;
			if (User.FindFirst(ClaimTypes.Role) == null)
				post.IsActive = false;

			_context.Update(post);
			await _context.SaveChangesAsync();

			return RedirectToAction("List");
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Delete(string? url)
		{

			if (url == null)
				return NotFound();

			var post = await _context.Posts.FirstOrDefaultAsync(u => u.Url == url);

			if (post == null)
				return NotFound();

			_context.Remove(post);
			await _context.SaveChangesAsync();
			return RedirectToAction("List", new { url = "admin" });

		}


		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteComment(int? commentId)
		{
			if (commentId == null)
				return NotFound();

			var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);

			if (comment == null)
				return NotFound();

			_context.Remove(comment);
			await _context.SaveChangesAsync();
			return Json(new { id = comment.CommentId, success = true });
		}



		[HttpPost]
		[Authorize]
		public async Task<string> AddLike(int postId, int userId)
		{
			Like like = new()
			{
				PostId = postId,
				UserId = userId,
			};

			_context.Add(like);
			await _context.SaveChangesAsync();
			return like.LikeId.ToString();

		}


		[HttpPost]
		[Authorize]
		public async Task<string> DeleteLike(int postId, int userId)
		{
			var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

			if (like == null)
				return "";

			_context.Likes.Remove(like);
			await _context.SaveChangesAsync();

			return like.LikeId.ToString();
		}

	}
}
