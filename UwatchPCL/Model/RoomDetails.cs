using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
    public class RoomDetails: BaseModel
    {
        public int RoomDetailID { get; set; }

        public int RoomID { get; set; }

        public string Item { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

    }
}
