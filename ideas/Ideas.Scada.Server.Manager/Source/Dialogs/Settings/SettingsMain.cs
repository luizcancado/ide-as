using System;
using Gtk;

namespace Ideas.Scada.Server.Manager.Settings
{
	
	
	public partial class SettingsMain : Gtk.Dialog
	{
		public SettingsMain ()
		{
			this.Build ();
			
			CreateSettingsTree ();
			CreateSettingsTreeNodes ();
			
			
			
		}
		
		private void CreateSettingsTree ()
		{
			Gtk.TreeStore trsSettingsTreeStore = new Gtk.TreeStore (typeof (string));
			
			Gtk.CellRendererPixbuf celSettingsTreeItemIcon = new Gtk.CellRendererPixbuf();
			Gtk.CellRendererText celSettingsTreeItemText = new Gtk.CellRendererText();
						
			Gtk.TreeViewColumn trcSettingsTreeColumn = new Gtk.TreeViewColumn();
			
			trcSettingsTreeColumn.PackStart(celSettingsTreeItemIcon, false);
			trcSettingsTreeColumn.AddAttribute(celSettingsTreeItemIcon, "pixbuf", 0);
			
			trcSettingsTreeColumn.PackStart(celSettingsTreeItemText, true);			
			trcSettingsTreeColumn.AddAttribute(celSettingsTreeItemText, "text", 1);
			
			trvSettingsTree.AppendColumn(trcSettingsTreeColumn);
			trvSettingsTree.Model = trsSettingsTreeStore;
			
		}

		void CreateSettingsTreeNodes ()
		{
			Gtk.TreeStore trsSettingsTreeStore = trvSettingsTree.Model as Gtk.TreeStore;
			
			Gdk.Pixbuf icnGeneral = new Gdk.Pixbuf(this.GetType().Assembly, "Ideas.Scada.Server.Manager.Resources.Icons.Interface.16.scada.png");
			Gdk.Pixbuf icnGeneralIdeasServer = new Gdk.Pixbuf(this.GetType().Assembly, "Ideas.Scada.Server.Manager.Resources.Icons.Interface.16.scada.png");
			
			Gtk.TreeIter iterGeneral = 
				trsSettingsTreeStore.AppendValues( 
                      new object[] { icnGeneral, "General"});
			
			trsSettingsTreeStore.AppendValues( 
			      iterGeneral,                               
                  new object[] { icnGeneralIdeasServer, "Ideas Server"});
		}
		
		protected void OnButtonOkActivated (object sender, System.EventArgs e)
		{
		}
		
		
	}
}

