using System;

namespace uWatch
{
    public class DeviceHelper
    {
        public static void ReleaseImageMemory(string imageDirecotryPath)
        {
            try
            {
                //var list = Directory.GetFiles(imageDirecotryPath, "*");
                //var list = System.IO.Directory.GetFiles(imageDirecotryPath,"*");

                //if (list.Length > 0)
                //{
                //    for (int i = 0; i < list.Length; i++)
                //    {
                //        File.Delete(list[i]);
                //    }
                //}

            }
            catch (Exception ex)
            {

            }
        }
        public DeviceHelper()
        {
        }
    }
}
