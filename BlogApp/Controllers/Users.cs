﻿using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EFCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }
        
        public IActionResult Register()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users.FirstOrDefaultAsync(x=> x.UserName == model.UserName || x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.CreateUser(new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "team-6.jpg"
                    });
                    return RedirectToAction("Login");

                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya Mail kullanımda");
                }

            }  
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                if(isUser != null)
                {
                    var userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserID.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));

                    //burada rol verilir
                    if(isUser.Email == "bugraozturk@hotmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);


                    //beni hatırla olayı
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity),authProperties);


                    return RedirectToAction("Index","Posts");
                }
                else
                {
                    ModelState.AddModelError("", "Mail veya Şifre yanlış");
                }
            }
           
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        

        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = _userRepository.Users.Include(x => x.Posts).Include(x => x.Comments).ThenInclude(x => x.Post).FirstOrDefault(x=> x.UserName == username);

            if(user == null)
            {
                return NotFound();

            }

            return View(user);
        }



    }
}
