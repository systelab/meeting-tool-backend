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

    }
}