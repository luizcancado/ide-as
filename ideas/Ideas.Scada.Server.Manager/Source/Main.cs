using Gtk;
using Ideas.Scada.Server.Manager;

namespace Idea.Scada.Server.Manager
{
	class MainClass
	{
		
		private static MainWindow win;
		private static StatusIcon trayIcon;
		
		public static void Main (string[] args)
		{
			// Inicia a aplicacao
			Application.Init ();
			
			if(args.Length == 0)
			{
				 win = new MainWindow ();
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
				
				Configuration config = new Configuration(newargs);
				win = new MainWindow(config);
			}
			
			// Attach to the Delete Event when the window has been closed.
			win.DeleteEvent += delegate { Application.Quit (); };
			
			// Creation of the Icon
			trayIcon = new StatusIcon(Gdk.Pixbuf.LoadFromResource ("Ideas.Scada.Server.Manager.Resources.Icons.icon_16x16.png"));
			trayIcon.Visible = true;
			
			// Show/Hide the window (even from the Panel/Taskbar) when the TrayIcon has been clicked.
			trayIcon.Activate += delegate { win.Visible = !win.Visible; };
			
			// A Tooltip for the Icon
			trayIcon.Tooltip = "Ideas Server Manager";
			
			win.ShowAll ();
			Application.Run ();
		}
	}
}

