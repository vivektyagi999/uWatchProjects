using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using UwatchPCL;

namespace UwatchPCL
{
	public class AddressComponent
	{
		public string long_name { get; set; }
		public string short_name { get; set; }
		public IList<string> types { get; set; }
	}

	public class Northeast
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Southwest
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Bounds
	{
		public Northeast northeast { get; set; }
		public Southwest southwest { get; set; }
	}

	public class Location
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	

	public class Geometry
	{
		public Bounds bounds { get; set; }
		public Location location { get; set; }
		public string location_type { get; set; }
		
	}

	public class Result
	{
		public IList<AddressComponent> address_components { get; set; }
		public string formatted_address { get; set; }
		public Geometry geometry { get; set; }
		public string place_id { get; set; }
		public IList<string> types { get; set; }
	}

	public class Example
	{
		public IList<Result> results { get; set; }
		public string status { get; set; }
}
}
