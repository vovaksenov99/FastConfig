using FastConfigLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FastConfig
{
	class Program
	{
		
		public static void Main()
		{
			try
			{
				FastConfigLibrary.FastConfig config = new FastConfigLibrary.FastConfig("test.config");

				Console.WriteLine(config.stringVariable);
				Console.WriteLine(config.doubleVariable);
				Console.WriteLine(config.londVariable);
				Console.WriteLine(config.boolVariable);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
			}

		}
	}
}
