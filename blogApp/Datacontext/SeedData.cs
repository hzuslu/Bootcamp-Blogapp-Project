using blogApp.Controllers;
using blogApp.Datacontext;
using blogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Data
{
    public static class SeedData
    {
        public static async Task TestDataAsync(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();


                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "Dogs", Url = "dogs" },
                        new Tag { Text = "Cats", Url = "cats" },
                        new Tag { Text = "Birds", Url = "birds" },
                        new Tag { Text = "Fish", Url = "fish" },
                        new Tag { Text = "Rabbits", Url = "rabbits" },
                        new Tag { Text = "Hamsters", Url = "hamsters" },
                        new Tag { Text = "Reptiles", Url = "reptiles" },
                        new Tag { Text = "Pet Care", Url = "pet-care" },
                        new Tag { Text = "Training", Url = "training" },
                        new Tag { Text = "Health", Url = "health" },
                        new Tag { Text = "Nutrition", Url = "nutrition" },
                        new Tag { Text = "Play and Entertainment", Url = "play-and-entertainment" },
                        new Tag { Text = "Pet Adoption", Url = "pet-adoption" }
                    );

                    await context.SaveChangesAsync();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            UserName = "alice_smith",
                            UserFirstName = "Alice",
                            UserSurname = "Smith",
                            Password = "password123",
                            Phone = "5559876543",
                            Email = "alice@example.com",
                            Address = "123 Main St",
                            City = "New York",
                            Country = "USA",
                            ZipCode = 10001,
                            Education = "Columbia University",
                            RegisterDate = DateTime.Now.AddDays(-40),
                            Image = "p1.jpg"
                        },
                        new User
                        {
                            UserName = "bob_johnson",
                            UserFirstName = "Bob",
                            UserSurname = "Johnson",
                            Password = "pass123word",
                            Phone = "1235554321",
                            Email = "bob@example.com",
                            Address = "456 Elm St",
                            City = "Los Angeles",
                            Country = "USA",
                            ZipCode = 90001,
                            Education = "Stanford University",
                            RegisterDate = DateTime.Now.AddDays(-35),
                            Image = "p2.jpg"
                        },
                        new User
                        {
                            UserName = "emily_wilson",
                            UserFirstName = "Emily",
                            UserSurname = "Wilson",
                            Password = "wilson987",
                            Phone = "7771112233",
                            Email = "emily@example.com",
                            Address = "789 Oak St",
                            City = "Chicago",
                            Country = "USA",
                            ZipCode = 60601,
                            Education = "Massachusetts Institute of Technology (MIT)",
                            RegisterDate = DateTime.Now.AddDays(-38),
                            Image = "p3.jpg"
                        },
                        new User
                        {
                            UserName = "chris_anderson",
                            UserFirstName = "Chris",
                            UserSurname = "Anderson",
                            Password = "chrispass",
                            Phone = "5554443333",
                            Email = "chris@example.com",
                            Address = "101 Pine St",
                            City = "Houston",
                            Country = "USA",
                            ZipCode = 77002,
                            Education = "University of Texas at Austin",
                            RegisterDate = DateTime.Now.AddDays(-42),
                            Image = "p4.jpg"
                        },
                        new User
                        {
                            UserName = "sarah_carter",
                            UserFirstName = "Sarah",
                            UserSurname = "Carter",
                            Password = "sarahpass",
                            Phone = "9998887777",
                            Email = "sarah@example.com",
                            Address = "222 Maple St",
                            City = "Miami",
                            Country = "USA",
                            ZipCode = 33101,
                            Education = "University of Florida",
                            RegisterDate = DateTime.Now.AddDays(-36),
                            Image = "p5.jpg"
                        },
                        new User
                        {
                            UserName = "david_jackson",
                            UserFirstName = "David",
                            UserSurname = "Jackson",
                            Password = "davidpass",
                            Phone = "1112223333",
                            Email = "david@example.com",
                            Address = "333 Birch St",
                            City = "San Francisco",
                            Country = "USA",
                            ZipCode = 94102,
                            Education = "California Institute of Technology (Caltech)",
                            RegisterDate = DateTime.Now.AddDays(-39),
                            Image = "p6.jpg"
                        }
                    );

                    await context.SaveChangesAsync();
                }

                if (!context.Posts.Any())
                {
                    var userAlice = context.Users.FirstOrDefault(u => u.UserName == "alice_smith");
                    var userBob = context.Users.FirstOrDefault(u => u.UserName == "bob_johnson");
                    var userEmily = context.Users.FirstOrDefault(u => u.UserName == "emily_wilson");
                    var userChris = context.Users.FirstOrDefault(u => u.UserName == "chris_anderson");
                    var userSarah = context.Users.FirstOrDefault(u => u.UserName == "sarah_carter");
                    var userDavid = context.Users.FirstOrDefault(u => u.UserName == "david_jackson");

                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "Effective Training Techniques for Dogs",
                            Content = "Training your dog is essential for a happy and harmonious relationship. In this guide, we explore effective training techniques for dogs, including positive reinforcement, clicker training, and obedience commands. Whether you're a new pet owner or looking to improve your dog's behavior, this article will help you build a strong bond with your furry friend.",
                            PostTime = DateTime.Now.AddDays(-20),
                            IsActive = true,
                            User = userAlice!,
                            Image = "1.jpg",
                            Tags = new List<Tag> { context.Tags.FirstOrDefault(t => t.Text == "Training")!, context.Tags.FirstOrDefault(t => t.Text == "Dogs")! },
                            Comments = new List<Comment>(),
                            Url = "dog-training"
                        },
                    new Post
                    {
                        Title = "Cat Care Essentials: Providing the Best for Your Feline Friend",
                        Content = "Caring for your cat involves more than just providing food and shelter. In this comprehensive article, we discuss cat care essentials, including proper nutrition, grooming tips, and creating a stimulating environment. Whether you're a seasoned cat owner or considering adopting a feline companion, this guide will help you ensure a happy and healthy life for your cat.",
                        PostTime = DateTime.Now.AddDays(-18),
                        IsActive = true,
                        User = userBob!,
                        Image = "2.jpg",
                        Tags = new List<Tag> { context.Tags.FirstOrDefault(t => t.Text == "Pet Care")!, context.Tags.FirstOrDefault(t => t.Text == "Cats")! },
                        Comments = new List<Comment>(),
                        Url = "cat-care"
                    },
                    new Post
                    {
                        Title = "Birdwatching: A Beginner's Guide to Avian Adventure",
                        Content = "Birdwatching is a rewarding hobby that connects you with nature and allows you to observe the beauty of birds in their natural habitat. In this beginner's guide, we provide tips for birdwatching, including bird identification, equipment recommendations, and popular birdwatching locations. Whether you're an outdoor enthusiast or simply curious about birds, this guide will help you embark on an avian adventure.",
                        PostTime = DateTime.Now.AddDays(-25),
                        IsActive = true,
                        User = userEmily!,
                        Image = "3.jpg",
                        Tags = new List<Tag> { context.Tags.FirstOrDefault(t => t.Text == "Rabbits")!, context.Tags.FirstOrDefault(t => t.Text == "Birds")! },
                        Comments = new List<Comment>(),
                        Url = "birdwatching"
                    },
                    new Post
                    {
                        Title = "Aquarium Setup Guide: Creating a Thriving Underwater Ecosystem",
                        Content = "Setting up an aquarium is an exciting journey that requires careful planning and attention to detail. In this guide, we walk you through the steps of aquarium setup, including tank selection, water conditioning, and choosing compatible fish species. Whether you're a beginner or an experienced hobbyist, this article will help you create a vibrant underwater world in your own home.",
                        PostTime = DateTime.Now.AddDays(-15),
                        IsActive = true,
                        User = userChris!,
                        Image = "4.jpg",
                        Tags = new List<Tag> { context.Tags.FirstOrDefault(t => t.Text == "Health")!, context.Tags.FirstOrDefault(t => t.Text == "Fish")! },
                        Comments = new List<Comment>(),
                        Url = "aquarium-setup"
                    },
                    new Post
                    {
                        Title = "Hamster Care 101: Providing a Happy and Healthy Home for Your Hamster",
                        Content = "Hamsters are adorable and low-maintenance pets that make great companions for both adults and children. In this guide, we share essential tips for hamster care, including cage setup, diet recommendations, and handling techniques. Whether you're a first-time hamster owner or looking to improve your hamster's quality of life, this article will help you create a nurturing environment for your furry friend.",
                        PostTime = DateTime.Now.AddDays(-22),
                        IsActive = true,
                        User = userSarah!,
                        Image = "5.jpg",
                        Tags = new List<Tag> { context.Tags.FirstOrDefault(t => t.Text == "Pet Care")!, context.Tags.FirstOrDefault(t => t.Text == "Hamsters")! },
                        Comments = new List<Comment>(),
                        Url = "hamster-care"
                    },
                    new Post
                    {
                        Title = "Reptile Terrarium Setup: Building a Safe and Stimulating Habitat",
                        Content = "Creating the perfect habitat for your reptile requires careful planning and consideration of their specific needs. In this article, we provide tips for setting up a reptile terrarium, including temperature control, substrate selection, and decorating with natural elements. Whether you're a reptile enthusiast or a new reptile owner, this guide will help you create a comfortable and engaging environment for your scaly friend.",
                        PostTime = DateTime.Now.AddDays(-28),
                        IsActive = true,
                        User = userDavid!,
                        Image = "6.jpg",
                        Tags = new List<Tag> { context.Tags.FirstOrDefault(t => t.Text == "Reptiles")!, context.Tags.FirstOrDefault(t => t.Text == "Nutrition")! },
                        Comments = new List<Comment>(),
                        Url = "reptile-terrarium"
                    }
                    );

                    await context.SaveChangesAsync();
                }

                if (!context.Comments.Any())
                {
                    var users = await context.Users.ToListAsync();
                    var posts = await context.Posts.ToListAsync();

                    var random = new Random();
                    for (int i = 0; i < 7; i++)
                    {
                        var userIndex = random.Next(users.Count);
                        var postIndex = random.Next(posts.Count);
                        var post = posts[postIndex];
                        var user = users[userIndex];

                        var numComments = random.Next(3, 5);
                        for (int j = 0; j < numComments; j++)
                        {
                            post.Comments.Add(new Comment
                            {
                                Content = $"Random comment {j + 1}",
                                CommentTime = DateTime.Now.AddDays(-random.Next(1, 30)),
                                User = user
                            });
                        }
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Text = "Dogs" },
                        new Category { Text = "Cats" },
                        new Category { Text = "Birds" },
                        new Category { Text = "Fish" },
                        new Category { Text = "Small Pets" },
                        new Category { Text = "Reptiles" },
                        new Category { Text = "Wildlife" }
                    );

                    await context.SaveChangesAsync();


                }

                if (!context.Events.Any())
                {
                    var categories = await context.Categories.ToListAsync();
                    var users = await context.Users.ToListAsync();
                    var random = new Random();

                    context.Events.AddRange(
                        new Event
                        {
                            EventTitle = "Dog Adoption Day",
                            Location = "Local Animal Shelter",
                            StartTime = new DateTime(2024, 4, random.Next(1, 15)),
                            User = users[random.Next(users.Count)],
                            Category = categories.FirstOrDefault(c => c.Text == "Dogs")!,
                            Users = users.OrderBy(x => random.Next()).Take(3).ToList()
                        },
                        new Event
                        {
                            EventTitle = "Cat Health Workshop",
                            Location = "Vet Clinic",
                            StartTime = new DateTime(2024, 4, random.Next(1, 15)),
                            User = users[random.Next(users.Count)],
                            Category = categories.FirstOrDefault(c => c.Text == "Cats")!,
                            Users = users.OrderBy(x => random.Next()).Take(3).ToList()
                        },
                        new Event
                        {
                            EventTitle = "Bird Watching Tour",
                            Location = "Nature Reserve",
                            StartTime = new DateTime(2024, 4, random.Next(1, 15)),
                            User = users[random.Next(users.Count)],
                            Category = categories.FirstOrDefault(c => c.Text == "Birds")!,
                            Users = users.OrderBy(x => random.Next()).Take(3).ToList()
                        },
                        new Event
                        {
                            EventTitle = "Aquarium Exploration",
                            Location = "City Aquarium",
                            StartTime = new DateTime(2024, 4, random.Next(1, 15)),
                            User = users[random.Next(users.Count)],
                            Category = categories.FirstOrDefault(c => c.Text == "Fish")!,
                            Users = users.OrderBy(x => random.Next()).Take(3).ToList()
                        },
                        new Event
                        {
                            EventTitle = "Reptile Exhibition",
                            Location = "Reptile Park",
                            StartTime = new DateTime(2024, 4, random.Next(1, 15)),
                            User = users[random.Next(users.Count)],
                            Category = categories.FirstOrDefault(c => c.Text == "Reptiles")!,
                            Users = users.OrderBy(x => random.Next()).Take(3).ToList()
                        }
                    );

                    await context.SaveChangesAsync();

                    var random2 = new Random();
                    var users2 = await context.Users.ToListAsync();
                    var events = await context.Events.ToListAsync();

                    foreach (var ev in events)
                    {
                        var randomUsers = users2.OrderBy(x => random2.Next()).Take(4).ToList();

                        ev.Users.AddRange(randomUsers);
                    }

                    await context.SaveChangesAsync();

                }
            }
        }
    }
}

