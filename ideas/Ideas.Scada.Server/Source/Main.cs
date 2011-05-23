using System;

namespace Ideas.Scada.Server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
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
				
				ServerConfiguration config = new ServerConfiguration(newargs);
				
				Console.WriteLine("Loading scada application...");
				
				try
				{
					// Loads configuration to the application kernel
					appKernel = new Kernel(config);
					
					Console.WriteLine("Application successfully loaded.");
				}
				catch(Exception e)
				{
					Console.WriteLine("ERROR while loading scada application.");
					Console.WriteLine(e.Message);
				}
				
				
				if(appKernel != null)
				{
					// Runs application kernel
					appKernel.Run();
				}
			}
			
			Console.ReadKey();
		}
	}
}

