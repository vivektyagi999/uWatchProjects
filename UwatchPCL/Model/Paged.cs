using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UwatchPCL.Model
{
	public class Paged 
	{
		public int PageIndex { get; set; }

		public int TotalPage { get; set; }

		public int TotalRecords { get; set; }

		public int? RecordPerPage { get; set; }

		public string SearchText { get; set; }

		
	}
}

