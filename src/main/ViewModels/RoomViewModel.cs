namespace Main.ViewModels
{
    using Main.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RoomViewModel
    {
        public int Id { get; set; }
        public string IdLotus { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public bool Free { get; set; }
    }
}