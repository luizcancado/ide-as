using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using Ideas.Scada.Common.Tags;
using log4net;
using System.Threading;

namespace Ideas.Scada.Common
{
	public class ScadaApplication
	{
		string rootPath;
		string filePath;
		string name;
		ProjectCollection projects = new ProjectCollection();
		private static readonly ILog log = LogManager.GetLogger(typeof(ScadaApplication));
		
		/// <summary>
		/// Constructs the class from the xml Scada file
		/// </summary>
		/// <param name="scadafile">
		/// A <see cref="System.String"/>
		/// </param>
		public ScadaApplication (string scadafile)
		{
			try 
			{
				log.Info("Loading scada application...");
				
				LoadFromXML(scadafile);
				
				log.Info("Finished loading scada application");
			}
			catch(Exception e)
			{
				string errMessage = "Could not load SCADA application";
				
				log.Error(errMessage);
				log.Error(e.Message);
				
				throw new Exception(errMessage + ": " + e.Message);
			}
		}
		
		public void LoadFromXML(string scadafile)
		{
			try
			{
				log.Info("Loading SCADA file: " + scadafile );
				
				log.Debug("Reading SCADA file...");
				
				string xmlContent = "";
				
				using (TextReader textReader = new StreamReader(scadafile))
				{
					xmlContent = textReader.ReadToEnd();
					textReader.Close();
					textReader.Dispose();
				}
				
				log.Debug("Reading finished.");
				
				log.Debug("Interpreting XML content...");
				
				XmlDocument xmlScadaFile = new XmlDocument();
				
				xmlScadaFile.LoadXml(xmlContent);
				
				log.Debug("Interpreting XML content finished.");
				
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
				
				log.Debug("Getting projects information...");
				
				// Loads application settings
				LoadProjectSettings(xmlScadaFile);
				
				log.Debug("Getting projects information finished.");
				
				log.Info("Loading finished.");
			}
			catch(Exception e)
			{
				throw new Exception("The configuration file is invalid or corrupted. " + e.Message);
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlScadaFile">
		/// A <see cref="XmlDocument"/> containing a Scadafile
		/// </param>
		private void LoadProjectSettings(XmlDocument xmlScadaFile)
		{
			XmlNodeList nodesList = xmlScadaFile.GetElementsByTagName("Project");
			
			foreach(XmlNode node in nodesList)
			{
				string projectName = node.Attributes["name"].Value;
				
				log.Info("Loading project: " + projectName);
				
				Project projectToAdd = new Project(node, this.rootPath);
				
				log.Info(projectName + " loaded.");
				
				Projects.Add(projectToAdd);
			} 
		}
		
		public void Start()
		{
			try
			{
				log.Info("Starting scada application " + this.Name);
				
				foreach(Project project in this.Projects)
				{			
					log.Info("Starting project " + project.Name + "... ");
				
					project.Start();
					
					log.Info("Project " + project.Name + " is started. ");
				}

				log.Info("Server started with the application: " + this.Name);
				
				Thread.Sleep(0);
			}
			catch(Exception e)
			{
				// Logs error for the application failed to start
				log.Error("Failed to start application: " + this.Name);
				log.Error(e.Message);
				
				// Rethrows exception to upper level
				throw e;
			}
		}
				
		public void Stop()
		{
			foreach(Project project in this.Projects)
			{
				log.Info("Stopping project " + project.Name + "... ");
				
				project.Stop();
				
				log.Info("Project " + project.Name + " is stopped. ");
			}
		}
		
		public void WriteTag(string projectName, string datasourceName, string tagName, string tagValue)
		{
			Tag tag = new Tag();
			tag.datasource = datasourceName;
			tag.name = tagName;
			tag.value = tagValue;
			
			WriteTag(projectName, tag);
		}
		
		/// <summary>
		/// Writes a value to a tag
		/// </summary>
		/// <param name="projectName">
		/// A <see cref="System.String"/> with the name of the Project <see cref="Ideas.Scada.Common.Project"/>
		/// </param>
		/// <param name="tag">
		/// A <see cref="Tag"/>
		/// </param>
		public void WriteTag(string projectName, Tag tag)
		{
			Project project = this.Projects[projectName];
			
			if(project != null)
			{
				//project.WriteTag(Tag tag);
				
				// IMHERE: Luiz
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
		
		public ProjectCollection Projects 
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

