using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models.ViewModels;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class PutRoomControllerTest : ControllerBaseTest
    {
        [Fact]
        public void PutRoom_BookStudent_ReturnOk()
        {
            // Arrange
            var room = new UpdateRoom
            {
                Name = "ZZZZ"
            };

            // Act
            ActionResult result = ApiRoomController.Put(RoomA.Name, room);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Room.Any(r => r.Name == room.Name && r.Id == RoomA.Id));
        }
        
        [Fact]
        public async void PutRoom_WithNonExistentRoom_ReturnBadRequest()
        {
            // Arrange
            var room = new UpdateRoom
            {
                Name = "P"
            };

            Context.Room.Add(room);

            await Context.SaveChangesAsync();

            // Act
            ActionResult result = ApiRoomController.Put("Z", room);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Room does not exist.", errors);
            
            Assert.True(Context.Room.Any(r => r.Name == room.Name));
        }
        
        [Fact]
        public async void PutRoom_WithAlreadyExistingRoom_ReturnBadRequest()
        {
            // Arrange
            var room = new UpdateRoom
            {
                Name = "P"
            };

            Context.Room.Add(room);

            await Context.SaveChangesAsync();

            // Act
            ActionResult result = ApiRoomController.Put(RoomA.Name, room);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Cannot update the room since the room already exists.", errors);
            
            Assert.False(Context.Room.Any(r => r.Name == room.Name && r.Id == RoomA.Id));
        }
    }
}