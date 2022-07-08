using ESTUDO.API.HATEOAS;
using ESTUDO.API.Models;

namespace ESTUDO.API.Container
{
    /// <summary>
    /// Class used to implement HATEOAS for an enrollment.
    /// </summary>
    public class EnrollmentContainer
    {
        /// <summary>
        /// Enrollment entity.
        /// </summary>
        public Enrollment Enrollment { get; set; }  
        /// <summary>
        /// Links to be returned with the enrollment.
        /// </summary>
        public Link[] Links { get; set; }
    }
}