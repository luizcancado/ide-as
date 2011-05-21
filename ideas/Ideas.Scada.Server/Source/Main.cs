using System;

namespace Ideas.Scada.Server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Kernel appKernel;
			
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

				// Runs application kernel
				appKernel = new Kernel(config);
				appKernel.Run();
			}
		}
	}
}

