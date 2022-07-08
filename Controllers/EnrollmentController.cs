using System;
using System.Collections.Generic;
using System.Linq;
using ESTUDO.API.Container;
using ESTUDO.API.Data;
using ESTUDO.API.DTO;
using ESTUDO.API.HATEOAS;
using ESTUDO.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESTUDO.API.Controllers
{
    /// <summary>
    /// Controller responsible for manipulating enrollments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class EnrollmentController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly LinkHelper _hateoas;
        /// <summary>
        /// Constructor for the enrollment controller, it injects the database and adds HATEOAS.
        /// </summary>
        public EnrollmentController(ApplicationDbContext database)
        {
            _database = database;
            _hateoas = new LinkHelper("localhost:5001/api/Enrollment");
            _hateoas.AddActionById("UNENROLL", "DELETE");
            _hateoas.AddActionById("GET_INFO", "GET");
            _hateoas.AddGeneralActions("LIST_ALL", "GET");
            _hateoas.AddGeneralActions("POST_NEW", "POST");
        }
        /// <summary>
        /// Allows a student to see all of their enrollments.
        /// </summary>
        [HttpGet]
        public IActionResult ListAll()
        {
            var loggedUserId = GetLoggedUserId();

            var enrollmentsFromDb = _database.Enrollments.Where(enroll => enroll.UserId == loggedUserId).Include(enroll => enroll.Course).ToList();

            List<EnrollmentContainer> enrollmentsHateoas = new List<EnrollmentContainer>();

            foreach (var enroll in enrollmentsFromDb)
            {
                EnrollmentContainer enrollWithLinks = new EnrollmentContainer();
                enrollWithLinks.Enrollment = enroll;
                enrollWithLinks.Links = _hateoas.GetActionsById(enroll.Id.ToString());
                enrollmentsHateoas.Add(enrollWithLinks);
            }

            return Ok(enrollmentsHateoas.ToArray());
        }
        /// <summary>
        /// Allows a student to see one of their enrollments.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var loggedUserId = GetLoggedUserId();
            try
            {
                var enrollmentFromDb = _database.Enrollments.Include(enroll => enroll.Course).First(enroll => enroll.Id == id && enroll.UserId == loggedUserId);

                EnrollmentContainer enrollWithLinks = new EnrollmentContainer();

                enrollWithLinks.Enrollment = enrollmentFromDb;
                enrollWithLinks.Links = _hateoas.GetActionsById(id.ToString());

                return Ok(enrollWithLinks);
            }
            catch (System.Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Enrollment Id not found for this user");
            }            
        }
        /// <summary>
        /// Allows a student to enroll in one new course.
        /// </summary>
        [HttpPost]
        public IActionResult Enroll([FromBody] EnrollmentDto model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var loggedUserId = GetLoggedUserId();

                    var courseFromDb = _database.Courses.First(course => course.Id == model.CourseId);

                    var existingEnroll = _database.Enrollments.FirstOrDefault(enroll => enroll.CourseId == courseFromDb.Id && enroll.UserId == loggedUserId);

                    if(existingEnroll != null)
                    {
                        Response.StatusCode = 409;
                        return new ObjectResult("You are already enrolled in this course");
                    }
                    
                    Enrollment newEnroll = new Enrollment();
                    
                    newEnroll.CourseId = model.CourseId;
                    newEnroll.UserId = loggedUserId;
                    newEnroll.UserFullName = _database.Users.First(user => user.Id == loggedUserId).FullName;

                    _database.Enrollments.Add(newEnroll);
                    _database.SaveChanges();

                    EnrollmentContainer enrollWithLinks = new EnrollmentContainer();
                    enrollWithLinks.Enrollment = newEnroll;
                    enrollWithLinks.Links = _hateoas.GetActionsById(newEnroll.Id.ToString());

                    return Ok(enrollWithLinks);
                }
                catch (System.Exception)
                {
                    Response.StatusCode = 404;
                    return new ObjectResult("Course Id not found");
                }
            }
            return BadRequest(); 
        }
        /// <summary>
        /// Allows a student to unenroll from one of their courses.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Unenroll(int id)
        {
            var loggedUserId = GetLoggedUserId();
            try
            {
                var enrollFromDb = _database.Enrollments.Include(enroll => enroll.Course).First(enroll => enroll.Id == id && enroll.UserId == loggedUserId);

                _database.Remove(enrollFromDb);
                _database.SaveChanges();

                EnrollmentContainer enrollWithLinks = new EnrollmentContainer();
                enrollWithLinks.Enrollment = enrollFromDb;
                enrollWithLinks.Links = _hateoas.GetGeneralActions();

                return Ok(new{msg="Unenrolled", enrollWithLinks});
            }
            catch (System.Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Enrollmment Id not found for this user");
            }
            
        }
        private string GetLoggedUserId()
        {
            return HttpContext.User.Claims.First(claim => claim.Type.ToString().Equals("id", System.StringComparison.InvariantCultureIgnoreCase)).Value;
        }
        
    }
}