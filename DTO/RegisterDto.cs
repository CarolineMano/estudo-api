using System.ComponentModel.DataAnnotations;

namespace ESTUDO.API.DTO
{
    /// <summary>
    /// Data transfer object for a new user.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Password for the new user.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// Password confirmation for the new user.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// New user's email address.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        /// <summary>
        /// New user's surname.
        /// </summary>
        [Required]
        [MinLength(2, ErrorMessage = "Must be at least {0} characters long")]
        public string Surname { get; set; }
        /// <summary>
        /// New user's given name.
        /// </summary>
        [Required]
        [MinLength(2, ErrorMessage = "Must be at least {0} characters long")]
        public string GivenName { get; set; }
        /// <summary>
        /// New user's role. At first, it can only be student or faculty.
        /// </summary>
        [Required]
        public string Role { get; set; }
    }
}