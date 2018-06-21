namespace Main.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Main.Models;
    using Main.ViewModels;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Interface with all the method needed
    /// </summary>
    public interface IMeetingRepository
    {
        void AddMeeting(Meeting newMeeting);

        void DelMeeting(Meeting newMeeting);
        MeetingViewModel GetMeetingByLotusId(string lotusId);
        object GetAllMeetings();
        object GetAllMeetingsByRoom(int idRoom);
        MeetingViewModel GetMeeting(Meeting nMeeting);

        Task<bool> SaveChangesAsync();

        void UpdateMeeting(Meeting nMeeting);

        void AddRoom(Room nRoom);
        Room GetRoom(Room nRoom);
        void DeleteRoom(Room nRoom);
        void UpdateRoom(Room nRoom);
        List<Room> GetAllRooms();
        Room GetRoomByLotusName(string nameRoom);

        CheckUpdate GeLastCheckUpdate();
        void AddCheckUpdate(CheckUpdate nCheckUpdate);
        object ExecSyncMeetings();
        void UpdateCheckUpdate(CheckUpdate nCheckUpdate);

        List<RoomViewModel> GetAvailabilityOfRooms();


        Task<HttpResponseMessage> DoReservation(string email, DateTime start, DateTime end);
        //string DoReservation(int idRoom, DateTime start, DateTime end);
    }
}