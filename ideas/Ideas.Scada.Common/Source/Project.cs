using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using Ideas.Scada.Common.Tags;


namespace Ideas.Scada.Common
{
	public class Project
	{
		private string name;
		private string filePath;
		
		List<Screen> screens = new List<Screen>();
		
		DataBase tagsDatabase;
		WebService tagsWebService;
		
		/// <summary>
		/// Constructs the class from the Xml Scada file
		/// </summary>
		public Project (XmlNode node)
		{
			string nodeName = node.Attributes["name"].Value;
			string nodePath = node.Attributes["path"].Value;
			
			this.Name = nodeName;
			this.FilePath = nodePath + Path.DirectorySeparatorChar;
			
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
				Screen screenToAdd = new Screen(xmlScreenNode);
				
				this.Screens.Add(screenToAdd);
			}
		}
		
		/// <summary>
		/// Load the Tags Database configuration if it is a correspondant Xml node
		/// </summary>
		/// <param name="xmlProjectNode">
		/// A <see cref="XmlNode"/>
		/// </param>
		private void LoadProjectTagsDatabase(XmlNode xmlTagsDatabasetNode)
		{
			if(xmlTagsDatabasetNode.Name.ToLower() == "tagsdatabase")
			{
				// Instantiate a new tag database to be added to the project
				DataBase tagsDatabaseToAdd = new DataBase(xmlTagsDatabasetNode);
							
				// Adds the new tags database to the current project
				this.TagsDatabase = tagsDatabaseToAdd;
			}
		}
		
		/// <summary>
		/// Load the Tags Webservice configuration if it is a correspondant Xml node
		/// </summary>
		/// <param name="xmlProjectNode">
		/// A <see cref="XmlNode"/>
		/// </param>
		private void LoadProjectTagsWebservice(XmlNode xmlTagsWebserviceNode)
		{
			if(xmlTagsWebserviceNode.Name.ToLower() == "tagswebservice")
			{
				// Instantiate a new tag database to be added to the project
				WebService tagsWebserviceToAdd = new WebService(xmlTagsWebserviceNode);
												
				this.TagsWebService = tagsWebserviceToAdd;
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
		
		#endregion PROPERTIES
	}
}

