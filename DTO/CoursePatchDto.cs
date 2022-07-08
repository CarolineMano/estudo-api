using System.ComponentModel.DataAnnotations;

namespace ESTUDO.API.DTO
{
    /// <summary>
    /// Data transfer object for a course edition.
    /// </summary>
    public class CoursePatchDto
    {
        /// <summary>
        /// New course name.
        /// </summary>
        [StringLength(50, ErrorMessage = "Must be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }
        /// <summary>
        /// New course load.
        /// </summary>
        public int CourseLoad { get; set; }
        /// <summary>
        /// New course price.
        /// </summary>
        [Range(0.0, float.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public float Price { get; set; }        
    }
}