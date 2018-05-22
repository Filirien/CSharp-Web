namespace StudentSystem.Data
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        public List<Homework> HomeworkSubmissions { get; set; } = new List<Homework>();

        public List<StudentsCourses> Students { get; set; } = new List<StudentsCourses>();

        public List<Resource> Resources { get; set; } = new List<Resource>();
    }
}
