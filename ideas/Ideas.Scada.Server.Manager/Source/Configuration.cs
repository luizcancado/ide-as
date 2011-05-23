using System;
using System.IO;
using NDesk.Options;

namespace Ideas.Scada.Server.Manager
{
	public class Configuration
	{
		public string file;
		
		public Configuration (string[] args)
		{
			OptionSet p = new OptionSet () 
			{
				{ 
					"f|file=", 
					"the SCADA Application File to open", 
					v => file = v
				},		
			};
			
			p.Parse(args);
		}
	}
}

