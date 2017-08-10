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
            string[] emails = new[] {
                "larare1@lexicon.se", "larare2@lexicon.se", "elev1@hotmail.com", "elev2@gmail.com","elev3@gmail.com",
                "elev4@gmail.com","elev5@gmail.com","elev6@gmail.com","elev7@gmail.com","elev8@gmail.com",
                "elev9@gmail.com","elev10@gmail.com","elev11@gmail.com","elev12@gmail.com","elev13@gmail.com","elev14@gmail.com"
            };
            string[] firstName = new[] {
                "Dimitris", "Mats", "Sebastian", "Henrik","Victor",
                "Erik", "Alex", "Adam", "Benjamin", "Jonas",
                "Peter", "Anton", "Mattias", "Joakim", "Obi-Wan","Ben"
            };
            string[] lastName = new[] {
                "Björlingh", "Johannesson", "Basse", "Forslin", "Berglund",
                "Andersson","Svensson","Johnsson","Gustavsson","Persson",
                "Björn","Ormtunga","Rehnvall","Von Anka","Kenobi","Kenobi"
            };

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
                            Name = "C# Syntax Document.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An introduction to the C# syntax"),
                            ContentType = "txt",
                        },
                        new DocumentModels
                        {
                            Name = "Visual Studio 2017 instructions.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A guide on how to install Visual Studio 2017"),
                            ContentType = "txt",
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
                            Name = "ASP.NET Introduction Document.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An introduction to ASP.NET"),
                            ContentType = "txt",
                        },
                        new DocumentModels
                        {
                            Name = "Another Visual Studio 2017 instructions.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A guide on how to install Visual Studio 2017 again"),
                            ContentType = "txt",
                        }
                    }
                },
                new CourseModels
                {
                    Name = "HTML",
                    Description = "Introduction course to HTML",
                    StartDate = new DateTime(2017, 08, 01),
                    EndDate = new DateTime(2017, 08, 25),
                    Modules = new List<ModuleModels>(),
                    Students = new List<Models.Identity.ApplicationUser>(),
                    Documents = new List<Models.Classes.DocumentModels>()
                    {
                        new DocumentModels
                        {
                            Name = "HTML Introduction Document.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An introduction to HTML"),
                            ContentType = "txt",
                        },
                        new DocumentModels
                        {
                            Name = "Another Visual Studio 2017 instructions.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A guide on how to install Visual Studio 2017 again"),
                            ContentType = "txt",
                        }
                    }
                },
                new CourseModels
                {
                    Name = "ASP.NET Basic",
                    Description = "Introduction course to ASP.NET MVC",
                    StartDate = new DateTime(2017, 11, 12),
                    EndDate = new DateTime(2017, 11, 30),
                    Modules = new List<ModuleModels>(),
                    Students = new List<Models.Identity.ApplicationUser>(),
                    Documents = new List<Models.Classes.DocumentModels>()
                    {
                        new DocumentModels
                        {
                            Name = "ASP.NET Introduction Document.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An introduction to ASP.NET"),
                            ContentType = "txt",
                        },
                        new DocumentModels
                        {
                            Name = "Another Visual Studio 2017 instructions.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A guide on how to install Visual Studio 2017 again"),
                            ContentType = "txt",
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
                                    Name = "Excercise instructions, loops.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, loops"),
                                    ContentType = "txt",
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
                                    Name = "Garage excercise instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Garage excercise instructions"),
                                    ContentType = "txt",
                                }
                            }
                        }
                    },
                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "A little loop example.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("Code snippets of C# loops"),
                            ContentType = "txt",
                        },
                        new DocumentModels
                        {
                            Name = "Advanced loop example.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("Code snippet of advanced usage of loops"),
                            ContentType = "txt",
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
                                    Name = "Excercise instructions, exceptions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, exceptions"),
                                    ContentType = "txt",
                                }
                            }
                        }
                    },

                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "Try Catch excercise.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("How to use try catch"),
                            ContentType = "txt",
                        },

                        new DocumentModels
                        {
                            Name = "Example exceptions.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A list of example exceptions"),
                            ContentType = "txt",
                        }
                    }
                },
                new ModuleModels
                {
                    Name = "MVC 2",
                    Description = "Brief introduction to MVC",
                    StartDate = new DateTime(2017, 10, 09),
                    EndDate = new DateTime(2017, 10, 20),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-controllers and its relations to the view and model",
                            StartDate = new DateTime(2017, 10, 09, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 09, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Controller excercise",
                            Description = "Practice creating MVC-controllers",
                            StartDate = new DateTime(2017, 10, 09, 14, 0, 0),
                            EndDate = new DateTime(2017, 10, 09, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, controller.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, controller"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-Models and its relations",
                            StartDate = new DateTime(2017, 10, 10, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 10, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Models excercise",
                            Description = "Practice creating MVC-Models",
                            StartDate = new DateTime(2017, 10, 10, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 10, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, models.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, models"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-View and its relations",
                            StartDate = new DateTime(2017, 10, 11, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 11, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "View excercise",
                            Description = "Practice creating MVC-View",
                            StartDate = new DateTime(2017, 10, 11, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 11, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, views.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, views"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "EntityFrameWork",
                            StartDate = new DateTime(2017, 10, 12, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "MVC EntityFrameWork",
                            Description = "Framework that entity",
                            StartDate = new DateTime(2017, 10, 12, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, entity framework.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, entity framework"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "How does CRUD work",
                            StartDate = new DateTime(2017, 10, 13, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "CRUD",
                            Description = "Basic CRUD Stuff",
                            StartDate = new DateTime(2017, 10, 13, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 13, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, CRUD.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, CRUD"),
                                    ContentType = "txt",
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
                                    Name = "Code along instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Code along instructions"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Project",
                            Description = "Don't forget to return the model",
                            StartDate = new DateTime(2017, 10, 17, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Project instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Project instructions"),
                                    ContentType = "txt",
                                }
                            }
                        }
                    },

                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "MVC Presentation.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A presentation of the ASP.MVC system"),
                            ContentType = "txt",
                        },

                        new DocumentModels
                        {
                            Name = "MVC code example.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An example usage of Models, Views and Controllers"),
                            ContentType = "txt",
                        }
                    }
                },
                new ModuleModels
                {
                    Name = "HTML",
                    Description = "Refresher to Html",
                    StartDate = new DateTime(2017, 10, 02),
                    EndDate = new DateTime(2017, 10, 06),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                                Name = "CSS",
                                Description = "Basic CSS Stuff",
                                StartDate = new DateTime(2017, 10, 02, 9, 0, 0),
                                EndDate = new DateTime(2017, 10, 02, 17, 0, 0),
                        }
                    }
                 },
                new ModuleModels
                {
                    Name = "MVC",
                    Description = "Brief introduction to MVC",
                    StartDate = new DateTime(2017, 08, 09),
                    EndDate = new DateTime(2017, 08, 20),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-controllers and its relations to the view and model",
                            StartDate = new DateTime(2017, 08, 14, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 14, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Controller excercise",
                            Description = "Practice creating MVC-controllers",
                            StartDate = new DateTime(2017, 08, 14, 14, 0, 0),
                            EndDate = new DateTime(2017, 08, 14, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, controller.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, controller"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-Models and its relations",
                            StartDate = new DateTime(2017, 08, 15, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 15, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Models excercise",
                            Description = "Practice creating MVC-Models",
                            StartDate = new DateTime(2017, 08, 15, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 15, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, models.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, models"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-View and its relations",
                            StartDate = new DateTime(2017, 08, 16, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 16, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "View excercise",
                            Description = "Practice creating MVC-View",
                            StartDate = new DateTime(2017, 08, 16, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 16, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, views.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, views"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "EntityFrameWork",
                            StartDate = new DateTime(2017, 08, 17, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "MVC EntityFrameWork",
                            Description = "Framework that entity",
                            StartDate = new DateTime(2017, 08, 17, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, entity framework.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, entity framework"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "How does CRUD work",
                            StartDate = new DateTime(2017, 08, 17, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "CRUD",
                            Description = "Basic CRUD Stuff",
                            StartDate = new DateTime(2017, 08, 17, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, CRUD.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, CRUD"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Code Along",
                            Description = "TYPE FASTER FOOL",
                            StartDate = new DateTime(2017, 08, 18, 9, 30, 0),
                            EndDate = new DateTime(2017, 08, 18, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Code along instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Code along instructions"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Project",
                            Description = "Don't forget to return the model",
                            StartDate = new DateTime(2017, 08, 18, 9, 30, 0),
                            EndDate = new DateTime(2017, 08, 18, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Project instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Project instructions"),
                                    ContentType = "txt",
                                }
                            }
                        }
                    },

                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "MVC Presentation.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A presentation of the ASP.MVC system"),
                            ContentType = "txt",
                        },

                        new DocumentModels
                        {
                            Name = "MVC code example.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An example usage of Models, Views and Controllers"),
                            ContentType = "txt",
                        }
                    }
                },
                new ModuleModels
                {
                    Name = "MVC 2",
                    Description = "Brief introduction to MVC",
                    StartDate = new DateTime(2017, 10, 09),
                    EndDate = new DateTime(2017, 10, 20),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-controllers and its relations to the view and model",
                            StartDate = new DateTime(2017, 10, 09, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 09, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Controller excercise",
                            Description = "Practice creating MVC-controllers",
                            StartDate = new DateTime(2017, 10, 09, 14, 0, 0),
                            EndDate = new DateTime(2017, 10, 09, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, controller.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, controller"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-Models and its relations",
                            StartDate = new DateTime(2017, 10, 10, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 10, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Models excercise",
                            Description = "Practice creating MVC-Models",
                            StartDate = new DateTime(2017, 10, 10, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 10, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, models.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, models"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-View and its relations",
                            StartDate = new DateTime(2017, 10, 11, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 11, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "View excercise",
                            Description = "Practice creating MVC-View",
                            StartDate = new DateTime(2017, 10, 11, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 11, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, views.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, views"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "EntityFrameWork",
                            StartDate = new DateTime(2017, 10, 12, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "MVC EntityFrameWork",
                            Description = "Framework that entity",
                            StartDate = new DateTime(2017, 10, 12, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, entity framework.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, entity framework"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "How does CRUD work",
                            StartDate = new DateTime(2017, 10, 13, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "CRUD",
                            Description = "Basic CRUD Stuff",
                            StartDate = new DateTime(2017, 10, 13, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 13, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, CRUD.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, CRUD"),
                                    ContentType = "txt",
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
                                    Name = "Code along instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Code along instructions"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Project",
                            Description = "Don't forget to return the model",
                            StartDate = new DateTime(2017, 10, 17, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Project instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Project instructions"),
                                    ContentType = "txt",
                                }
                            }
                        }
                    },

                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "MVC Presentation.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A presentation of the ASP.MVC system"),
                            ContentType = "txt",
                        },

                        new DocumentModels
                        {
                            Name = "MVC code example.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An example usage of Models, Views and Controllers"),
                            ContentType = "txt",
                        }
                    }
                },
                new ModuleModels
                {
                    Name = "HTML",
                    Description = "Refresher to Html",
                    StartDate = new DateTime(2017, 10, 02),
                    EndDate = new DateTime(2017, 10, 06),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                                Name = "CSS",
                                Description = "Basic CSS Stuff",
                                StartDate = new DateTime(2017, 10, 02, 9, 0, 0),
                                EndDate = new DateTime(2017, 10, 02, 17, 0, 0),
                        }
                    }
                 },
                new ModuleModels
                {
                    Name = "MVC",
                    Description = "Brief introduction to MVC",
                    StartDate = new DateTime(2017, 08, 09),
                    EndDate = new DateTime(2017, 08, 20),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-controllers and its relations to the view and model",
                            StartDate = new DateTime(2017, 08, 14, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 14, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Controller excercise",
                            Description = "Practice creating MVC-controllers",
                            StartDate = new DateTime(2017, 08, 14, 14, 0, 0),
                            EndDate = new DateTime(2017, 08, 14, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, controller.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, controller"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-Models and its relations",
                            StartDate = new DateTime(2017, 08, 15, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 15, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Models excercise",
                            Description = "Practice creating MVC-Models",
                            StartDate = new DateTime(2017, 08, 15, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 15, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, models.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, models"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-View and its relations",
                            StartDate = new DateTime(2017, 08, 16, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 16, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "View excercise",
                            Description = "Practice creating MVC-View",
                            StartDate = new DateTime(2017, 08, 16, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 16, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, views.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, views"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "EntityFrameWork",
                            StartDate = new DateTime(2017, 08, 17, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "MVC EntityFrameWork",
                            Description = "Framework that entity",
                            StartDate = new DateTime(2017, 08, 17, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, entity framework.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, entity framework"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "How does CRUD work",
                            StartDate = new DateTime(2017, 08, 17, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "CRUD",
                            Description = "Basic CRUD Stuff",
                            StartDate = new DateTime(2017, 08, 17, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, CRUD.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, CRUD"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Code Along",
                            Description = "TYPE FASTER FOOL",
                            StartDate = new DateTime(2017, 08, 18, 9, 30, 0),
                            EndDate = new DateTime(2017, 08, 18, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Code along instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Code along instructions"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Project",
                            Description = "Don't forget to return the model",
                            StartDate = new DateTime(2017, 08, 18, 9, 30, 0),
                            EndDate = new DateTime(2017, 08, 18, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Project instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Project instructions"),
                                    ContentType = "txt",
                                }
                            }
                        }
                    },

                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "MVC Presentation.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A presentation of the ASP.MVC system"),
                            ContentType = "txt",
                        },

                        new DocumentModels
                        {
                            Name = "MVC code example.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An example usage of Models, Views and Controllers"),
                            ContentType = "txt",
                        }
                    }
                },
                new ModuleModels
                {
                    Name = "MVC 2",
                    Description = "Brief introduction to MVC",
                    StartDate = new DateTime(2017, 10, 09),
                    EndDate = new DateTime(2017, 10, 20),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-controllers and its relations to the view and model",
                            StartDate = new DateTime(2017, 10, 09, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 09, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Controller excercise",
                            Description = "Practice creating MVC-controllers",
                            StartDate = new DateTime(2017, 10, 09, 14, 0, 0),
                            EndDate = new DateTime(2017, 10, 09, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, controller.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, controller"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-Models and its relations",
                            StartDate = new DateTime(2017, 10, 10, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 10, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Models excercise",
                            Description = "Practice creating MVC-Models",
                            StartDate = new DateTime(2017, 10, 10, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 10, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, models.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, models"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-View and its relations",
                            StartDate = new DateTime(2017, 10, 11, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 11, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "View excercise",
                            Description = "Practice creating MVC-View",
                            StartDate = new DateTime(2017, 10, 11, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 11, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, views.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, views"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "EntityFrameWork",
                            StartDate = new DateTime(2017, 10, 12, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "MVC EntityFrameWork",
                            Description = "Framework that entity",
                            StartDate = new DateTime(2017, 10, 12, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, entity framework.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, entity framework"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "How does CRUD work",
                            StartDate = new DateTime(2017, 10, 13, 9, 0, 0),
                            EndDate = new DateTime(2017, 10, 12, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "CRUD",
                            Description = "Basic CRUD Stuff",
                            StartDate = new DateTime(2017, 10, 13, 13, 0, 0),
                            EndDate = new DateTime(2017, 10, 13, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, CRUD.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, CRUD"),
                                    ContentType = "txt",
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
                                    Name = "Code along instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Code along instructions"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Project",
                            Description = "Don't forget to return the model",
                            StartDate = new DateTime(2017, 10, 17, 9, 30, 0),
                            EndDate = new DateTime(2017, 10, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Project instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Project instructions"),
                                    ContentType = "txt",
                                }
                            }
                        }
                    },

                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "MVC Presentation.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A presentation of the ASP.MVC system"),
                            ContentType = "txt",
                        },

                        new DocumentModels
                        {
                            Name = "MVC code example.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An example usage of Models, Views and Controllers"),
                            ContentType = "txt",
                        }
                    }
                },
                new ModuleModels
                {
                    Name = "HTML",
                    Description = "Refresher to Html",
                    StartDate = new DateTime(2017, 10, 02),
                    EndDate = new DateTime(2017, 10, 06),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                                Name = "CSS",
                                Description = "Basic CSS Stuff",
                                StartDate = new DateTime(2017, 10, 02, 9, 0, 0),
                                EndDate = new DateTime(2017, 10, 02, 17, 0, 0),
                        }
                    }
                 },
                new ModuleModels
                {
                    Name = "MVC",
                    Description = "Brief introduction to MVC",
                    StartDate = new DateTime(2017, 08, 09),
                    EndDate = new DateTime(2017, 08, 20),
                    Activities = new List<ActivityModels>
                    {
                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-controllers and its relations to the view and model",
                            StartDate = new DateTime(2017, 08, 14, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 14, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Controller excercise",
                            Description = "Practice creating MVC-controllers",
                            StartDate = new DateTime(2017, 08, 14, 14, 0, 0),
                            EndDate = new DateTime(2017, 08, 14, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, controller.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, controller"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-Models and its relations",
                            StartDate = new DateTime(2017, 08, 15, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 15, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "Models excercise",
                            Description = "Practice creating MVC-Models",
                            StartDate = new DateTime(2017, 08, 15, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 15, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, models.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, models"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "MVC-View and its relations",
                            StartDate = new DateTime(2017, 08, 16, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 16, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "View excercise",
                            Description = "Practice creating MVC-View",
                            StartDate = new DateTime(2017, 08, 16, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 16, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, views.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, views"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "EntityFrameWork",
                            StartDate = new DateTime(2017, 08, 17, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "MVC EntityFrameWork",
                            Description = "Framework that entity",
                            StartDate = new DateTime(2017, 08, 17, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, entity framework.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, entity framework"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Lecture",
                            Description = "How does CRUD work",
                            StartDate = new DateTime(2017, 08, 17, 9, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 12, 0,0),

                        },

                        new ActivityModels
                        {
                            Name = "CRUD",
                            Description = "Basic CRUD Stuff",
                            StartDate = new DateTime(2017, 08, 17, 13, 0, 0),
                            EndDate = new DateTime(2017, 08, 17, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Excercise instructions, CRUD.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Excercise instructions, CRUD"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Code Along",
                            Description = "TYPE FASTER FOOL",
                            StartDate = new DateTime(2017, 08, 18, 9, 30, 0),
                            EndDate = new DateTime(2017, 08, 18, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Code along instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Code along instructions"),
                                    ContentType = "txt",
                                }
                            }
                        },

                        new ActivityModels
                        {
                            Name = "Project",
                            Description = "Don't forget to return the model",
                            StartDate = new DateTime(2017, 08, 18, 9, 30, 0),
                            EndDate = new DateTime(2017, 08, 18, 17, 0, 0),
                            Documents = new List<DocumentModels>
                            {
                                new DocumentModels
                                {
                                    Name = "Project instructions.txt",
                                    Owner = adminUser,
                                    TimeStamp = DateTime.Now,
                                    Contents = Encoding.UTF8.GetBytes("Project instructions"),
                                    ContentType = "txt",
                                }
                            }
                        }
                    },

                    Documents = new List<DocumentModels>
                    {
                        new DocumentModels
                        {
                            Name = "MVC Presentation.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("A presentation of the ASP.MVC system"),
                            ContentType = "txt",
                        },

                        new DocumentModels
                        {
                            Name = "MVC code example.txt",
                            Owner = adminUser,
                            TimeStamp = DateTime.Now,
                            Contents = Encoding.UTF8.GetBytes("An example usage of Models, Views and Controllers"),
                            ContentType = "txt",
                        }
                    }
                },
            };
            for (int e = 0; e < 14; e++)
            {
                if (e%2==0)
                {
                    courses[1].Students.Add(students.ElementAt(e));
                }
                else
                {
                    courses[0].Students.Add(students.ElementAt(e));
                }
            }
            
            

            courses[0].Modules.Add(modules[0]);
            courses[0].Modules.Add(modules[1]);
            courses[0].Modules.Add(modules[4]);
            courses[1].Modules.Add(modules[2]);
            courses[1].Modules.Add(modules[3]);
            courses[1].Modules.Add(modules[9]);
            courses[2].Modules.Add(modules[5]);
            courses[2].Modules.Add(modules[6]);
            courses[3].Modules.Add(modules[7]);
            courses[3].Modules.Add(modules[8]);


            context.SaveChanges();
        }
    }
}
