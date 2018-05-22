namespace StudentSystem
{
    using Microsoft.EntityFrameworkCore;
    using StudentSystem.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Startup
    {
        private static Random random = new Random();

        public static void Main()
        {
            using (StudentDbContext context = new StudentDbContext())
            {
                context.Database.Migrate();

                SeedData(context);
                //PrintStudentsWithHomework(context);
                //PrintCoursesAndResources(context);
                //PrintAllCourseWithMoreThanFiveResources(context);
                //PrintCourseActionOnDate(context);
                PrintStudentsWithPrices(context);
            }
        }

        private static void PrintStudentsWithPrices(StudentDbContext context)
        {
            var result = context
                 .Students
                 .Where(s=>s.Courses.Any())
                 .Select(c => new
                 {
                     c.Name,
                     Courses = c.Courses.Count,
                     TotalPrice = c.Courses.Sum(s => s.Course.Price),
                     AveragePrice = c.Courses.Average(s => s.Course.Price)
                 })
                 .OrderByDescending(c => c.TotalPrice)
                 .ThenByDescending(c => c.Courses)
                 .ThenBy(c => c.Name)
                 .ToList();
            foreach (var student in result)
            {
                Console.WriteLine($"{student.Name} - {student.Courses} - {student.TotalPrice} - {student.AveragePrice}");
            }
        }

        private static void PrintCourseActionOnDate(StudentDbContext context)
        {
            var date = DateTime.Now.AddDays(25);

            var result = context
                .Courses
                .Where(c => c.StartDate < date && date < c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.StartDate,
                    c.EndDate,
                    Duration = c.EndDate.Subtract(c.StartDate),
                    Students = c.Students

                })
                .OrderByDescending(c=>c.Students)
                .ThenByDescending(c=>c.Duration)
                .ToList();
            foreach (var course in result)
            {
                Console.WriteLine($"{course.Name}: {course.StartDate.ToShortDateString()} - {course.EndDate.ToShortDateString()}");
                Console.WriteLine($"---Duration: {course.Duration.TotalDays}");
                Console.WriteLine($"---Duration: {course.Students}");
            }
        }

        private static void PrintAllCourseWithMoreThanFiveResources(StudentDbContext context)
        {
            var result = context
                .Courses
                .OrderByDescending(c => c.Resources.Count)
                .ThenByDescending(c => c.StartDate)
                .Where(c => c.Resources.Count > 5)
                .Select(c => new
                {
                    c.Name,
                    Resources = c.Resources.Count
                })
                .ToList();
            foreach (var course in result)
            {
                Console.WriteLine($"{course.Name} - {course.Resources}");
            }
        }

        private static void PrintCoursesAndResources(StudentDbContext context)
        {
            var result = context
                .Courses
                .OrderBy(c => c.StartDate)
                .Select(c => new
                {
                    c.Name,
                    c.Description,
                    Resources = c.Resources.Select(r => new
                    {
                        r.Name,
                        r.Url,
                        r.ResourceType
                    })
                })
                .ToList();
            foreach (var course in result)
            {
                Console.WriteLine($"{course.Name} - {course.Description}");
                foreach (var resource in course.Resources)
                {
                    Console.WriteLine($"{resource.Name} - {resource.Url}");
                }
            }
        }

        private static void PrintStudentsWithHomework(StudentDbContext context)
        {
            var result = context
                 .Students
                 .Select(s => new
                 {
                     s.Name,
                     Homeworks = s.HomeworkSubmissions.Select(h => new
                     {
                         h.Content,
                         h.ContentType
                     })
                 })
                 .ToList();
            foreach (var student in result)
            {
                Console.WriteLine(student.Name);

                foreach (var homework in student.Homeworks)
                {
                    Console.WriteLine($"---{homework.Content} - {homework.ContentType}");
                }
            }
        }

        private static void SeedData(StudentDbContext context)
        {
            //students
            Console.WriteLine("Adding students..");
            const int totalStudents = 25;
            const int totalCourses = 10;
            var currentDate = DateTime.Now;

            for (int i = 0; i < totalStudents; i++)
            {
                context.Students.Add(new Student
                {
                    Name = $"Student {i}",
                    RegistrationDate = currentDate.AddDays(i),
                    Birthday = currentDate.AddYears(-20).AddDays(i),
                    PhoneNumber = $"Random Phone {i}"
                });
            }

            context.SaveChanges();

            //courses
            var addedCourses = new List<Course>();
            for (int i = 0; i < totalCourses; i++)
            {
                var course = new Course
                {
                    Name = $"Course {i}",
                    Description = $" Course Details {i}",
                    Price = 100 * i,
                    StartDate = currentDate.AddDays(i),
                    EndDate = currentDate.AddDays(20 + i)
                };
                addedCourses.Add(course);
                context.Courses.Add(course);
            }

            context.SaveChanges();
            // Students in courses
            var studentsIds = context
                       .Students
                       .Select(s => s.Id)
                       .ToList();
            for (int i = 0; i < totalCourses; i++)
            {
                var currentCourse = addedCourses[i];
                var studentsInCourse = random.Next(2, totalStudents / 2);
                for (int j = 0; j < studentsInCourse; j++)
                {
                    var studentId = studentsIds[random.Next(0, studentsIds.Count)];

                    if (!currentCourse.Students.Any(s=>s.StudentId == studentId))
                    {
                        currentCourse.Students.Add(new StudentsCourses
                        {
                            StudentId = studentId
                        });
                    }
                    else
                    {
                        j--;
                    }
                  
                }
                var resourcesInCourse = random.Next(2, 20);
                var types = new[] { 0, 1, 2, 999 };
                for (int j = 0; j < resourcesInCourse; j++)
                {
                    currentCourse.Resources.Add(new Resource
                    {
                        Name = $"Resource {i} {j}",
                        Url = $"URL{i} {j}",
                        ResourceType = (TypeOfResource)types[random.Next(0, types.Length)],
                    });
                } 
            }
            context.SaveChanges();
            // Homeworks
            for (int i = 0; i < totalCourses; i++)
            {
                var currentCourse = addedCourses[i];
                var studentsInCourseIds = currentCourse
                    .Students
                    .Select(s => s.StudentId)
                    .ToList();
                for (int j = 0; j < studentsInCourseIds.Count; j++)
                {
                    var totalHomework = random.Next(2, 5);
                    for (int k = 0; k < totalHomework; k++)
                    {
                        context.Homeworks.Add(new Homework
                        {
                            Content = $"Content Homework {i}",
                            SubmissionDate = currentDate.AddDays(-i),
                            ContentType = ContentType.Zip,
                            StudentId = studentsInCourseIds[j],
                            CourseId = currentCourse.Id
                        });
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
