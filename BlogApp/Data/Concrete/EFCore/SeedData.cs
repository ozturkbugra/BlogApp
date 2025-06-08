using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EFCore
{
    public class SeedData
    {
        public static void TestVerileriDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(

                        new Entity.Tag { Text = "Web Programlama", Url= "web-programlama", Color = TagColors.warning },
                        new Entity.Tag { Text = "Backend", Url = "backend",  Color = TagColors.secondary },
                        new Entity.Tag { Text = "Frontend", Url = "frontend", Color = TagColors.danger },
                        new Entity.Tag { Text = "Full Stack", Url = "full-stack", Color = TagColors.primary }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new Entity.User { UserName = "bugraozturk" ,  Image= "team-1.jpg",Name = "Buğra Öztürk", Email = "bugraozturk@hotmail.com", Password="123456" },
                        new Entity.User { UserName = "admin" , Image = "team-2.jpg", Name = "Mert Ak", Email = "mertak@hotmail.com", Password = "123456" },
                        new Entity.User { UserName = "test" , Image = "team-3.jpg", Name = "test test", Email = "test@hotmail.com", Password = "123456" }
                        );
                    context.SaveChanges();

                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Entity.Post { Title = "Asp.net Core",
                            Content = "Asp.net Core dersleri",
                            IsActive = true,
                            Image = "core.jpg",
                            PusblishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            UserID =1,
                            Url= "aspnet-core-dersleri",
                            Comments = new List<Comment> { 
                                new Comment {Text = "Mükemmel Bir Kurs",PublishedOn = DateTime.Now,UserID = 1}, 
                                new Comment {Text = "Çok iyi şeyler öğrendim",PublishedOn = DateTime.Now.AddHours(-10),UserID = 2}, 
                                new Comment {Text = "Teşekkürler Gerçekten",PublishedOn = DateTime.Now.AddDays(1),UserID = 3} 
                            }

                        },new Entity.Post { Title = "Asp.net MVC",
                            Content = "Asp.net MVC dersleri",
                            IsActive = true,
                            Image = "mvc.png",
                            PusblishedOn = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(2).ToList(),
                            UserID =2,
                            Url = "aspnet-mvc-dersleri"
                        },new Entity.Post { Title = "HTML",
                            Content = "Html dersleri",
                            IsActive = true,
                            Image = "html.jpg",
                            PusblishedOn = DateTime.Now.AddDays(-2),
                            Tags = context.Tags.Take(2).ToList(),
                            UserID =3,
                            Url = "html"
                        }

                        );
                    context.SaveChanges();

                }

            }
        }
    }
}
