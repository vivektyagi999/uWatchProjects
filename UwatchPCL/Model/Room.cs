using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
    public class Room : BaseModel
    {
        public int RoomID { get; set; }

        public string RoomName { get; set; }

        public int OwnerId { get; set; }

        public string RoomNameNID
        {
            get{
                return RoomName + "#" + RoomID.ToString();
            }
        }
     }
}
