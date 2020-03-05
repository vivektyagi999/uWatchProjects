using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UwatchPCL.Model;
using Xamarin.Forms;

namespace UwatchPCL
{
	public class MemberStatics : BaseModel
	{
		public int PageIndex { get; set; }

		public int RecordPerPage { get; set; }
		public string SearchText { get; set; }
		public int User_Idx { get; set; }

	
	}
}

