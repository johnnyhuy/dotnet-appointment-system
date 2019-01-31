using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rmit.Asr.Application.Controllers;
using Rmit.Asr.Application.Controllers.Api;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Tests.Controllers
{
    public class ControllerBaseTest : IDisposable
    {
        protected const string StaffId = "e12345";
        protected const string StaffEmail = "e12345@rmit.edu.au";
        protected const string StaffUsername = StaffEmail;
        protected const string StudentId = "s1234567";
        protected const string StudentEmail = "s1234567@student.rmit.edu.au";
        protected const string StudentUsername = StudentEmail;
        
        protected readonly ApplicationDataContext Context;
        protected SlotController SlotController;
        protected SlotApiController ApiSlotController;
        protected RoomApiController ApiRoomController;
        protected Staff Staff;
        protected Student Student;
        protected Room RoomA;
        protected Room RoomB;
        protected Room RoomC;
        protected Room RoomD;

        protected ControllerBaseTest()
        {
            DbContextOptions options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            Context = new ApplicationDataContext(options);
            
            Staff = new Staff
            {
                Id = Guid.NewGuid().ToString(),
                StaffId = StaffId,
                Email = StaffEmail,
                FirstName = "Shawn",
                LastName = "Taylor",
                UserName = StaffUsername
            };
            Student = new Student
            {
                Id = Guid.NewGuid().ToString(),
                StudentId = StudentId,
                Email = StudentEmail,
                FirstName = "Shawn",
                LastName = "Taylor",
                UserName = StudentUsername
            };
            
            var mockStaffStore = new Mock<IUserStore<Staff>>();
            mockStaffStore.Setup(x => x.FindByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(Staff);
            var staffManager = new UserManager<Staff>(mockStaffStore.Object, null, null, null, null, null, null, null, null);
            
            var mockStudentStore = new Mock<IUserStore<Student>>();
            mockStudentStore.Setup(x => x.FindByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(Student);
            var studentManager = new UserManager<Student>(mockStudentStore.Object, null, null, null, null, null, null, null, null);

            SlotController = new SlotController(Context, staffManager, studentManager);
            ApiSlotController = new SlotApiController(Context);
            ApiRoomController = new RoomApiController(Context);
            
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            SlotController.TempData = tempData;

            Seed();
        }
        
        public void Dispose()
        {
            Context.Dispose();
        }

        private void Seed()
        {
            if (Context.Room.Any() || Context.Slot.Any() || Context.Users.Any())
            {
                return;
            }

            RoomA = new Room
            {
                Name = "A"
            };
            RoomB = new Room
            {
                Name = "B"
            };
            RoomC = new Room
            {
                Name = "C"
            };
            RoomD = new Room
            {
                Name = "D"
            };
            
            Context.Room.AddRange(RoomA, RoomB, RoomC, RoomD);
            
            Context.Staff.AddRange(
                Staff,
                new Staff
                {
                    StaffId = "e54321",
                    FirstName = "Bob",
                    LastName = "Doe",
                    Email = "e54321@rmit.edu.au"
                }
            );
            
            Context.Student.AddRange(
                Student,
                new Student
                {
                    StudentId = "s3604367",
                    FirstName = "Johnny",
                    LastName = "Doe",
                    Email = "s3604367@student.rmit.edu.au"
                }
            );

            Context.SaveChanges();
        }
        
        /// <summary>
        /// Set the HTTP context user in the controller.
        /// </summary>
        /// <param name="username"></param>
        protected void UserLoggedIn(string username)
        {
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username)
            }));
            
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext {User = userPrincipal}
            };

            SlotController.ControllerContext = controllerContext;
        }
    }
}