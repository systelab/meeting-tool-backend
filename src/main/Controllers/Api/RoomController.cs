namespace Main.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;

    using Main.Models;
    using Main.Services;
    using Main.ViewModels;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [EnableCors("MyPolicy")]
    [Route("api/room")]
    public class RoomController : Controller
    {
        private readonly IMeetingRepository repository;
        private readonly ILogger<MeetingController> logger;

        private readonly IMapper mapper;

        public RoomController( IMeetingRepository repository, ILogger<MeetingController> logger, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Create a new room
        /// </summary>
        /// <param name="room">Room model</param>
        /// <returns></returns>  
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomViewModel room)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Bad data");
            }

            try
            {
                this.repository.AddRoom(this.mapper.Map<Room>(room));
                return this.Ok("Done");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to create the room: {ex}");
                return this.BadRequest("Error Occurred");
            }


        }

        /// <summary>
        /// Get list of Rooms
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllRooms(int id)
        {
            try
            {
                if (id == 0)
                {
                    var results = this.repository.GetAllRooms();
                    return this.Ok(results);
                }
                else
                {
                    Room itmv = this.repository.GetRoom(new Room { Id = id });
                    return this.Ok(this.mapper.Map<RoomViewModel>(itmv));
                }
                
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get the room: {ex}");
                return this.BadRequest("Error Occurred");
            }
        }
        /// <summary>
        /// Get the availability of the rooms
        /// </summary>
        /// <returns></returns>
        [Route("availability")]
        [HttpGet]
        public IActionResult GetAvailabilityOfRooms()
        {
            try
            {
                    var results = this.repository.GetAvailabilityOfRooms();
                    return this.Ok(results);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get the availability of the rooms: {ex}");
                return this.BadRequest("Error Occurred");
            }
        }

        /// <summary>
        /// Remove a specific room
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>

        [HttpDelete]
        public async Task<IActionResult> RemoveRoom(int id)
        {
            try
            {
                if (id == 0 )
                {
                    return this.BadRequest("Bad data");
                }
                else
                {
                    Room itmv = this.repository.GetRoom(new Room { Id = id });
                    if (itmv == null)
                    {
                        return this.GetAllRooms(-1);
                    }
                    itmv.Deleted = true;
                    this.repository.DeleteRoom(itmv);
                    return this.Ok("Done");

                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed delete category: {ex}");
                return this.BadRequest("Error Occurred");
            }
        }

        /// <summary>
        /// Update the information of an existing room
        /// </summary>
        /// <param name="room">item model</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomViewModel room)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Bad data");
            }

            Room results = this.repository.GetRoom(new Room { Id = room.Id });
            if (results == null || results.Id == 0)
            {
                return this.BadRequest("User does not exist");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(room.Name))
                {
                    results.Name = room.Name;
                }
                this.repository.UpdateRoom(results);
                return this.Ok(results);
            }

        }
    }
}