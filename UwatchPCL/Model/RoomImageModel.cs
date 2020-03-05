using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
    public class RoomImageModel: BaseModel
    {
        public int RoomID { get; set; }

        public byte[] RoomImage { get; set; }

        public string RoomImagePath { get; set; }

        public int InventoryImageID { get; set; }

    }
}
