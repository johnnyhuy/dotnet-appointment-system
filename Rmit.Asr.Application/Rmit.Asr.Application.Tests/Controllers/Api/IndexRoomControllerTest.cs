using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class IndexRoomControllerTest : ControllerBaseTest
    {
        [Fact]
        public void IndexRoom_WithValidParameters_ReturnRooms()
        {
            // Arrange
            List<Room> rooms = Context.Room.ToList();

            // Act
            ActionResult<IEnumerable<Room>> result = ApiRoomController.Index();

            // Assert
            Assert.Equal(rooms.Count, result.Value.Count());
        }
    }
}