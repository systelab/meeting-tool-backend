using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace main.Models
{
    public class EventsList
    {
        public List<Events> events
        {
            get; set;
        }
    } 
    public class Events
    {
        public string summary { get; set; }
       
        public int sequence { get; set; }
        public string transparency { get; set; }
        public DateMeeting start { get; set; }
        public DateMeeting end { get; set; }
        public MeetingOrganizer organizer { get; set; }
        public List<Attende> attendees { get; set; }
    }
    public class DateMeeting
    {
        public string date { get; set; }
        public string time { get; set; }
        public bool utc { get; set; }
    }
    public class MeetingOrganizer
    {
        public string displayName { get; set; }
        public string email { get; set; }
    }
    public class Attende
    {
        public string role { get; set; }
        public string userType { get; set; }
        public string status { get; set; }
        public bool rsvp { get; set; }
        public string email { get; set; }
    }
}
