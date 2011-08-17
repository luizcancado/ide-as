using System;
using NDesk.Options;
using System.Text;

namespace Ideas.Scada.Server
{
	public class ServerConfiguration
	{
		public string file;
		
		public ServerConfiguration (string[] args)
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
		
		public static void ShowHelp()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("");
			
			Console.Write(sb.ToString());
		}
		
	}
}

