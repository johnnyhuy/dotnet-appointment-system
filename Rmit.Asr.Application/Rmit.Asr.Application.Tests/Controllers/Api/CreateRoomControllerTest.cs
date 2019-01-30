using System.Linq;
using System.Net;
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
                RoomId = "G"
            };

            // Act
            dynamic result = ApiRoomController.Create(room);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Room.Any(r => r.RoomId == room.RoomId));
        }
        
        [Fact]
        public void CreateRoom_WithAlreadyExistRoom_ReturnNotFound()
        {
            // Arrange
            var room = new Room
            {
                RoomId = "A"
            };

            // Act
            dynamic result = ApiRoomController.Create(room);

            // Assert
            Assert.IsAssignableFrom<JsonResult>(result);
            Assert.Equal("Room already exists.", result.Value.Message);
            Assert.Equal((int) HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}