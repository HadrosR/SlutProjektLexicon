namespace LexiconLMSPortal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models.Classes;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<LexiconLMSPortal.Models.Identity.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LexiconLMSPortal.Models.Identity.ApplicationDbContext context)
        {
            /* DO NOT TOUCH! :D */
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            string[] roleNames = new[] { "Teacher", "Student" };

            foreach (string roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    IdentityRole role = new IdentityRole { Name = roleName };
                    IdentityResult result = roleManager.Create(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }

            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);
            string[] emails = new[] { "larare1@lexicon.se", "larare2@lexicon.se", "elev1@hotmail.com", "elev2@gmail.com" };
            string[] firstName = new[] { "Dimitris", "Mats", "Sebastian", "Henrik" };
            string[] lastName = new[] { "Björlingh", "Johannesson", "Basse", "Forslin" };

            int i = 0;
            foreach (string email in emails)
            {
                if (!context.Users.Any(u => u.UserName == email))
                {
                    Models.Identity.ApplicationUser user = new Models.Identity.ApplicationUser { UserName = email, Email = email, FirstName = firstName[i], LastName = lastName[i] };
                    var result = userManager.Create(user, "victor");
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
                i++;
            }

            Models.Identity.ApplicationUser adminUser = userManager.FindByName("larare1@lexicon.se");
            userManager.AddToRole(adminUser.Id, "Teacher");
            adminUser = userManager.FindByName("larare2@lexicon.se");
            userManager.AddToRole(adminUser.Id, "Teacher");

            var students = userManager.Users.ToList().Where(u => u.Email != "larare1@lexicon.se" && u.Email != "larare2@lexicon.se");
            foreach (var student in students)
            {
                userManager.AddToRole(student.Id, "Student");
            }
            context.SaveChanges();

            CourseModels[] courses = new CourseModels[] {
                new CourseModels
                {
                    Name = "C#",
                    Description = "Introduction course to C# .NET",
                    StartDate = new DateTime(2017, 09, 11),
                    EndDate = new DateTime(2017, 09, 25),
                    Modules = new List<ModuleModels>(),
                    Students = new List<Models.Identity.ApplicationUser>()

                },
                new CourseModels
                {
                    Name = "ASP.NET",
                    Description = "Introduction course to ASP.NET MVC",
                    StartDate = new DateTime(2017, 10, 11),
                    EndDate = new DateTime(2017, 10, 25),
                    Modules = new List<ModuleModels>(),
                    Students = new List<Models.Identity.ApplicationUser>()
                }
            };


            foreach (CourseModels g in courses)
            {
                context.Courses.Add(g);
            }

            context.SaveChanges();


            ModuleModels[] modules = new ModuleModels[]{
                new ModuleModels
                {
                    Name = "Loops",
                    Description = "Brief introduction to loops",
                    StartDate = new DateTime(2017, 09, 11),
                    EndDate = new DateTime(2017, 09, 13),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Loops excercise",
                            Description = "Learn how to implement for, while, and do-while loops",
                            StartDate = new DateTime(2017, 09, 11),
                            EndDate = new DateTime(2017, 09, 12)
                        },
                        new ActivityModels
                        {
                            Name = "Garage 2.0",
                            Description = "Garage excercise",
                            StartDate = new DateTime(2017, 09, 12),
                            EndDate = new DateTime(2017, 09, 13)
                        }
                    }

                },
                new ModuleModels
                {
                    Name = "Try, Catch and Exceptions",
                    Description = "Errors and exception handling",
                    StartDate = new DateTime(2017, 09, 14),
                    EndDate = new DateTime(2017, 09, 16),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Exception excercise",
                            Description = "Learn how to implement exception handling",
                            StartDate = new DateTime(2017, 09, 14),
                            EndDate = new DateTime(2017, 09, 15)
                        }
                    }
                },
                new ModuleModels
                {
                    Name = "MVC",
                    Description = "Brief introduction to MVC",
                    StartDate = new DateTime(2017, 10, 09),
                    EndDate = new DateTime(2017, 10, 20),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Controller excercise",
                            Description = "Practice creating MVC-controllers",
                            StartDate = new DateTime(2017, 10, 09),
                            EndDate = new DateTime(2017, 10, 09)
                        },
                        new ActivityModels
                        {
                            Name = "Models excercise",
                            Description = "Practice creating MVC-Models",
                            StartDate = new DateTime(2017, 10, 10),
                            EndDate = new DateTime(2017, 10, 10)
                        },
                        new ActivityModels
                        {
                            Name = "View excercise",
                            Description = "Practice creating MVC-View",
                            StartDate = new DateTime(2017, 10, 11),
                            EndDate = new DateTime(2017, 10, 11)
                        },new ActivityModels
                        {
                            Name = "MVC EntityFrameWork",
                            Description = "Framework that entity",
                            StartDate = new DateTime(2017, 10, 12),
                            EndDate = new DateTime(2017, 10, 12)
                        },
                        new ActivityModels
                        {
                            Name = "CRUD",
                            Description = "Basic CRUD Stuff",
                            StartDate = new DateTime(2017, 10, 13),
                            EndDate = new DateTime(2017, 10, 15)
                        },
                        new ActivityModels
                        {
                            Name = "Code Along",
                            Description = "TYPE FASTER FOOL",
                            StartDate = new DateTime(2017, 10, 16),
                            EndDate = new DateTime(2017, 10, 17)
                        },new ActivityModels
                        {
                            Name = "Project",
                            Description = "Don't forget to return the model",
                            StartDate = new DateTime(2017, 10, 17),
                            EndDate = new DateTime(2017, 10, 20)
                        }
                    }
                }
            };
            courses[0].Students.Add(students.ElementAt(0));
            courses[1].Students.Add(students.ElementAt(1));

            courses[0].Modules.Add(modules[0]);
            courses[0].Modules.Add(modules[1]);
            courses[1].Modules.Add(modules[2]);

            context.SaveChanges();
        }
    }
}
