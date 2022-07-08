using Microsoft.AspNetCore.Identity;

namespace ESTUDO.API.Models
{
    /// <summary>
    /// Enrollment entity, links a user to a course.
    /// </summary>
    public class Enrollment
    {
        /// <summary>
        /// Unique Id for the enrollment.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id to the course the student is enrolled in.
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// Course the student is enrolled in.
        /// </summary>
        public Course Course { get; set; }
        /// <summary>
        /// Unique Id for the enrolled student.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Enrolled student's full name.
        /// </summary>
        public string UserFullName { get; set; }
    }
}