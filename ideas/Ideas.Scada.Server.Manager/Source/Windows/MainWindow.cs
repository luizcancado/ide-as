using System;
using Gtk;
using System.Collections.Generic;
using Ideas.Scada.Common;
using Ideas.Scada.Common.DataSources;
using System.Diagnostics;
using System.IO;

namespace Ideas.Scada.Server.Manager
{
	public partial class MainWindow : Gtk.Window
	{
		private ScadaApplication scadaApplication;
		private Process serverProcess;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public MainWindow () : base(Gtk.WindowType.Toplevel)
		{
			Build ();
	
			scadaApplication = null;
	
		}
		
		/// <summary>
		/// Constructor from a Configuration setup
		/// </summary>
		/// <param name="config">
		/// A <see cref="Configuration"/>
		/// </param>
		public MainWindow (Configuration config) : base(Gtk.WindowType.Toplevel)
		{
			Build ();
			
			if(config == null)
			{
				scadaApplication = null;
			}
			else
			{
				scadaApplication = new ScadaApplication(config.file);
				UpdateScadaApplicationsListTree ();
			}
		}
	
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
		
		protected virtual void mnuFileExit_Click (object sender, System.EventArgs e)
		{
			this.Destroy();
			Application.Quit();
		}
		
		protected virtual void mnuOpenApplication_Click (object sender, System.EventArgs e)
		{
			OpenApplication();
		}
		
		/// <summary>
		/// Loads the SCADA configuration file. Converts XML to a ScadaApplication class
		/// </summary>
		/// <param name="scadafile">
		/// A <see cref="System.String"/>
		/// </param>
		public void LoadScadaFile(string scadafile)
		{
			scadaApplication = new ScadaApplication(scadafile);
			
			// Updates the list tree/expander
			UpdateScadaApplicationsListTree();
			
			UpdateTextView();
		}
		
		
		protected void UpdateTextView ()
		{
			StreamReader scadaFile = new StreamReader(scadaApplication.FilePath);
			string fileString = scadaFile.ReadToEnd();
			
			txvTextView.Buffer.Text = fileString;
		}
		
		protected void UpdateScadaApplicationsListTree ()
		{
			// Check if a scada app was already loaded
			if(scadaApplication == null)
			{
				return;
			}
			
			// Clean tree view
			CleanTreeView();
	
			Gdk.Pixbuf icnApplicationIcon = IconFactory.LookupDefault("scada").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
			Gdk.Pixbuf icnFolderIcon = IconFactory.LookupDefault("folder").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
			Gdk.Pixbuf icnScreenIcon =  IconFactory.LookupDefault("screen").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
			Gdk.Pixbuf icnProjectIcon = IconFactory.LookupDefault("folder").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
			Gdk.Pixbuf icnTagsWebserviceIcon = IconFactory.LookupDefault("tagswebservice").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
			Gdk.Pixbuf icnTagsDatabaseIcon =  IconFactory.LookupDefault("tagsdatabase").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
			
			Gtk.CellRendererPixbuf cellIcon = new Gtk.CellRendererPixbuf ();
			Gtk.CellRendererText cellItem = new Gtk.CellRendererText ();
			TreeViewColumn colItem = new TreeViewColumn ();
			colItem.Title = "Application";
			colItem.PackStart (cellIcon, false);
			colItem.AddAttribute (cellIcon, "pixbuf", 0);
			colItem.PackStart (cellItem, true);
	  		colItem.AddAttribute (cellItem, "text", 1);
					
			// Add column to the TreeView
			trvApplicationTreeView.AppendColumn (colItem);
			
			
			TreeStore applicationTreeStore = new TreeStore(typeof (Gdk.Pixbuf), typeof (string));
				
			Gtk.TreeIter applicationIter = 
				applicationTreeStore.AppendValues(new object[] { icnApplicationIcon, scadaApplication.Name});
			
			foreach(Project project in scadaApplication.Projects)
			{
				Gtk.TreeIter projectIter = 
					applicationTreeStore.AppendValues(
	                      applicationIter, 
	                      new object[] { icnProjectIcon, project.Name});
				
				Gtk.TreeIter screensIter = 
					applicationTreeStore.AppendValues(
	                      projectIter, 
	                      new object[] { icnFolderIcon, "Screens" });
				
				foreach(Screen screen in project.Screens)
				{
					applicationTreeStore.AppendValues(
						screensIter, 
						new object[] { icnScreenIcon, screen.Name });
				}
				
				Gtk.TreeIter tagsDatabaseIter = 
					applicationTreeStore.AppendValues(
						projectIter, 
						new object[] { icnFolderIcon, "Tags DataSources" });
				
				foreach(DataSource datasource in project.Datasources)
				{
					applicationTreeStore.AppendValues(
	                      tagsDatabaseIter, 
	                      new object[] { icnTagsDatabaseIcon, datasource.Name });
				}
						
				Gtk.TreeIter tagsWebserviceIter = 
					applicationTreeStore.AppendValues(
						projectIter, 
						new object[] { icnFolderIcon, "Tags WebService"});
				
				applicationTreeStore.AppendValues(
	                      tagsWebserviceIter, 
	                      new object[] { icnTagsWebserviceIcon, project.TagsWebService.Name });
			
			}
			
			trvApplicationTreeView.Model = applicationTreeStore;
			
			trvApplicationTreeView.ExpandAll();
				
			this.ShowAll();
		}
	
		protected virtual void aboutAction_Click (object sender, System.EventArgs e)
		{
			About aboutDialog = new About();
			
			aboutDialog.Show();
		}
		
		
		/// <summary>
		/// Start an instance of Ideas.Scada.Server
		/// </summary>
		private void startServers()
		{	
			serverProcess.StartInfo.FileName = "/home/luiz/Projects/Ideas/Ideas.Scada.Server/bin/Debug/Ideas.Scada.Server.exe";
			serverProcess.StartInfo.Arguments = scadaApplication.FilePath;
			serverProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(scadaApplication.FilePath);		
			
			serverProcess.Start();
			
			tbbStopServer.Sensitive = true;
			tbbStartServer.Sensitive = false;	
		}
		
		/// <summary>
		/// Stops the actual instance of Idea.Scada.Server
		/// </summary>
		private void stopServers()
		{
			try
			{
				serverProcess.Kill();
			}
			catch(Exception e)
			{
				
			}
			finally
			{
				serverProcess.Close();
				serverProcess = null;
				
				tbbStopServer.Sensitive = false;
				tbbStartServer.Sensitive = true;	
			}
		}
		
		/// <summary>
		/// Open dialog to choose the SCADA application file
		/// </summary>
		private void OpenApplication()
		{
			FileChooserDialog fileChooserDialog = 
				new FileChooserDialog ("Open file", 
				                       null, 
				                       FileChooserAction.Open);
			
			// Add buttons to file dialog
			fileChooserDialog.AddButton(Stock.Cancel, ResponseType.Cancel);
			fileChooserDialog.AddButton(Stock.Open, ResponseType.Ok);
			
			// Select file filters
			fileChooserDialog.Filter = new FileFilter();
			fileChooserDialog.Filter.AddPattern("*.scada");
			
			fileChooserDialog.SelectMultiple = false;
			
			ResponseType RetVal = (ResponseType)fileChooserDialog.Run();
			
			string fileName = fileChooserDialog.Filename;
			
			// Destroy the dialog
			fileChooserDialog.Destroy();
			
			// Handle the dialog's exit value
			// read the file name from Fcd.Filename
			if (RetVal == ResponseType.Ok) 
			{			
				// Load application's configuration file
				LoadScadaFile(fileName);
			}
			
			enableToolBarButtons();
		}
		
		/// <summary>
		/// Clear the tree view widget
		/// </summary>
		void CleanTreeView ()
		{
			foreach(TreeViewColumn column in trvApplicationTreeView.Columns)
			{
				trvApplicationTreeView.RemoveColumn(column);
			}
		}
		
		/// <summary>
		/// Close any opened SCADA application
		/// </summary>
		private void CloseApplication()
		{
			if(scadaApplication != null)
			{
				scadaApplication.Stop();
				scadaApplication = null;
			}
			
			CleanTreeView ();
	
			// Disable tooblar buttons
			disableToolBarButtons();
			
			// Clear text view
			txvTextView.Buffer.Text = "";
			
			this.ShowAll();
		}
		
		protected virtual void tbbOpen_Click (object sender, System.EventArgs e)
		{
			OpenApplication();
		}
		
		protected virtual void tbbClose_Click (object sender, System.EventArgs e)
		{
			CloseApplication();
		}
		
		protected void enableToolBarButtons()
		{
			tbbClose.Sensitive = true;		
			tbbStartServer.Sensitive = true;
		}
		
		protected void disableToolBarButtons()
		{
			tbbClose.Sensitive = false;
			tbbStartServer.Sensitive = false;
			tbbStopServer.Sensitive = false;
		}
		
		protected virtual void tbbStartServer_Click (object sender, System.EventArgs e)
		{
			startServers();		
		}
		
		protected virtual void tbbStopServer_Click (object sender, System.EventArgs e)
		{
			stopServers();
		}
		
		protected virtual void OnCloseActionActivated (object sender, System.EventArgs e)
		{
			CloseApplication();
		}
		
		protected virtual void OnSettingsActionActivated (object sender, System.EventArgs e)
		{
			Ideas.Scada.Server.Manager.Settings.SettingsMain settingsDialog = new Ideas.Scada.Server.Manager.Settings.SettingsMain();
			settingsDialog.Show();
		}
			
		protected void OnSaveActionActivated (object sender, System.EventArgs e)
		{
			string stringToSave = txvTextView.Buffer.Text;
			string file = String.Empty;
				
			if(scadaApplication != null)
			{
				file = scadaApplication.FilePath;
			}
					
			// Close application visualization
			CloseApplication();
			
			try
			{
				if(!String.IsNullOrEmpty(file))
				{
					// Write text to file
					System.IO.File.WriteAllText(file, stringToSave); 
					
					// Create a new scada application with the updated file
					LoadScadaFile(file);
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("Cannot save file.");
				Console.WriteLine(ex.Message);
			}
			
			
		}
	}
}