namespace Main.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MeetingViewModel
    {
        public int Id { get; set; }
        public string Who { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Link { get; set; }
        public string IdLotus { get; set; }
        public int IdRoom { get; set; }
        public string RoomName { get; set; }
        
    }
}