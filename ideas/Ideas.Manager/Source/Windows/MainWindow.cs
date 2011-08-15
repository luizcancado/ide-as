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
        /// <summary>
        /// The ScadaApplication opened by the manager
        /// </summary>
		private ScadaApplication scadaApplication;
        
        /// <summary>
        /// A system process instantiated as a Ideas.Scada.Server
        /// </summary>
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
		/// A <see cref="Configuration"/> with the manager settings
		/// </param>
		public MainWindow (Configuration config) : base(Gtk.WindowType.Toplevel)
		{
			Build ();
			
            // Check if any configuration was provided
			if(config == null)
			{
                // If not, initializes an empty scada application
				scadaApplication = null;
			}
			else
			{
                // Else, creates a new scada application based on a .scada file
				scadaApplication = new ScadaApplication(config.file);
				UpdateScadaApplicationsListTree ();
			}
		}
	
        /// <summary>
        /// Handler for the Delete event of the MainWindow
        /// </summary>
        /// <param name="sender">
        /// A <see cref="System.Object"/> reference to the event sender
        /// </param>
        /// <param name="e">
        /// A <see cref="DeleteEventArgs"/> with parameters passed to the event
        /// </param>
		protected void OnDeleteEvent (object sender, DeleteEventArgs e)
		{
			Application.Quit ();
			e.RetVal = true;
		}
		
        /// <summary>
        /// Handler for the Click event of the Menu: File -> Exit
        /// </summary>
        /// <param name="sender">
        /// A <see cref="System.Object"/> reference to the event sender
        /// </param>
        /// <param name="e">
        /// A <see cref="System.EventArgs"/> with parameters passed to the event
        /// </param>
		protected virtual void mnuFileExit_Click (object sender, System.EventArgs e)
		{
			this.Destroy();
			Application.Quit();
		}
		
        /// <summary>
        /// Handler for the Click event of the Menu: File -> Open
        /// </summary>
        /// <param name="sender">
        /// A <see cref="System.Object"/> reference to the event sender
        /// </param>
        /// <param name="e">
        /// A <see cref="System.EventArgs"/> with parameters passed to the event
        /// </param>
		protected virtual void OnOpenActionActivated (object sender, System.EventArgs e)
		{
			OpenApplication();
		}
		
		/// <summary>
		/// Loads the SCADA file. Converts XML to a ScadaApplication class
		/// </summary>
		/// <param name="scadafile">
		/// A <see cref="System.String"/> containing the filename of the .scada file to open
		/// </param>
		public void LoadScadaFile(string scadafile)
		{
            // Creates the scada aplication based on the scada file
			scadaApplication = new ScadaApplication(scadafile);
			
			// Updates the TreeView with the application structure
			UpdateScadaApplicationsListTree();
			
            // Fills the TextView with the content of the file
			UpdateTextView();
		}
		
		/// <summary>
        /// Fills the TextView with the content of the file
        /// </summary>
		protected void UpdateTextView ()
		{
            // Read the content of the scada file
			StreamReader scadaFile = new StreamReader(scadaApplication.FilePath);
			string fileString = scadaFile.ReadToEnd();
			
            // Fill the textview control
			txvTextView.Buffer.Text = fileString;
		}
		
        /// <summary>
        /// Updates the TreeView with the application structure
        /// </summary>
		protected void UpdateScadaApplicationsListTree ()
		{
			// Check if a scada app was already loaded
			if(scadaApplication == null)
			{
				return;
			}
			
			// Clean tree view
			CleanTreeView();
	        
            // Definition of the icons to appear for the tree nodes
			Gdk.Pixbuf icnApplicationIcon = IconFactory.LookupDefault("ideas").RenderIcon(new Style(), TextDirection.None, StateType.Active, IconSize.Menu, null, null);
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
						new object[] { icnFolderIcon, "DataSources" });
				
				foreach(DataSource datasource in project.Datasources)
				{
					applicationTreeStore.AppendValues(
	                      tagsDatabaseIter, 
	                      new object[] { icnTagsDatabaseIcon, datasource.Name });
				}
						
				Gtk.TreeIter tagsWebserviceIter = 
					applicationTreeStore.AppendValues(
						projectIter, 
						new object[] { icnFolderIcon, "WebService"});
				
				applicationTreeStore.AppendValues(
	                      tagsWebserviceIter, 
	                      new object[] { icnTagsWebserviceIcon, project.TagsWebService.Name });
			
			}
			
			trvApplicationTreeView.Model = applicationTreeStore;
			
			trvApplicationTreeView.ExpandAll();
				
			this.ShowAll();
		}
	
        /// <summary>
        /// Handler for the Click event of the Menu: Help -> About
        /// </summary>
        /// <param name="sender">
        /// A <see cref="System.Object"/> reference to the event sender
        /// </param>
        /// <param name="e">
        /// A <see cref="System.EventArgs"/> with parameters passed to the event
        /// </param>
		protected virtual void aboutAction_Click (object sender, System.EventArgs e)
		{
            // Show the AboutDialog
			About aboutDialog = new About();
			aboutDialog.Show();
		}
			
		/// <summary>
		/// Start an instance of Ideas.Scada.Server
		/// </summary>
		private void StartServersByProcess()
		{	
			// Define where the server exec is placed
			serverProcess = new Process();
			serverProcess.StartInfo.FileName = 
				@"mono";
			serverProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(scadaApplication.FilePath);
			
			// Pass the (already loaded) scada file as argument
			serverProcess.StartInfo.Arguments = 
                @"/home/luiz/Projects/Ideas/Ideas.Scada.Server/bin/Debug/Ideas.Scada.Server.exe " 
                    + scadaApplication.FilePath;
			
			// Redirects outputs 
			serverProcess.StartInfo.UseShellExecute = false;
			serverProcess.StartInfo.RedirectStandardOutput = true;
			serverProcess.StartInfo.RedirectStandardError = true;
			serverProcess.OutputDataReceived += OnProcessDataReceived;
			serverProcess.ErrorDataReceived += OnProcessDataReceived;
		
			// Start the Ideas Scada Server instance
			serverProcess.Start();
			
			// Start listening for output messages from the process
			serverProcess.BeginOutputReadLine();
			serverProcess.BeginErrorReadLine();
			
			// Manipulate toolbar buttons
			tbbStopServer.Sensitive = true;
			tbbStartServer.Sensitive = false;	
		}
		
		private void OnProcessDataReceived (object sender, DataReceivedEventArgs e)
		{
			
		}
		
		/// <summary>
		/// Stops the actual instance of Idea.Scada.Server
		/// </summary>
		private void StopServers()
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
			// Check if is there any app loaded or not
			if(scadaApplication != null)
			{
				scadaApplication.Stop();
				scadaApplication = null;
			}
			
			// Clean the application tree view
			CleanTreeView ();
	
			// Disable tooblar buttons
			disableToolBarButtons();
			
			// Clear text view
			txvTextView.Buffer.Text = "";
			
			// Refresh
			this.ShowAll();
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
		
		protected virtual void OnStartServerActionActivated (object sender, System.EventArgs e)
		{
			StartServersByProcess();		
		}
		
		protected virtual void OnStopServerActionActivated (object sender, System.EventArgs e)
		{
			StopServers();
		}
		
        /// <summary>
        /// Handler for the Click event of the Menu: File -> Close
        /// </summary>
        /// <param name="sender">
        /// A <see cref="System.Object"/> reference to the event sender
        /// </param>
        /// <param name="e">
        /// A <see cref="System.EventArgs"/> with parameters passed to the event
        /// </param>
		protected virtual void OnCloseActionActivated (object sender, System.EventArgs e)
		{
			CloseApplication();
		}
		
		protected virtual void OnSettingsActionActivated (object sender, System.EventArgs e)
		{
			Ideas.Scada.Server.Manager.Settings.SettingsMain settingsDialog = new Ideas.Scada.Server.Manager.Settings.SettingsMain();
			settingsDialog.Show();
		}
		
        /// <summary>
        /// Handler for the Click event of the Menu: File -> Save
        /// </summary>
        /// <param name="sender">
        /// A <see cref="System.Object"/> reference to the event sender
        /// </param>
        /// <param name="e">
        /// A <see cref="System.EventArgs"/> with parameters passed to the event
        /// </param>
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
				MessageDialog md = 
					new MessageDialog (
						null, 
						DialogFlags.Modal, 
						MessageType.Error, 
						ButtonsType.Ok, 
						ex.Message);
	            md.Run ();
			    md.Destroy();
			}
		}
	}
}