using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class PutRoomControllerTest : ControllerBaseTest
    {
        [Fact]
        public async void PutRoom_BookStudent_ReturnOk()
        {
            // Arrange
            var room = new Room
            {
                Name = "P"
            };

            Context.Room.Add(room);

            await Context.SaveChangesAsync();

            // Act
            ActionResult result = ApiRoomController.Put(room.Name, room);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Room.Any(r => r.Name == room.Name));
        }
        
        [Fact]
        public async void PutRoom_WithNonExistentRoom_ReturnBadRequest()
        {
            // Arrange
            var room = new Room
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
            var room = new Room
            {
                Name = "P"
            };

            Context.Room.Add(room);

            await Context.SaveChangesAsync();

            // Act
            ActionResult result = ApiRoomController.Put("A", room);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Cannot update the room since the room already exists.", errors);
            
            Assert.True(Context.Room.Any(r => r.Name == room.Name));
        }
    }
}