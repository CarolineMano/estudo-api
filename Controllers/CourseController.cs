using System.Collections.Generic;
using System.Linq;
using ESTUDO.API.Container;
using ESTUDO.API.Data;
using ESTUDO.API.DTO;
using ESTUDO.API.HATEOAS;
using ESTUDO.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESTUDO.API.Controllers
{
    /// <summary>
    /// Controller responsible for manipulating (CRUD) courses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Faculty")]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly LinkHelper _hateoas;
        /// <summary>
        /// Constructor for the course controller, it injects the database and adds HATEOAS.
        /// </summary>
        public CourseController(ApplicationDbContext database)
        {
            _database = database;
            _hateoas = new LinkHelper("localhost:5001/api/Course");
            _hateoas.AddActionById("DELETE_COURSE", "DELETE");
            _hateoas.AddActionById("EDIT_COURSE", "PATCH");
            _hateoas.AddActionById("GET_INFO", "GET");
            _hateoas.AddGeneralActions("GET_ALL", "GET");
            _hateoas.AddGeneralActions("POST_NEW", "POST");
        }
        /// <summary>
        /// Returns every course in the database, even to anonymous users.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListAll()
        {
            List<CourseContainer> coursesHateoas = new List<CourseContainer>();

            var coursesFromDb = _database.Courses.ToList();

            foreach (var course in coursesFromDb)
            {
                CourseContainer courseWithLinks = new CourseContainer();
                courseWithLinks.Course = course;
                courseWithLinks.Links = _hateoas.GetActionsById(course.Id.ToString());
                coursesHateoas.Add(courseWithLinks);
            }

            return Ok(coursesHateoas.ToArray());
        }
        /// <summary>
        /// Returns one specific course from the database, even to anonymous users.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetOne(int id)
        {
            try
            {
                var course = _database.Courses.First(course => course.Id == id);

                CourseContainer courseWithLinks = new CourseContainer();

                courseWithLinks.Course = course;
                courseWithLinks.Links = _hateoas.GetActionsById(id.ToString());

                return Ok(courseWithLinks);
            }
            catch (System.Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Course not found");
            }            
        }
        /// <summary>
        /// Allows a faculty member to add a new course.
        /// </summary>
        [HttpPost]
        public IActionResult NewCourse([FromBody] CourseDto model)
        {
            if(ModelState.IsValid)
            {
                var loggedInInstructor = GetLoggedUserId();
                Course newCourse = new Course();
                newCourse.Name = model.Name;
                newCourse.CourseLoad = model.CourseLoad;
                newCourse.Price = model.Price;
                newCourse.InstructorId = loggedInInstructor;
                newCourse.InstructorName = _database.Users.First(user => user.Id == loggedInInstructor).FullName;

                _database.Courses.Add(newCourse);
                _database.SaveChanges();

                CourseContainer courseWithLinks = new CourseContainer();

                courseWithLinks.Course = newCourse;
                courseWithLinks.Links = _hateoas.GetActionsById(newCourse.Id.ToString());

                Response.StatusCode = 201;
                return new ObjectResult(courseWithLinks);
            }
            return BadRequest();            
        }
        /// <summary>
        /// Allows a faculty member to delete one of their own courses.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var loggedUserId = GetLoggedUserId();            
            try
            {
                var courseFromDb = _database.Courses.First(course => course.Id == id && course.InstructorId == loggedUserId);

                var existingEnroll = _database.Enrollments.FirstOrDefault(enroll => enroll.CourseId == courseFromDb.Id);

                if(existingEnroll != null)
                {
                    Response.StatusCode = 409;
                    return new ObjectResult("There are students enrolled in this course");
                }
                _database.Courses.Remove(courseFromDb);
                _database.SaveChanges();

                CourseContainer courseWithLinks = new CourseContainer();

                courseWithLinks.Course = courseFromDb;
                courseWithLinks.Links = _hateoas.GetGeneralActions();

                return Ok(new {msg = "Course deleted", course = courseWithLinks});
            }
            catch (System.Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Course not found for the current instructor");                
            }
        }
        /// <summary>
        /// Allows a faculty member to edit one of their own courses.
        /// </summary>
        [HttpPatch("{id}")]
        public IActionResult EditCourse([FromBody] CoursePatchDto model, int id)
        {
            var loggedUserId = GetLoggedUserId();
            try
            {
                var courseFromDb = _database.Courses.First(course => course.Id == id && course.InstructorId == loggedUserId);

                courseFromDb.Name = model.Name != null ? model.Name : courseFromDb.Name;
                courseFromDb.CourseLoad = model.CourseLoad >= 1 ? model.CourseLoad : courseFromDb.CourseLoad;
                courseFromDb.Price = model.Price > 0 ? model.Price : courseFromDb.Price;

                CourseContainer courseWithLinks = new CourseContainer();

                courseWithLinks.Course = courseFromDb;
                courseWithLinks.Links = _hateoas.GetActionsById(courseFromDb.Id.ToString());


                _database.SaveChanges();
                return Ok(courseWithLinks); 
            }
            catch (System.Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Course not found for the current instructor");    
            }
        }
        private string GetLoggedUserId()
        {
            return HttpContext.User.Claims.First(claim => claim.Type.ToString().Equals("id", System.StringComparison.InvariantCultureIgnoreCase)).Value;
        }
    }
}