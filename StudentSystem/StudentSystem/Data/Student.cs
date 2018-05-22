namespace StudentSystem.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public DateTime? Birthday { get; set; }


        public List<Homework> HomeworkSubmissions { get; set; } = new List<Homework>();

        public List<StudentsCourses> Courses { get; set; } = new List<StudentsCourses>();
    }
}
