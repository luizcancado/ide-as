using System;
using Ideas.Scada.Common;
using System.Threading;

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
			// Start SCADA server
			scadaApplication.Start();
		}
	}
}

