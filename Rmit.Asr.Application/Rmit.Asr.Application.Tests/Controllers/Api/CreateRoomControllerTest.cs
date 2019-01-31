using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class CreateRoomControllerTest : ControllerBaseTest
    {
        [Fact]
        public void CreateRoom_BookStudent_ReturnOk()
        {
            // Arrange
            var room = new Room
            {
                Name = "G"
            };

            // Act
            ActionResult result = ApiRoomController.Create(room);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Room.Any(r => r.Name == room.Name));
        }
        
        [Fact]
        public void CreateRoom_WithAlreadyExistRoom_ReturnNotFound()
        {
            // Arrange
            var room = new Room
            {
                Name = "A"
            };

            // Act
            ActionResult result = ApiRoomController.Create(room);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Room already exists.", errors);
        }
    }
}