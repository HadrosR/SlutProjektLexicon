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
    using System.Text;

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
                    Students = new List<Models.Identity.ApplicationUser>(),
                    Documents = new List<Models.Classes.DocumentModels>()
                    {
                        new DocumentModels
                        {
                            Name = "C# Syntax Document",
                            Owner = adminUser,
                            Description = "An introduction to the C# syntax",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An introduction to the C# syntax"),
                        },
                        new DocumentModels
                        {
                            Name = "Visual Studio 2017 instructions",
                            Owner = adminUser,
                            Description = "A guide on how to install Visual Studio 2017",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A guide on how to install Visual Studio 2017"),
                        }
                    }

                },
                new CourseModels
                {
                    Name = "ASP.NET",
                    Description = "Introduction course to ASP.NET MVC",
                    StartDate = new DateTime(2017, 10, 11),
                    EndDate = new DateTime(2017, 10, 25),
                    Modules = new List<ModuleModels>(),
                    Students = new List<Models.Identity.ApplicationUser>(),
                    Documents = new List<Models.Classes.DocumentModels>()
                    {
                        new DocumentModels
                        {
                            Name = "ASP.NET Introduction Document",
                            Owner = adminUser,
                            Description = "An introduction to ASP.NET",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An introduction to ASP.NET"),
                        },
                        new DocumentModels
                        {
                            Name = "Another Visual Studio 2017 instructions",
                            Owner = adminUser,
                            Description = "A guide on how to install Visual Studio 2017 again",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A guide on how to install Visual Studio 2017 again"),
                        }
                    }
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
                            StartDate = new DateTime(2017, 09, 11,9,30,0),
                            EndDate = new DateTime(2017, 09, 11,17,0,0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, loops",
                                    Owner = adminUser,
                                    Description = "Excercise instructions, loops",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, loops"),
                                }
                            }
                        },
                        new ActivityModels
                        {
                            Name = "Garage 2.0",
                            Description = "Garage excercise",
                            StartDate = new DateTime(2017, 09, 12, 9,30,0),
                            EndDate = new DateTime(2017, 09, 12,17,0,0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Garage excercise instructions",
                                    Owner = adminUser,
                                    Description = "Garage excercise instructions",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Garage excercise instructions"),
                                }
                            }
                        }
                    },
                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "A little loop example",
                            Owner = adminUser,
                            Description = "Code snippets of C# loops",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("Code snippets of C# loops"),
                        },
                        new DocumentModels
                        {
                            Name = "Advanced loop example",
                            Owner = adminUser,
                            Description = "Code snippet of advanced usage of loops",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("Code snippet of advanced usage of loops"),
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
                            StartDate = new DateTime(2017, 09, 14, 9, 30, 0),
                            EndDate = new DateTime(2017, 09, 14, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, exceptions",
                                    Owner = adminUser,
                                    Description = "Excercise instructions, exceptions",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, exceptions"),
                                }
                            }
                        }
                    },
                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "Try Catch excercise",
                            Owner = adminUser,
                            Description = "Learn how to use try catch",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("How to use try catch"),
                        },
                        new DocumentModels
                        {
                            Name = "Example exceptions",
                            Owner = adminUser,
                            Description = "A list of example exceptions",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A list of example exceptions"),
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
                            StartDate = new DateTime(2017, 10, 09, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 09, 17, 0,0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, controller",
                                    Owner = adminUser,
                                    Description = "Excercise instructions, controller",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, controller"),
                                }
                            }
                        },
                        new ActivityModels
                        {
                            Name = "Models excercise",
                            Description = "Practice creating MVC-Models",
                            StartDate = new DateTime(2017, 10, 10, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 10, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, models",
                                    Owner = adminUser,
                                    Description = "Excercise instructions, models",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, models"),
                                }
                            }
                        },
                        new ActivityModels
                        {
                            Name = "View excercise",
                            Description = "Practice creating MVC-View",
                            StartDate = new DateTime(2017, 10, 11, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 11, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, views",
                                    Owner = adminUser,
                                    Description = "Excercise instructions, views",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, views"),
                                }
                            }
                        },new ActivityModels
                        {
                            Name = "MVC EntityFrameWork",
                            Description = "Framework that entity",
                            StartDate = new DateTime(2017, 10, 12, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 12, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, entity framework",
                                    Owner = adminUser,
                                    Description = "Excercise instructions, entity framework",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, entity framework"),
                                }
                            }
                        },
                        new ActivityModels
                        {
                            Name = "CRUD",
                            Description = "Basic CRUD Stuff",
                            StartDate = new DateTime(2017, 10, 13, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 13, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, CRUD",
                                    Owner = adminUser,
                                    Description = "Excercise instructions, CRUD",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, CRUD"),
                                }
                            }
                        },
                        new ActivityModels
                        {
                            Name = "Code Along",
                            Description = "TYPE FASTER FOOL",
                            StartDate = new DateTime(2017, 10, 16, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 16, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Code along instructions",
                                    Owner = adminUser,
                                    Description = "Code along instructions",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Code along instructions"),
                                }
                            }
                        },new ActivityModels
                        {
                            Name = "Project",
                            Description = "Don't forget to return the model",
                            StartDate = new DateTime(2017, 10, 17, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Project instructions",
                                    Owner = adminUser,
                                    Description = "Project instructions",
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Project instructions"),
                                }
                            }
                        }
                    },

                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "MVC Presentation",
                            Owner = adminUser,
                            Description = "A presentation of the ASP.MVC system",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A presentation of the ASP.MVC system"),
                        },
                        new DocumentModels
                        {
                            Name = "MVC code example",
                            Owner = adminUser,
                            Description = "An example usage of Models, Views and Controllers",
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An example usage of Models, Views and Controllers"),
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
