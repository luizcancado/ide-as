using System;
using Ideas.Scada.Common;

namespace Ideas.Scada.Server
{
	public class Kernel
	{
		static ScadaApplication scadaApplication;
		
		public Kernel (ServerConfiguration config)
		{
			if(config == null)
			{
				scadaApplication = null;
			}
			else
			{
				scadaApplication = new ScadaApplication(config.file);
			}
		}
		
		/// <summary>
		/// Starts the SCADA server
		/// </summary>
		public void Run()
		{
			try
			{
				Console.WriteLine("Loading SCADA application...");
				
				// Loads SCADA application
				scadaApplication.Start();
				
				Console.WriteLine(scadaApplication.Name + "It was successfully loaded.");
			}
			catch(Exception e)
			{
				Console.WriteLine("Error while loading SCADA application.");
				Console.WriteLine(e.Message);
			}
		}
	}
}

