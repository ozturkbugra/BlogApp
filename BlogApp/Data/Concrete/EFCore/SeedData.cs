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

                        new Entity.Tag { Text = "Web Programlama", Url= "web-programlama" },
                        new Entity.Tag { Text = "Backend", Url = "backend" },
                        new Entity.Tag { Text = "Frontend", Url = "frontend" },
                        new Entity.Tag { Text = "Full Stack", Url = "full-stack" }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new Entity.User { UserName = "bugraozturk" },
                        new Entity.User { UserName = "admin" },
                        new Entity.User { UserName = "test" }
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
                            Url= "aspnet-core-dersleri"
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
