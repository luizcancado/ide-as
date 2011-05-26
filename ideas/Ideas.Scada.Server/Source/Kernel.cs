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
			// Loads SCADA application
			scadaApplication.Start();
		}
	}
}

