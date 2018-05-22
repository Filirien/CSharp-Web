namespace StudentSystem.Data
{
	using System;
    using System.ComponentModel.DataAnnotations;

    public class Resource
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public TypeOfResource ResourceType { get; set; }

        [Required]
        public string Url { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
