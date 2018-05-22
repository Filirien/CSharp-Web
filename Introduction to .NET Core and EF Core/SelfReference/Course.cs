namespace ManyToMany
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int StudentId { get; set; }
        public ICollection<StudentsCourses> CoursesStudents { get; set; }
    }
}
