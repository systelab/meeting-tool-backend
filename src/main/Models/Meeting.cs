using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Main.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Who { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Link { get; set; }
        public string IdLotus { get; set; }
        public bool check {get;set;}

        [ForeignKey("Room")]
        public int IdRoom { get; set; }
    }
}