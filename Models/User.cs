using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ESTUDO.API.Models
{
    /// <summary>
    /// User entity, inherits from IdentityUser
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// User's surname
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// User's given name
        /// </summary>
        public string GivenName { get; set; }
        /// <summary>
        /// User's full name, not mapped to the database
        /// </summary>
        [NotMapped]
        public string FullName => $"{GivenName} {Surname}";
    }
}