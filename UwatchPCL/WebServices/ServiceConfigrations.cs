using System;
namespace UwatchPCL
{

    public class ServiceConfigrations
    {

        public string GetUserDetailByAccessCode => "api/Account/GetUserDetailsByAccessCode?AccessCode=";

        public string GetUserDetailByProductCode => "api/device/IsDeviceInStock?serial_no=";
        

        public string GetCancelDeviceOffConfigUrl => "api/device/CancelDeviceOffConfig?AppConfigID=";

        public string SignUpForApp => "api/Account/uWatchSignUpFromApp";

        public string ResetPassword => "api/Account/UpdateUserPassword";

        public string CheckLatestAppVersionUrl => "api/Account/CheckLatestAppVersion";
        public string CheckUserTokenUrl => "api/Account/CheckUserToken";

        // public string SignUpWithTag => "api/device/AddTagToDeviceFromApp";
        public string SignUpWithTag => "api/device/SaveDefaultTranmitterConfig";

        public string CubeConfigLib => "api/device/GetDefaultConfigLib";

        public string CheckAppUpdate => "api/Account/CheckAppUpdate?CurrentVersion=";

        public string GetReceiptImage => "api/Device/FetchReceiptImagePath?AssetsID=";

        public string GetDeviceConfigurationLink => "api/Device/FetchDeviceConfigurationDetails";

        public string AddTagsToCubefromApp => "api/Device/AddTagsToCubefromApp";

    }
}


