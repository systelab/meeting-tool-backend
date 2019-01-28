using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Main.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string IdLotus { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public string email { get; set; }
        public bool tv { get; set; }
        public bool projector { get; set; }
        public bool computer { get; set; }
        public bool phone { get; set; }
        public bool videoconference { get; set; }
        public bool microfonia { get; set; }
        public int attenders { get; set; }
        public string plano { get; set; }
        public int planta { get; set; }
    }
}