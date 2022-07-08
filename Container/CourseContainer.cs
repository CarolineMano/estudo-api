using ESTUDO.API.HATEOAS;
using ESTUDO.API.Models;

namespace ESTUDO.API.Container
{
    /// <summary>
    /// Class used to implement HATEOAS for a course.
    /// </summary>
    public class CourseContainer
    {
        /// <summary>
        /// Course entity.
        /// </summary>
        public Course Course { get; set; }
        /// <summary>
        /// Links to be returned with the course.
        /// </summary>
        public Link[] Links { get; set; }
    }
}