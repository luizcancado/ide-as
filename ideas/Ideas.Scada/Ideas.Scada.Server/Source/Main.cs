using System;
using log4net;
using log4net.Config;
using System.IO;

namespace Ideas.Scada.Server
{
	class MainClass
	{
		// Define a static logger variable so that it references the
	    // Logger instance named "MyApp".
	    private static readonly ILog log = LogManager.GetLogger(typeof(MainClass));
		
		public static void Main (string[] args)
		{		
			// Set up configuration from the logging config file
			XmlConfigurator.ConfigureAndWatch(new FileInfo("Logging.conf"));
			
			Kernel appKernel = null;
			
			if(args.Length == 0)
			{
				// If no argument was passed show help message
				ServerConfiguration.ShowHelp();
			}
			else
			{				
				string[] newargs;
				
				// If there is only one arg, it is assumed that that is the file arg.
				if(!args[0].StartsWith("-"))
				{
					newargs = new string[] {"-f", args[0]};
				}
				else
				{
					newargs = args;
				}
				
				// Reads configuration
				ServerConfiguration config = new ServerConfiguration(newargs);
				
				try
				{
					// Loads configuration to the application kernel
					appKernel = new Kernel(config);
					
					// Check if Kernel was successfully created
					if(appKernel != null)
					{
						// Runs application kernel
						appKernel.Run();
					}
					
				}
				catch(Exception e)
				{
					log.Fatal("Server could not be started.");
					log.Fatal(e.Message);
				}			
				
			}
			
			Console.ReadKey();
		}
	}
}

