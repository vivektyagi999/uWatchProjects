// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace UwatchPCL.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static  class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }
 

     #region Setting Constants

	private const string IsLogoutKey = "isLogut_key";
		private static readonly bool IsLogotDefault = true;

		private const string IsRememberMeKey = "isRememberMe_key";
		private static readonly bool IsRememberMeDefault = true;

		private const string RoleidKey = "roleid_key";
		private static readonly int RoleidDefault = 0;

		private const string UseridKey = "userid_key";
		private static readonly int UseridDefault = 0;

		private const string AccessTokenkey = "accesstoken_key";
		private static readonly string AccessTokenDefault = string.Empty;

		private const string UsernameKey = "username_key";
		private static readonly string UsernameDefault = string.Empty;

		private const string PasswordKey = "password_key";
		private static readonly string PasswordDefault = string.Empty;

		private const string DeviceTokenKey = "devicetoken_key";
		private static readonly string DeviceTokenDefault = string.Empty;

		private const string FullNameKey = "fullname_key";
		private static readonly string FullNameDefault = string.Empty;

		private const string VersionKey = "version_key";
		private static readonly string VersionDefault = string.Empty;

        private const string CurrentVersionKey = "Currentversion_key";
        private static readonly string CurrentVersionDefault = string.Empty;

		private const string IsNewItemKey = "isnewitem_key";
		private static readonly bool IsNewItemDefault = true;

		#endregion

        public static string CurrentVersion
        {
            get
            {

                return AppSettings.GetValueOrDefault(CurrentVersionKey, CurrentVersionDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CurrentVersionKey, value);
            }
        }

		public static string Version
		{
			get
			{
				
				return AppSettings.GetValueOrDefault(VersionKey, VersionDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(VersionKey, value);
			}
		}
		public static bool IsNewItem
		{
			get
			{
				return AppSettings.GetValueOrDefault(IsNewItemKey, IsNewItemDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(IsNewItemKey, value);
			}
		}
		public static bool IsRememberMe
		{
			get
			{
				return AppSettings.GetValueOrDefault(IsRememberMeKey, IsRememberMeDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(IsRememberMeKey, value);
			}
		}

		public static bool IsLogout
		{
			get
			{
				return AppSettings.GetValueOrDefault(IsLogoutKey, IsLogotDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(IsLogoutKey, value);
			}
		}

		public static int UserID
		{
			get
			{
				return AppSettings.GetValueOrDefault(UseridKey, UseridDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(UseridKey, value);
			}
		}

		public static int RoleID
		{
			get
			{
				return AppSettings.GetValueOrDefault(RoleidKey, RoleidDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(RoleidKey, value);
			}
		}

		public static string AccessToken
		{
			get
			{
				return AppSettings.GetValueOrDefault(AccessTokenkey, AccessTokenDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(AccessTokenkey, value);
			}
		}


		public static string UserName
		{
			get
			{
				return AppSettings.GetValueOrDefault(UsernameKey, UsernameDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(UsernameKey, value);
			}
		}


		public static string Password
		{
			get
			{
				return AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(PasswordKey, value);
			}
		}


		public static string DeviceToken
		{
			get
			{
				return AppSettings.GetValueOrDefault(DeviceTokenKey, DeviceTokenDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(DeviceTokenKey, value);
			}
		}

		public static string FullName
		{
			get
			{
				return AppSettings.GetValueOrDefault(FullNameKey, FullNameDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(FullNameKey, value);
			}
		}


	}
}