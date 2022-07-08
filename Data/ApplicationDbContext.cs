using System;
using ESTUDO.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESTUDO.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUsers(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            User user1 = new User() 
            { 
                Id = Guid.NewGuid().ToString(),
                UserName = "joel@email.com", 
                Email = "joel@email.com", 
                NormalizedEmail = "JOEL@EMAIL.COM",
                NormalizedUserName = "JOEL@EMAIL.COM",
                PasswordHash = hasher.HashPassword(null, "MyPass_w0rd"), 
                EmailConfirmed = true,
                GivenName = "Joel", 
                Surname = "Flores"
            };

            User user2 = new User() 
            { 
                Id = Guid.NewGuid().ToString(),
                UserName = "jacques@email.com", 
                Email = "jacques@email.com",
                NormalizedEmail = "JACQUES@EMAIL.COM",
                NormalizedUserName = "JACQUES@EMAIL.COM",
                PasswordHash = hasher.HashPassword(null, "MyPass_w0rd"),  
                EmailConfirmed = true,
                GivenName = "Jacques", 
                Surname = "Woodard"
            };

            builder.Entity<User>().HasData(user1);
            builder.Entity<User>().HasData(user2);

            IdentityRole role1 = new IdentityRole()
            {
                Name = "Faculty",
                NormalizedName = "FACULTY"
            };  

            IdentityRole role2 = new IdentityRole()
            {
                Name = "Student",
                NormalizedName = "STUDENT"
            };

            builder.Entity<IdentityRole>().HasData(role1);
            builder.Entity<IdentityRole>().HasData(role2);

            IdentityUserRole<string> userRole1 = new IdentityUserRole<string>()
            {
                RoleId = role1.Id,
                UserId = user1.Id
            };

            IdentityUserRole<string> userRole2 = new IdentityUserRole<string>()
            {
                RoleId = role2.Id,
                UserId = user2.Id
            };
            
            builder.Entity<IdentityUserRole<string>>().HasData(userRole1);
            builder.Entity<IdentityUserRole<string>>().HasData(userRole2);

            Course course1 = new Course()
            {
                Id = 1,
                Name = ".NET",
                CourseLoad = 20,
                Price = 48.99f,
                InstructorId = user1.Id,
                InstructorName = user1.FullName
            };

            builder.Entity<Course>().HasData(course1);

            Enrollment enrollment1 = new Enrollment()
            {
                Id = 1,
                CourseId = 1,
                UserId = user2.Id,
                UserFullName = user2.FullName
            };

            builder.Entity<Enrollment>().HasData(enrollment1);
        }
    }
}