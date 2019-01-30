using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class CreateRoomControllerTest : ControllerBaseTest
    {
        [Fact]
        public void CreateRoom_BookStudent_ReturnOk()
        {
            // Arrange
            const string roomId = "G";
            
            dynamic values = new ExpandoObject();
            values.RoomId = new ExpandoObject();
            values.RoomId.Value = roomId;

            // Act
            dynamic result = ApiRoomController.Create(values);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Room.Any(r => r.RoomId == roomId));
        }
    }
}