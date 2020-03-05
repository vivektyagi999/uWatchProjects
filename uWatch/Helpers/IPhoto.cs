using System;
using System.IO;
using System.Threading.Tasks;

namespace uWatch
{
	public interface IPhoto
	{
		Task<Stream> GetPhoto(string path);
	}
}

