using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using Ideas.Scada.Common.Tags;
using Ideas.Scada.Common.DataSources;

namespace Ideas.Scada.Common
{
	public class Project
	{
		#region MEMBERS
		
		private string name;
		private string filePath;	
		private List<Screen> screens = new List<Screen>();
		private List<DataSource> datasources = new List<DataSource>();
		private DataBase tagsDatabase = new DataBase();
		private WebService tagsWebService;
		private string initialScreenName;
			
		#endregion
		
		/// <summary>
		/// Constructs the class from the Xml Scada file
		/// </summary>
		public Project (XmlNode node, string appPath)
		{
			string nodeName = node.Attributes["name"].Value;
			string nodePath = node.Attributes["path"].Value;
			string initScreen = node.Attributes["initscreen"].Value;
			
			this.Name = nodeName;
			this.FilePath = appPath + Path.DirectorySeparatorChar;
			this.FilePath += nodePath + Path.DirectorySeparatorChar;
			this.InitialScreenName = initScreen;
					
			foreach(XmlNode childNode in node.ChildNodes)
			{
				LoadProjectScreen(childNode);
				LoadProjectTagsDatabase(childNode);
				LoadProjectTagsWebservice(childNode);
			}
		}
		
		public Project ()
		{
			
		}
		
		/// <summary>
		/// Load a screen configuration configuration if it is a correspondant Xml node
		/// </summary>
		/// <param name="xmlProjectNode">
		/// A <see cref="XmlNode"/>
		/// </param>
		private void LoadProjectScreen(XmlNode xmlScreenNode)
		{
			if(xmlScreenNode.Name.ToLower() == "screen")
			{
				Screen screenToAdd = new Screen(xmlScreenNode, this);
				
				this.Screens.Add(screenToAdd);
			}
		}
		
		/// <summary>
		/// Load the Tags Database configuration if it is a correspondant Xml node
		/// </summary>
		/// <param name="xmlProjectNode">
		/// A <see cref="XmlNode"/>
		/// </param>
		private void LoadProjectTagsDatabase(XmlNode node)
		{
			if(node.Name.ToLower() == "datasource")
			{
				// Instantiate a new tag database to be added to the project
				DataSource tagsDatabaseToAdd = 
					CreateDataSourceFromXMLNode(node);
							
				// Adds the new tags database to the current project
				this.Datasources.Add(tagsDatabaseToAdd);
				
				// Create datasource structure in tagsdatabase
				this.TagsDatabase.AddDataSource(tagsDatabaseToAdd);
			}
		}

		private DataSource CreateDataSourceFromXMLNode (XmlNode node)
		{
			DataSource retDataSource = null;
			string type = "";
				
			// Get datasource type string
			try
			{
				type = node.Attributes["type"].Value.ToLower();
			}
			catch(Exception e)
			{
				throw new Exception("Type not defined for one of the datasources.");
			}
			
			// Associate it with a datasource object
			switch(type)
			{
				case "openopc":
					retDataSource = new OpenOPC(node, this);
					break;
				default:
					throw new Exception("Unknown datasource type: " + type);
			}
			
			return retDataSource;
		}
		
		/// <summary>
		/// Load the Tags Webservice configuration if it is a correspondant Xml node
		/// </summary>
		/// <param name="xmlProjectNode">
		/// A <see cref="XmlNode"/>
		/// </param>
		private void LoadProjectTagsWebservice(XmlNode xmlTagsWebserviceNode)
		{
			if(xmlTagsWebserviceNode.Name.ToLower() == "webservice")
			{
				// Instantiate a new tag database to be added to the project
				WebService tagsWebservice = new WebService(xmlTagsWebserviceNode, this);
												
				this.TagsWebService = tagsWebservice;
			}
		}
		
		#region PROPERTIES
		
		public string Name 
		{
			get 
			{
				return this.name;
			}
			set
			{
				name = value;
			}
		}

		public string FilePath 
		{
			get 
			{
				return this.filePath;
			}
			set
			{
				filePath = value;
			}
		}

		public string InitialScreenName {
			get {
				return this.initialScreenName;
			}
			set {
				initialScreenName = value;
			}
		}		
		
		public List<Screen> Screens 
		{
			get 
			{
				return this.screens;
			}
			set 
			{
				screens = value;
			}
		}
		
		public DataBase TagsDatabase 
		{
			get 
			{
				return this.tagsDatabase;
			}
			set 
			{
				tagsDatabase = value;
			}
		}

		public WebService TagsWebService 
		{
			get 
			{
				return this.tagsWebService;
			}
			set 
			{
				tagsWebService = value;
			}
		}
		
		public List<DataSource> Datasources {
			get {
				return this.datasources;
			}
			set {
				datasources = value;
			}
		}
		
		#endregion PROPERTIES
	}
}

