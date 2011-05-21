using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using Ideas.Scada.Common.Tags;

namespace Ideas.Scada.Common
{
	public class ScadaApplication
	{
		string rootPath;
		string filePath;
		string name;
		List<Project> projects = new List<Project>();
		
		/// <summary>
		/// Constructs the class from the xml Scada file
		/// </summary>
		/// <param name="scadafile">
		/// A <see cref="System.String"/>
		/// </param>
		public ScadaApplication (string scadafile)
		{
			LoadFromXML(scadafile);			
		}
		
		public void LoadFromXML(string scadafile)
		{
			try
			{
				XmlTextReader xmlTextReader = new XmlTextReader(scadafile);
				xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
				
				XmlDocument xmlScadaFile = new XmlDocument();
				
				xmlScadaFile.Load(xmlTextReader);
			
				XmlNodeList nodesList = xmlScadaFile.GetElementsByTagName("Application");
			
				this.name = nodesList[0].Attributes["name"].Value;
				
				// Saves the path to the Scada file
				this.filePath = scadafile;
				
				// Saves the root path
				this.rootPath = Path.GetDirectoryName(scadafile);
				
				// Verifies if there is only one Application settings for the file
				if(nodesList.Count > 1)
				{
					throw new Exception("This file has more than one Application settings. Please make sure that there is only one Application settings for each file.");
				}
								
				// Loads application settings
				LoadProjectSettings(xmlScadaFile);
							

			}
			catch(Exception e)
			{
				throw new Exception("The configuration file is invalid or corrupted. " + e.Message);
			}
		}
		
		private void LoadProjectSettings(XmlDocument xmlScadaFile)
		{
			XmlNodeList nodesList = xmlScadaFile.GetElementsByTagName("Project");
			
			foreach(XmlNode node in nodesList)
			{
				Project projectToAdd = new Project(node);
				
				Projects.Add(projectToAdd);
			}
		}
		
		public void Start()
		{
			foreach(Project project in this.Projects)
			{
				project.TagsWebService.Start();
				project.TagsDatabase.Start();
			}		
		}
				
		public void Stop()
		{
			foreach(Project project in this.Projects)
			{
				project.TagsWebService.Stop();
				project.TagsDatabase.Stop();
			}	
		}
		
		
		#region PROPERTIES
		
		public string RootPath 
		{
			get 
			{
				return this.rootPath;
			}
		}

		public string FilePath 
		{
			get 
			{
				return this.filePath;
			}
		}
		
		public List<Project> Projects 
		{
			get 
			{
				return this.projects;
			}
		}
		
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
		
		#endregion PROPERTIES
	}
}

