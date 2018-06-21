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
    using main.ViewModels;
    using Main.Models;
    using Main.Services;
    using Main.ViewModels;


    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [EnableCors("MyPolicy")]
    [Route("api/meetings")]
    public class MeetingController : Controller
    {
        private readonly IMeetingRepository repository;
        private readonly ILogger<MeetingController> logger;

        public MeetingController( IMeetingRepository repository, ILogger<MeetingController> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get list of meetings
        /// </summary>
        /// <returns>result of the action</returns>
        [HttpGet]
        public IActionResult GetMeetings(int idMeeting,int idRoom)
        {
            try
            {
                if (idMeeting == 0 && idRoom == 0)
                {
                    var results = this.repository.GetAllMeetings();
                    return this.Ok(results);
                }
                else if (idMeeting > 0)
                {
                    MeetingViewModel itmv = this.repository.GetMeeting(new Meeting { Id = idMeeting });
                    return this.Ok(itmv);
                }
                else if (idRoom > 0)
                {
                    var results = this.repository.GetAllMeetingsByRoom(idRoom);
                    return this.Ok(results);
                }
                else
                {
                    return this.BadRequest("Not correct values provided");
                }
                
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get the the meetigns: {ex}");
                return this.BadRequest("Error Occurred");
            }
        }

     /// <summary>
     /// Check if needed to update the meetings info
     /// </summary>
     /// <returns></returns>
        [HttpPost]
        public IActionResult CheckUpdate()
        {
            try
            {
                var itmv = this.repository.GeLastCheckUpdate();
                if(itmv == null)
                {
                    var results = this.repository.ExecSyncMeetings();
                    CheckUpdate nChe = new CheckUpdate { Dob = DateTime.Now };
                    this.repository.AddCheckUpdate(nChe);
                    return this.Ok(results);
                }
                else
                {
                    DateTime dt = itmv.Dob.AddMinutes(5);
                    if (dt < DateTime.Now)
                    {
                        var results = this.repository.ExecSyncMeetings();
                        itmv.Dob = DateTime.Now;
                        this.repository.UpdateCheckUpdate(itmv);
                        return this.Ok(results);
                    }
                    else
                    {
                        return this.Ok("Wait a minutes...");
                    }
                }

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get the the meetigns: {ex}");
                return this.BadRequest("Error Occurred");
            }
        }

        /// <summary>
        /// Do quick reservation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("reservation")]
        public IActionResult DoReservation(EventViewModel meeting)
        {
            try
            {
                Room r1 = new Room
                {
                    Id = meeting.idRoom
                };
                Room room = this.repository.GetRoom(r1);

                Task<HttpResponseMessage> response =  this.repository.DoReservation(room.email, meeting.Start, meeting.End);
                if(response.Result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var results = this.repository.ExecSyncMeetings();
                    CheckUpdate nChe = new CheckUpdate { Dob = DateTime.Now };
                    this.repository.AddCheckUpdate(nChe);
                }
                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed booking the meeting: {ex}");
                return this.BadRequest("Error Occurred");
            }
        }

    }
}