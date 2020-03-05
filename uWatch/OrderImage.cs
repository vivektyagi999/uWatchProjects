using System;
namespace uWatch
{
    public class OrderImage
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string FileName { get; set; }
        public byte[] orderImage { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Uploaddate { get; set; }
    }
}
