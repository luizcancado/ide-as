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
//		private ApplicationServer webAppServer;	
		
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
				
				
	//			XSPWebSource websource = new XSPWebSource(IPAddress.Any, this.ServerPort);
	//			
	//			webAppServer = new ApplicationServer(websource);
	//					
	//			// Adds application to the webserver
	//			//webAppServer.AddApplication("localhost", this.ServerPort, "/", serverRootPath);
	//			webAppServer.AddApplicationsFromCommandLine(this.ServerPort + ":/:" + serverRootPath);
	//			
	//			// Starts server instance
	//			webAppServer.Start(true);
				
				log.Info("Configuring WebService...");
				
				// Create WebServer at ./Resources/WebApplication
				string serverRoot = 
					AppDomain.CurrentDomain.BaseDirectory +
					"Resources" + Path.DirectorySeparatorChar +
					"WebApplication";
							
				// Generates a config file to start the WebServer
				GenerateAppConfigFile(serverRoot, screensPath);
				
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
				
				log.Info("Client access address: http://" + this.ServerAddress + ":" + this.ServerPort);
				log.Info("Path to screens:" + this.ScreensPath);
				
				log.Info("WebService configured.");
				
				log.Debug("WebService configuration:");
				log.Debug(infoWebServer.FileName + " " + infoWebServer.Arguments);
								
				log.Info("Starting WebService...");
				
				// Executes XSP WebServer
				prcWebServer = Process.Start(infoWebServer);
				
				log.Info("WebService started.");
				
				this.isStarted = true;
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
//			if(webAppServer != null)
//			{
//				webAppServer.Stop();
//				webAppServer.UnloadAll();
//			}
			
			try
			{
				if(isStarted)
				{
					log.Info("Stoping WebService...");
					
					prcWebServer.Kill();
					
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
			fileContent += "		<path>.</path>\n";
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

