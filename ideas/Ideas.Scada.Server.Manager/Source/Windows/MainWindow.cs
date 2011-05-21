using System;
using Gtk;
using System.Collections.Generic;
using Ideas.Common;
using Ideas.Server.Manager;
using Ideas.Scada.Common.DataSources;

public partial class MainWindow : Gtk.Window
{
	static ScadaApplication scadaApplication;
	
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
		openApplication();
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
	}
	
	protected void UpdateScadaApplicationsListTree ()
	{
		// Check if a scada app was already loaded
		if(scadaApplication == null)
		{
			return;
		}
		
		// Closes current application if any
		closeApplication();
		
		TreeViewColumn colApplication = new TreeViewColumn ();
		colApplication.Title = "Application";
 
		Gtk.CellRendererText cellApplication = new Gtk.CellRendererText ();
		
		colApplication.PackStart (cellApplication, true);
  
		trvApplicationTreeView.AppendColumn (colApplication);
		
		colApplication.AddAttribute (cellApplication, "text", 0);
		
		
		TreeStore applicationTreeStore = new TreeStore(typeof (string));
		Gtk.TreeIter applicationIter = applicationTreeStore.AppendValues (scadaApplication.Name);	
		
		foreach(Project project in scadaApplication.Projects)
		{
			Gtk.TreeIter projectIter = applicationTreeStore.AppendValues(applicationIter, project.Name);
			
			Gtk.TreeIter screensIter = applicationTreeStore.AppendValues(projectIter, "Screens");
			
			foreach(Screen screen in project.Screens)
			{
				applicationTreeStore.AppendValues(screensIter, screen.Name);
			}
			
			Gtk.TreeIter tagsDatabaseIter = applicationTreeStore.AppendValues(projectIter, "Tags Database");
			applicationTreeStore.AppendValues(tagsDatabaseIter, project.TagsDatabase.Name);
			
			Gtk.TreeIter tagsWebserviceIter = applicationTreeStore.AppendValues(projectIter, "Tags WebService");
			applicationTreeStore.AppendValues(tagsWebserviceIter, project.TagsWebService.Name);
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
	
	private void startServers()
	{	
		scadaApplication.Start();
		
		tbbStopServer.Sensitive = true;
		tbbStartServer.Sensitive = false;		
	}
	
	private void stopServers()
	{
		scadaApplication.Stop();
		
		tbbStopServer.Sensitive = false;
		tbbStartServer.Sensitive = true;
	}
	
	private void openApplication()
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
	
	private void closeApplication()
	{
		foreach(TreeViewColumn column in trvApplicationTreeView.Columns)
		{
			trvApplicationTreeView.RemoveColumn(column);
		}

		// Disable tooblar buttons
		disableToolBarButtons();
		
		this.ShowAll();
	}
	
	protected virtual void tbbOpen_Click (object sender, System.EventArgs e)
	{
		openApplication();
	}
	
	protected virtual void tbbClose_Click (object sender, System.EventArgs e)
	{
		closeApplication();
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
	
	
	
}
