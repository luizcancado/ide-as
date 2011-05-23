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
				Console.WriteLine("Starting scada application server...");
				
				// Loads SCADA application
				scadaApplication.Start();
				
				Console.WriteLine("Server started with the application: " + scadaApplication.Name);
			}
			catch(Exception e)
			{
				Console.WriteLine("Error while starting scada application server.");
				Console.WriteLine(e.Message);
			}
		}
	}
}

