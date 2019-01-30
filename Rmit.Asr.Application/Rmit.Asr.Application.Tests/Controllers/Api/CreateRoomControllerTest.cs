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
                RoomId = "G"
            };

            // Act
            dynamic result = ApiRoomController.Create(room);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Room.Any(r => r.RoomId == room.RoomId));
        }
    }
}