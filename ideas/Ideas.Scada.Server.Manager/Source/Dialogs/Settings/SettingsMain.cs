using System;
using Gtk;
using Ideas.Scada.Server.Manager.Settings.General;
	
namespace Ideas.Scada.Server.Manager.Settings
{
	public partial class SettingsMain : Gtk.Dialog
	{
		#region MEMBERS
		
		IdeasServer cfgGeneralIdeasServer;
		
		#endregion MEMBERS
			
		public SettingsMain ()
		{
			cfgGeneralIdeasServer = new IdeasServer();
			
			this.Build ();
			
			LoadInitialPanel();
			
			CreateSettingsTree ();
			CreateSettingsTreeNodes ();
		}

		public void LoadInitialPanel ()
		{
			hpaned1.Add(cfgGeneralIdeasServer);
			hpaned1.ShowAll();
		}
		
		private void CreateSettingsTree ()
		{
			Gtk.TreeStore trsSettingsTreeStore = new Gtk.TreeStore (typeof (Gdk.Pixbuf), typeof (string));
			
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
						
			Gdk.Pixbuf icnGeneral = IconFactory.LookupDefault("scada").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
			Gdk.Pixbuf icnGeneralIdeasServer = IconFactory.LookupDefault("folder").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
			
			Gtk.TreeIter iterGeneral = 
				trsSettingsTreeStore.AppendValues( 
                      new object[] {null, "General"});
			
			trsSettingsTreeStore.AppendValues( 
			      iterGeneral,                               
                  new object[] { icnGeneralIdeasServer, "Ideas Server"});
		}
		
		protected void OnButtonOkActivated (object sender, System.EventArgs e)
		{
		}
		
		
	}
}

