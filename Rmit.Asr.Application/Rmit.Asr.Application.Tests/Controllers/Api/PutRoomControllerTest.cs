using System.Dynamic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class PutRoomControllerTest : ControllerBaseTest
    {
        // TODO: Remove room ID as key
        [Fact(Skip = "Need remove room ID as key to fix this.")]
        public async void PutRoom_BookStudent_ReturnOk()
        {
            // Arrange
            var room = new Room
            {
                RoomId = "P"
            };

            Context.Room.Add(room);

            await Context.SaveChangesAsync();

            dynamic values = new ExpandoObject();
            values.RoomId = new ExpandoObject();
            values.RoomId.Value = "G";

            // Act
            dynamic result = ApiRoomController.Put(room.RoomId, values);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Room.Any(r => r.RoomId == room.RoomId));
        }
        
        [Fact]
        public async void PutRoom_WithNonExistentRoom_ReturnNotFound()
        {
            // Arrange
            var room = new Room
            {
                RoomId = "P"
            };

            Context.Room.Add(room);

            await Context.SaveChangesAsync();

            dynamic values = new ExpandoObject();
            values.StudentId = new ExpandoObject();

            // Act
            dynamic result = ApiRoomController.Put("Z", values);

            // Assert
            Assert.IsAssignableFrom<JsonResult>(result);
            Assert.Equal("Room does not exist.", result.Value.Message);
            Assert.Equal((int) HttpStatusCode.NotFound, result.StatusCode);
            
            Assert.True(Context.Room.Any(r => r.RoomId == room.RoomId));
        }
    }
}