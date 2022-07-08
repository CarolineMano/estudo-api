using System.ComponentModel.DataAnnotations;

namespace ESTUDO.API.DTO
{
    /// <summary>
    /// Data transfer object for a new enrollment.
    /// </summary>
    public class EnrollmentDto
    {
        /// <summary>
        /// Course id for the target course.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public int CourseId { get; set; }
    }
}