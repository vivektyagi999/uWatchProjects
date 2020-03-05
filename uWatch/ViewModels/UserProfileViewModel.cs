using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using UwatchPCL;
using System.Threading.Tasks;
using UwatchPCL.Model.Response;

namespace uWatch
{
	public class UserProfileViewModel: INotifyPropertyChanged
	{
		public UserProfileViewModel ()
		{
			IsEditable = false;
		}

		/// <summary>
		/// The userId of the User.
		/// </summary>
		private int _userId;
		public int UserId 
		{
			get
			{
				return _userId;
			}
			set 
			{
				_userId = value;
				OnPropertyChanged ();
			}
		}


		/// <summary>
		/// The username of the User.
		/// </summary>
		private string _userName;
		public string UserName 
		{
			get
			{
				return _userName;
			}
			set 
			{
				_userName = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The first name of the User.
		/// </summary>
		private string _firstName;
		public string FirstName 
		{
			get
			{
				return _firstName;
			}
			set 
			{
				_firstName = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The last name of the User.
		/// </summary>
		private string _lastName;
		public string LastName 
		{
			get
			{
				return _lastName;
			}
			set 
			{
				_lastName = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The email of the User.
		/// </summary>
		private string _mobile;
		public string Mobile 
		{
			get
			{
				return _mobile;
			}
			set 
			{
				_mobile = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The email of the User.
		/// </summary>
		private string _email;
		public string Email 
		{
			get
			{
				return _email;
			}
			set 
			{
				_email = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The email of the User.
		/// </summary>
		private string _address;
		public string Address 
		{
			get
			{
				return _address;
			}
			set 
			{
				_address = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The email of the User.
		/// </summary>
		private string _city;
		public string City 
		{
			get
			{
				return _city;
			}
			set 
			{
				_city = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The email of the User.
		/// </summary>
		private string _country;
		public string Country 
		{
			get
			{
				return _country;
			}
			set 
			{
				_country = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The email of the User.
		/// </summary>
		private string _postcode;
		public string Postcode 
		{
			get
			{
				return _postcode;
			}
			set 
			{
				_postcode = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// The last name of the User.
		/// </summary>
		private bool _isEditable;
		public bool IsEditable 
		{
			get
			{
				return _isEditable;
			}
			set 
			{
				_isEditable = value;
				OnPropertyChanged ();
			}
		}

	

		public async Task UserDetails(int userid)
		{
			try
			{
				var req = new UwatchPCL.Model.Response.AccountModel();
				req.User_Idx = userid;
				var res = await ApiService.Instance.UserDetails(req);
				this.UserId = req.User_Idx;
				
			}
			catch { }

		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) 
			{
				handler(this, new PropertyChangedEventArgs (propertyName));
			}
		}

	}
}

