using System;
using System.Net;
using System.Xml;
using Mono.WebServer;
using System.Diagnostics;
using System.IO;
using log4net;

namespace Ideas.Scada.Common.Tags
{
	public class WebService
	{
		#region MEMBERS
		
		private string name;
		private Project project;
		private bool isStarted = false;
		private int serverPort;
		private string serverAddress;
		private string screensPath;
		private Process prcWebServer;
		private ApplicationServer webAppServer;	
		
		private static readonly ILog log = LogManager.GetLogger(typeof(WebService));
		
		#endregion MEMBERS
		
		/// <summary>
		/// Constructs the class
		/// </summary>	
		public WebService ()
		{
			
		}
		
		public WebService (
			XmlNode node, 
			Project parentProject)
		{
			log.Info("Reading WebService information from SCADA file...");
			
			string nodeName = node.Attributes["name"].Value;
			string nodeServerPort = node.Attributes["port"].Value;
			string nodeServerAddress = node.Attributes["address"].Value;
					
			this.Name = nodeName;
			this.Project = parentProject;
			this.ServerPort = Convert.ToInt32(nodeServerPort);
			this.ServerAddress = nodeServerAddress;
			this.ScreensPath = 
				parentProject.FilePath + "screens";
			
			log.Info("Finished reading WebService information from SCADA file.");
		}
		
		#region PUBLIC METHODS 
		
		
		/// <summary>
		/// Initiates tags webservice
		/// </summary>
		public void Start()
		{					
			try 
			{	
				log.Info("Configuring WebService...");
				
				// Create WebServer at ./Resources/WebApplication
				string serverRoot = 
					AppDomain.CurrentDomain.BaseDirectory +
					"Resources" + Path.DirectorySeparatorChar +
					"WebApplication";
							
				// Generate a webapp config file to start the WebServer
				GenerateAppConfigFile(serverRoot, screensPath);
				
				// Generate a web.config file with specific configuration for the application
				GenerateWebConfigFile(serverRoot);
					
				StartWebServerFromClass(serverRoot);
			}
			catch(Exception e)
			{
				log.Error("It was not possible to start the WebService at: " + this.ServerAddress + ":" + this.ServerPort);
				log.Error(e.Message);
				throw new Exception("ERROR: Could not start webservice: " + e.Message);
			}
		
		}
		
		/// <summary>
		/// Stops tags webservice
		/// </summary>
		public void Stop()
		{
			try
			{
				if(isStarted)
				{
					log.Info("Stoping WebService...");
					
					if(prcWebServer != null)
					{
						prcWebServer.Kill();
					}
					
					if(webAppServer != null)
					{
						webAppServer.Stop();
						webAppServer.UnloadAll();
					}
					
					log.Info("WebService stopped.");
				}
			}
			catch(Exception e)
			{
				log.Error("It was not possible to stop the WebService from: " + this.ServerAddress + ":" + this.ServerPort);
				log.Error(e.Message);
				throw new Exception("Could not stop webservice: " + e.Message);
			}
			
			
		}
		
		public string GenerateAppConfigFile (string targetPath, string screensPath)
		{
		
			string targetFile = 
				targetPath + 
				Path.DirectorySeparatorChar + "Ideas.webapp";
			
			log.Debug("Generating webapp configuration file at: " + targetFile);
			log.Debug("Screens path: " + screensPath);
			
			// Deletes file if it exists
			if(File.Exists(targetFile))
			{
				log.Debug("Deleting current webapp file at: " + targetFile);
				File.Delete(targetFile);
			}
			
			string fileContent = "";
			fileContent += "<apps>\n";
			fileContent += "	<web-application>\n";
			fileContent += "		<name>Root</name>\n";
			fileContent += "		<vpath>/</vpath>\n";
			fileContent += "		<path>" + targetPath + "</path>\n";
			fileContent += "	</web-application>\n";
			fileContent += "	<web-application>\n";
			fileContent += "		<name>Screens</name>\n";
			fileContent += "		<vpath>/screens</vpath>\n";
			fileContent += "		<path>" + screensPath + "</path>\n";
			fileContent += "	</web-application>\n";
			fileContent += "</apps>";
			
			log.Debug("Writing new webapp file at: " + targetFile);
			
			// Write the file back with the correct screens path
			StreamWriter writer = new StreamWriter(File.OpenWrite(targetFile));
			writer.Write(fileContent);
			writer.Close();
			
			log.Debug("Writing file finished. File closed.");
			
			// Return filename
			return targetFile;
		}
		
		public void GenerateWebConfigFile (string targetPath)
		{
			//XmlDocument xmlWebConfig = new XmlDocument();
			
			string initialScreenURL = 
				//"screens/" + 
				this.Project.Screens[this.Project.InitialScreenName].Name;
			
			string targetFile = 
				targetPath + 
				Path.DirectorySeparatorChar + "Web.config";
			
			string projectScreenList = GetProjectScreenList();
			
			log.Debug("Generating webconfig file...");
			
			// Deletes file if it exists
			if(File.Exists(targetFile))
			{
				File.Delete(targetFile);
				log.Debug("Deleted current Webconfig file at: " + targetFile);
			}
			
		
			
			string fileContent = "";
			fileContent += "<?xml version=\"1.0\"?> \n";
			fileContent += "<configuration> \n";
			fileContent += "<configSections>";
			fileContent += "	<sectionGroup name=\"system.web.extensions\" type=\"System.Web.Configuration.SystemWebExtensionsSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\">";
			fileContent += "		<sectionGroup name=\"scripting\" type=\"System.Web.Configuration.ScriptingSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\">";
			fileContent += "			<section name=\"scriptResourceHandler\" type=\"System.Web.Configuration.ScriptingScriptResourceHandlerSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"MachineToApplication\"/>";
			fileContent += "			<sectionGroup name=\"webServices\" type=\"System.Web.Configuration.ScriptingWebServicesSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\">";
			fileContent += "				<section name=\"jsonSerialization\" type=\"System.Web.Configuration.ScriptingJsonSerializationSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"Everywhere\"/>";
			fileContent += "				<section name=\"profileService\" type=\"System.Web.Configuration.ScriptingProfileServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"MachineToApplication\"/>";
			fileContent += "				<section name=\"authenticationService\" type=\"System.Web.Configuration.ScriptingAuthenticationServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"MachineToApplication\"/>";
			fileContent += "				<section name=\"roleService\" type=\"System.Web.Configuration.ScriptingRoleServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"MachineToApplication\"/>";
			fileContent += "			</sectionGroup>";
			fileContent += "		</sectionGroup>";
			fileContent += "	</sectionGroup>";
			fileContent += "</configSections>";
			fileContent += "	<system.web> \n";
			fileContent += "		<compilation defaultLanguage=\"C#\" debug=\"true\"> \n";
			fileContent += "			<assemblies> \n";
			fileContent += "			</assemblies> \n";
			fileContent += "		</compilation> \n";
			fileContent += "		<customErrors mode=\"Off\"> \n";
			fileContent += "		</customErrors> \n";
			fileContent += "		<authentication mode=\"None\"> \n";
			fileContent += "		</authentication> \n";
			fileContent += "		<authorization> \n";
			fileContent += "			<allow users=\"*\" /> \n";
			fileContent += "		</authorization> \n";
			fileContent += "		<httpHandlers> \n";
			fileContent += "		</httpHandlers> \n";
			fileContent += "		<trace enabled=\"false\" localOnly=\"true\" pageOutput=\"false\" requestLimit=\"10\" traceMode=\"SortByTime\" /> \n";
			fileContent += "		<sessionState mode=\"InProc\" cookieless=\"false\" timeout=\"20\" /> \n";
			fileContent += "		<globalization requestEncoding=\"utf-8\" responseEncoding=\"utf-8\" /> \n";
			fileContent += "		<pages> \n";
			fileContent += "		</pages> \n";
			fileContent += "	</system.web> \n";
			fileContent += "	<appSettings> \n";
			fileContent += "		<add key=\"InitialScreen\" value=\"" + initialScreenURL + "\"/> \n";
			fileContent += "		<add key=\"Screens\" value=\"" + projectScreenList + "\"/> \n";
			fileContent += "	</appSettings> \n";
			fileContent += "</configuration> \n";

			
			log.Debug("Writing new webconfig file at: " + targetFile);
			
			// Write the file back with the correct screens path
			StreamWriter writer = new StreamWriter(File.OpenWrite(targetFile));
			writer.Write(fileContent);
			writer.Close();
			
			log.Debug("Webconfig generation finished.");
		}
		
		
		/// <summary>
		/// TODO: write a comment.
		/// </summary>
		/// <param name="serverRoot"> 
		/// A string 
		/// </param>
		private void StartWebServerFromClass (string serverRoot)
		{
			
			XSPWebSource websource = new XSPWebSource(IPAddress.Any, this.ServerPort);
			
			webAppServer = new ApplicationServer(websource);
			
			// Adds application to the webserver
			webAppServer.AddApplicationsFromConfigFile(serverRoot + Path.DirectorySeparatorChar + "Ideas.webapp");
			
			// Starts server instance
			webAppServer.Start(false);
		}
		
		
		/// <summary>
		/// TODO: write a comment.
		/// </summary>
		/// <param name="serverRoot"> A string </param>
		private void StartWebServerFromExec (string serverRoot)
		{
			// Mount argument string
			string infoWebServerArguments = "" +
				" --nonstop" + 
				" --port " + serverPort + 
				" --address " + serverAddress +
				" --root \"" + serverRoot + "\"" +
				" --appconfigfile Ideas.webapp ";
				
			// Configurate XSP WebServer execution
			ProcessStartInfo infoWebServer = new ProcessStartInfo();	
			infoWebServer.FileName = "xsp2";
			infoWebServer.Arguments = infoWebServerArguments;
			infoWebServer.UseShellExecute = false;
			infoWebServer.RedirectStandardOutput = true;
			infoWebServer.RedirectStandardInput = true;
			
			log.Info("Client access address: http://" + this.ServerAddress + ":" + this.ServerPort);
			log.Info("Path to screens:" + this.ScreensPath);
			
			log.Info("WebService configured.");
			
			log.Debug("WebService configuration:");
			log.Debug(infoWebServer.FileName + " " + infoWebServer.Arguments);
							
			log.Info("Starting WebService...");
			
			// Executes XSP WebServer
			prcWebServer = Process.Start(infoWebServer);
			
			if(prcWebServer.HasExited)
			{
				throw new Exception("Unable to start WebServer. Check XSP installation.");
			}
			else
			{
				log.Info("WebService started.");
				this.isStarted = true;
			}
		}

		private string GetProjectScreenList()
		{
			string projScreen = "";
			
			foreach(Screen scr in this.Project.Screens)
			{
				projScreen += scr.Name + "," + scr.FilePath + ";";
			}
			
			projScreen = projScreen.TrimEnd(new char[] {';'});
			
			return projScreen;
		}
		
		#endregion
		
		#region DELEGATES
		
		// Declare a delegate type for processing a book:
   		public delegate void OnStartedDelegate(WebService webService);
		
		
		#endregion
		
		#region PROPERTIES
		
		public string Name 
		{
			get 
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		
		public int ServerPort 
		{
			get 
			{
				return this.serverPort;
			}
			set
			{
				this.serverPort = value;
			}
		}

		public Project Project {
			get {
				return this.project;
			}
			set {
				project = value;
			}
		}
		public string ServerAddress
		{
			get 
			{
				return this.serverAddress;
			}
			set
			{
				this.serverAddress = value;
			}
		}
		
		public string ScreensPath 
		{
			get 
			{
				return this.screensPath;
			}
			set
			{
				screensPath = value;
			}
		}
		
		public bool IsStarted {
			get {
				return this.isStarted;
			}
		}
		#endregion 
	}
	
}

