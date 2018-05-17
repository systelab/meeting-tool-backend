namespace Main.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Main.Models;
    using Main.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Repository with all the queries to the database using the entity framework
    /// </summary>
    public class MeetingRepository : IMeetingRepository
    {
        private readonly MeetingsContext context;
        private readonly IConfigurationRoot config;
        /// <summary>
        /// Set the context of the app
        /// </summary>
        /// <param name="_context"></param>
        public MeetingRepository(IConfigurationRoot config, MeetingsContext _context)
        {
            this.context = _context;
            this.config = config;
        }


        /// <summary>
        /// Insert the meeting into the database
        /// </summary>
        /// <param name="newMeeting">Object with the information of the meeting that you want to insert</param>
        public void AddMeeting(Meeting newMeeting)
        {
            this.context.Add(newMeeting);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Delete a Meeting because it was anulated
        /// </summary>
        /// <param name="newMeeting"></param>
        public void DelMeeting(Meeting newMeeting)
        {
            this.context.Entry(newMeeting).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            this.context.SaveChanges();
        }

        /// <summary>
        /// List all the meetings saved in the database
        /// </summary>
        /// <returns>List of meeting object</returns>
        public object GetAllMeetings()
        {
            var itemslist = (from t1 in this.context.Meetings
                            join t2 in this.context.Rooms on t1.IdRoom equals t2.Id
                             orderby t1.StartDateTime ascending
                            select new { IdMeeting = t1.Id, t1.IdRoom, t1.StartDateTime, t1.Who, t1.EndDateTime, t1.IdLotus, RoomName = t2.Name, t2.Deleted}).Where(x => x.Deleted == false);
            return itemslist;
        }
        /// <summary>
        /// List all the meetings for a room for today
        /// </summary>
        /// <returns>List of items object</returns>
        public object GetAllMeetingsByRoom(int idRoom)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dtEnd = dtStart.AddDays(1);
            var itemslist = (from t1 in this.context.Meetings
                             join t2 in this.context.Rooms on t1.IdRoom equals t2.Id
                             orderby t1.StartDateTime ascending
                             select new { IdMeeting = t1.Id, t1.IdRoom, t1.StartDateTime, t1.Who, t1.EndDateTime, t1.IdLotus, RoomName = t2.Name, t2.Deleted, })
                             .Where(x => x.Deleted == false).Where(x => x.IdRoom == idRoom)
                             .Where(x => DateTime.Compare(x.StartDateTime, dtStart) > 0)
                             .Where(x => DateTime.Compare(x.StartDateTime,dtEnd) < 0);

            return itemslist.ToList();
        }
        /// <summary>
        /// Return the meeting for today
        /// </summary>
        /// <returns></returns>
        public List<Meeting> GetAllMeetingsToday()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dtEnd = dtStart.AddDays(1);
            var itemslist = (from t1 in this.context.Meetings
                             join t2 in this.context.Rooms on t1.IdRoom equals t2.Id
                             orderby t1.StartDateTime ascending
                             select new { IdMeeting = t1.Id, t1.IdRoom, t1.StartDateTime, t1.Who, t1.EndDateTime, t1.IdLotus, RoomName = t2.Name, t2.Deleted })
                             .Where(x => x.Deleted == false);

            List<Meeting> listmeetings = new List<Meeting>();
            foreach (var result in itemslist)
            {
                Meeting it1 = new Meeting
                {
                    EndDateTime = result.EndDateTime,
                    Who = result.Who,
                    StartDateTime = result.StartDateTime,
                    Id = result.IdMeeting,
                    IdLotus = result.IdLotus,
                    IdRoom = result.IdRoom
                };
                listmeetings.Add(it1);
            }
            return listmeetings;
        }
        /// <summary>
        /// Return the meetings not checked
        /// </summary>
        /// <returns></returns>
        public List<Meeting> GetAllMeetingsTodayNotChecked()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dtEnd = dtStart.AddDays(1);
            var itemslist = (from t1 in this.context.Meetings
                             join t2 in this.context.Rooms on t1.IdRoom equals t2.Id
                             orderby t1.StartDateTime ascending
                             select new { IdMeeting = t1.Id, t1.IdRoom, t1.StartDateTime, t1.Who, t1.EndDateTime, t1.IdLotus, RoomName = t2.Name, t2.Deleted,t1.check })
                             .Where(x => x.Deleted == false)
                             .Where(x => DateTime.Compare(x.StartDateTime, dtStart) > 0)
                             .Where(x => DateTime.Compare(x.StartDateTime, dtEnd) < 0)
                             .Where(x => x.check == false);

            List<Meeting> listmeetings = new List<Meeting>();
            foreach (var result in itemslist)
            {
                Meeting it1 = new Meeting
                {
                    EndDateTime = result.EndDateTime,
                    Who = result.Who,
                    StartDateTime = result.StartDateTime,
                    Id = result.IdMeeting,
                    IdLotus = result.IdLotus,
                    IdRoom = result.IdRoom
                };
                listmeetings.Add(it1);
            }
            return listmeetings;
        }
        

        /// <summary>
        /// Get a specific Meeting
        /// </summary>
        /// <param name="nMeeting"></param>
        /// <returns></returns>
        public MeetingViewModel GetMeeting(Meeting nMeeting)
        {
            var itemslist = (from t1 in this.context.Meetings
                             join t2 in this.context.Rooms on t1.IdRoom equals t2.Id
                             orderby t1.StartDateTime descending
                             select new { IdMeeting = t1.Id, t1.IdRoom, t1.StartDateTime, t1.Who, t1.EndDateTime, RoomName = t2.Name, t1.IdLotus, t2.Deleted }).Where(o => o.IdMeeting == nMeeting.Id).Where(o => o.Deleted == false);

            MeetingViewModel it1 = new MeetingViewModel
            {
                RoomName = itemslist.First().RoomName,
                EndDateTime = itemslist.First().EndDateTime,
                Who = itemslist.First().Who,
                StartDateTime = itemslist.First().StartDateTime,
                Id = itemslist.First().IdMeeting,
                IdLotus = itemslist.First().IdLotus,
                IdRoom = itemslist.First().IdRoom
            };
            return it1;
        }

        /// <summary>
        /// Get meeting by lotus ID
        /// </summary>
        /// <param name="lotusId"></param>
        /// <returns></returns>
        public MeetingViewModel GetMeetingByLotusId(string lotusId)
        {
            var itemslist = (from t1 in this.context.Meetings
                             join t2 in this.context.Rooms on t1.IdRoom equals t2.Id
                             orderby t1.StartDateTime descending
                             select new { IdMeeting = t1.Id, t1.IdRoom, t1.StartDateTime, t1.Who, t1.EndDateTime, RoomName = t2.Name, t1.IdLotus, t2.Deleted }).Where(o => o.IdLotus == lotusId).Where(o => o.Deleted == false);
            if(itemslist.Count() > 0)
            {
                MeetingViewModel it1 = new MeetingViewModel
                {
                    RoomName = itemslist.First().RoomName,
                    EndDateTime = itemslist.First().EndDateTime,
                    Who = itemslist.First().Who,
                    StartDateTime = itemslist.First().StartDateTime,
                    Id = itemslist.First().IdMeeting,
                    IdLotus = itemslist.First().IdLotus,
                    IdRoom = itemslist.First().IdRoom
                };
                return it1;
            }
            else
            {
                return null;
            }
           
        }
        /// <summary>
        /// Update Information of an existing Meeting
        /// </summary>
        /// <param name="nMeeting"></param>
        public void UpdateMeeting(Meeting nMeeting)
        {
            this.context.Entry(nMeeting).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync()) > 0;
        }



        /// <summary>
        /// Add a room
        /// </summary>
        /// <param name="nRoom"></param>
        public void AddRoom(Room nRoom)
        {
            this.context.Add(nRoom);
            this.context.SaveChanges();
        }
        /// <summary>
        /// Get a specific room
        /// </summary>
        /// <param name="nRoom"></param>
        /// <returns></returns>
        public Room GetRoom(Room nRoom)
        {
            var f = this.context.Rooms.Where(x => x.Id == nRoom.Id).ToList();
            if (f.Count > 0)
            {
                return f.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Get Room data by the lotus name
        /// </summary>
        /// <param name="nameRoom"></param>
        /// <returns></returns>
        public Room GetRoomByLotusName(string nameRoom)
        {
            var f = this.context.Rooms.Where(x => x.IdLotus == nameRoom).Where(x => x.Deleted == false).ToList();
            if (f.Count > 0)
            {
                return f.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Delete a specific room
        /// </summary>
        /// <param name="nRoom"></param>
        public void DeleteRoom(Room nRoom)
        {
            this.context.Entry(nRoom).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();

        }
        /// <summary>
        /// Update the name of a specific room
        /// </summary>
        /// <param name="nRoom"></param>
        public void UpdateRoom(Room nRoom)
        {
            this.context.Entry(nRoom).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        /// <summary>
        /// Get a list of rooms
        /// </summary>
        /// <returns></returns>
        public List<Room> GetAllRooms()
        {
            return this.context.Rooms.Where(x => x.Deleted == false).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Get the las date of the check
        /// </summary>
        /// <returns></returns>
        public CheckUpdate GeLastCheckUpdate()
        {
            var f = this.context.CheckUpdates.OrderByDescending(x => x.Dob).ToList();
            if(f.Count > 0)
            {
                return f.First();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Add a new date check
        /// </summary>
        /// <param name="nCheckUpdate"></param>
        public void AddCheckUpdate(CheckUpdate nCheckUpdate)
        {
            this.context.Add(nCheckUpdate);
            this.context.SaveChanges();
        }
        /// <summary>
        /// Update a date check
        /// </summary>
        /// <param name="nCheckUpdate"></param>
        public void UpdateCheckUpdate(CheckUpdate nCheckUpdate)
        {
            this.context.Entry(nCheckUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
        }

        public string doCall(string urlParameters)
        {
            HttpClientHandler handler = new HttpClientHandler();
            using (var client = new HttpClient(handler, false))
            {


                client.BaseAddress = new Uri(this.config["lotus:url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetStringAsync(urlParameters).Result;
                client.Dispose();
                return response;
            }
        }

        /// <summary>
        /// Exec the sync between app and Lotus
        /// </summary>

        public object ExecSyncMeetings()
        {
            try
            {

                    string urlParameters = "?Login=&username=" + this.config["lotus:username"] + "&password=" + this.config["lotus:password"] + "&redirectto=" + this.config["lotus:redirectto"];
                    var response1 = doCall(urlParameters);
                    var releases = JArray.Parse(response1);
                    string count = "";
                    foreach (dynamic item in releases[0].Children())
                    {
                        if (item.Name == "@children") //Count of Childrens
                        {
                            count = item.Value;
                        }
                    }
                    urlParameters += "?count=" + (int.Parse(count) + 10).ToString();
                    releases.Clear();
                    var response = doCall(urlParameters);
                    releases = JArray.Parse(response);
                  
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                  
                    int j = 0;
                    List<LotusMeeting> listlotusMeetings = new List<LotusMeeting>();
                    foreach (var record in releases)
                    {
                        if (j > 0)
                        {
                            LotusMeeting lotus = new LotusMeeting();
                            foreach (dynamic item in record.Children())
                            {
                                if (item.Name == "@unid") //Meeting ID
                                {
                                    lotus.Unid = item.Value;
                                }
                                else if (item.Name == "StartDateTime")
                                {

                                    string val = item.Value;
                                    string[] dt = val.Split(' ');
                                    string[] day = dt[0].Split('/');
                                    string[] hour = dt[1].Replace("Z", "").Split(':');
                                    Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                                    lotus.StartDateTime = new DateTime(int.Parse(day[2]), int.Parse(day[0]), int.Parse(day[1]), int.Parse(hour[0]) + 2, int.Parse(hour[1]), int.Parse(hour[2]));
                                }
                                else if (item.Name == "EndDateTime")
                                {
                                    string val = item.Value;
                                    string[] dt = val.Split(' ');
                                    string[] day = dt[0].Split('/');
                                    string[] hour = dt[1].Replace("Z", "").Split(':');
                                    Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                                    lotus.EndDateTime = new DateTime(int.Parse(day[2]), int.Parse(day[0]), int.Parse(day[1]), int.Parse(hour[0]) + 2, int.Parse(hour[1]), int.Parse(hour[2]));
                                }
                                else if (item.Name == "$17") // Meeting room name
                                {
                                    lotus.RoomName = item.Value;
                                }
                                else if (item.Name == "$18") // Owner
                                {
                                    string val = item.Value;
                                    string[] ow = val.Split('/');
                                    lotus.Owner = ow[0];
                                }
                            }
                            if (lotus != null)
                            {
                                listlotusMeetings.Add(lotus);
                            }
                        }
                        j++;
                    }

                    List<Meeting> resultCheck = this.GetAllMeetingsToday();
                    List<Meeting> resultChecked = new List<Meeting>();
                    if (listlotusMeetings.Count > 0)
                    {
                        foreach (LotusMeeting lot in listlotusMeetings)
                        {
                            Room room = this.GetRoomByLotusName(lot.RoomName);
                            if (room == null)
                            {
                                string[] ro = lot.RoomName.Split('/');
                                room = new Room
                                {
                                    IdLotus = lot.RoomName,
                                    Name = ro[0]
                                };
                                this.AddRoom(room);
                                room = this.GetRoomByLotusName(lot.RoomName);
                            }

                            MeetingViewModel mee = this.GetMeetingByLotusId(lot.Unid);
                            if (mee == null)
                            {
                                Meeting meeting = new Meeting
                                {
                                    StartDateTime = lot.StartDateTime,
                                    EndDateTime = lot.EndDateTime,
                                    Who = lot.Owner,
                                    IdLotus = lot.Unid,
                                    IdRoom = room.Id,
                                    check = true
                                };

                                this.AddMeeting(meeting);
                            }
                            else
                            {
                                Meeting meeting = new Meeting
                                {
                                    Id = mee.Id,
                                    IdLotus = mee.IdLotus,
                                    IdRoom = mee.IdRoom,
                                    StartDateTime = lot.StartDateTime,
                                    EndDateTime = lot.EndDateTime,
                                    Who = lot.Owner,
                                    check = true
                                };
                                this.UpdateMeeting(meeting);
                                resultChecked.Add(meeting);
                            }
                        }
                    }
                    //Update all the meetings not checked need to be deleted
                    foreach (Meeting r in resultCheck)
                    {
                        int result = resultChecked.Where(s => s.Id == r.Id).Count();
                        if(result == 0)
                        {
                            this.DelMeeting(r);
                        }
                    }
                
                return "Done";
            }
            catch(Exception ex)
            {
                return ex;
            }
           
        }

        public List<RoomViewModel> GetAvailabilityOfRooms()
        {
            List<Room> rooms = GetAllRooms();
            List<RoomViewModel> roomsAvailable = new List<RoomViewModel>();
            foreach (Room room in rooms)
            {
                bool free = false;
                if (CheckAvailability(room.Id))
                {
                    free = true;
                }
                RoomViewModel roomView = new RoomViewModel
                {
                    Id = room.Id,
                    Free = free,
                    IdLotus = room.IdLotus,
                    Name = room.Name
                };
                roomsAvailable.Add(roomView);
            }
            return roomsAvailable;
        }
        private bool CheckAvailability(int idRoom)
        {
            DateTime now = DateTime.Now;
            var itemslist = (from t1 in this.context.Meetings
                             join t2 in this.context.Rooms on t1.IdRoom equals t2.Id
                             select new { IdMeeting = t1.Id, t1.IdRoom, t1.StartDateTime, t1.Who, t1.EndDateTime, t1.IdLotus, RoomName = t2.Name, t2.Deleted, t1.check })
                             .Where(x => x.Deleted == false)
                             .Where(x => DateTime.Compare(x.StartDateTime, now) < 0)
                             .Where(x => DateTime.Compare(x.EndDateTime, now) > 0)
                             .Where(x => x.IdRoom == idRoom);
            if(itemslist.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }

}