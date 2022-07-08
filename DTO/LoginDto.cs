using System.ComponentModel.DataAnnotations;

namespace ESTUDO.API.DTO
{
    /// <summary>
    /// Data transfer object for a new login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email of the registered user.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        /// <summary>
        /// Password of the registered user.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}