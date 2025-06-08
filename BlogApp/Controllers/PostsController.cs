using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EFCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {

        private IPostRepository _postrepository;
        private ICommentRepository _commentrepository;

        public PostsController(IPostRepository postRepository, ICommentRepository commentrepository)
        {
            _postrepository = postRepository;
            _commentrepository = commentrepository;
        }

        public async Task<IActionResult> Index(string url)
        {
            var claims = User.Claims;

            var posts = _postrepository.Posts.Where(x=> x.IsActive == true);

            if (!string.IsNullOrEmpty(url))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == url));
            }
            return View(
                new PostsViewModel
                {
                    Posts = await posts.ToListAsync(),

                }

            );
        }

        public async Task<IActionResult> Details(string url)
        {
            return View(await _postrepository.Posts.Include(x=> x.User).Include(x=> x.Tags).Include(x=> x.Comments).ThenInclude(x=> x.User).FirstOrDefaultAsync(p => p.Url == url));
        }

        [HttpPost]
        public JsonResult AddComment(int PostID, string Text)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment
            {
                Text = Text,
                PublishedOn = DateTime.Now,
                PostID = PostID,
                UserID = int.Parse(userID ?? ""),
            };
            _commentrepository.CreateComment(entity);

            return Json(new
            {
                userName,
                Text,
                entity.PublishedOn,
                avatar
            });

            //return Redirect("/posts/details/"+Url);
        }


        public IActionResult Create() 
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            if(id == null)
            {
                return NotFound();
            }

            var post = _postrepository.Posts.FirstOrDefault(x => x.PostID == id);

            if(post == null)
            {
                return NotFound();
            }

            return View(new PostCreateViewModel
            {
                PostID = post.PostID,
                Content = post.Content,
                Description = post.Description,
                Title = post.Title,
                Url = post.Url,
                IsActive = post.IsActive
            });
        }

        [HttpPost]
        public IActionResult Edit(PostCreateViewModel model)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var update = new Post
                {
                    PostID = model.PostID,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url
                };

                if(User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    update.IsActive = model.IsActive;
                }
                _postrepository.EditPost(update);
                return RedirectToAction("List");
            }
            return View(model);
        }



        public async Task<IActionResult> List()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postrepository.Posts;

            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(x => x.UserID == userID);
            }



            return View(await posts.ToListAsync());
        }

        [HttpPost]
        public ActionResult Create(PostCreateViewModel model)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                _postrepository.CreatePost(new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    Description = model.Description,
                    Url = model.Url,
                    UserID = int.Parse(userID),
                    PusblishedOn = DateTime.Now,
                    Image = "mvc.png",
                    IsActive = false,


                });

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
