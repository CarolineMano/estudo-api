using System.ComponentModel.DataAnnotations;

namespace ESTUDO.API.DTO
{
    /// <summary>
    /// Data transfer object for a new course.
    /// </summary>
    public class CourseDto
    {
        /// <summary>
        /// Course Name.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Must be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }
        /// <summary>
        /// Course load.
        /// </summary>
        [Required]
        [Range(1.0, float.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public int CourseLoad { get; set; }
        /// <summary>
        /// Course price, can be set to zero initially.
        /// </summary>
        [Required]
        [Range(0.0, float.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public float Price { get; set; }
    }
}