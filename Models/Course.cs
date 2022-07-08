using System.ComponentModel.DataAnnotations.Schema;

namespace ESTUDO.API.Models
{
    /// <summary>
    /// Course entity, has a relationship to an instructor (faculty).
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Unique Id for the course.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Course name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Course load in hours.
        /// </summary>
        public int CourseLoad { get; set; }
        /// <summary>
        /// Course price.
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// Unique Id for the instructor of the course.
        /// </summary>
        public string InstructorId { get; set; }
        /// <summary>
        /// Instructor's full name.
        /// </summary>
        public string InstructorName { get; set; }
    }
}