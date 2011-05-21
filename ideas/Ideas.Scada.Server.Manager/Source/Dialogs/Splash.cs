using System;

namespace Ideas.Scada.Server.Manager
{
	public partial class Splash : Gtk.Window
	{
		public Splash () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

