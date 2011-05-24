using System;
using System.Net;
using System.Xml;
using Mono.WebServer;
using System.Diagnostics;
using System.IO;


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
		
		#endregion
		
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
			string nodeName = node.Attributes["name"].Value;
			string nodeServerPort = node.Attributes["port"].Value;
			string nodeServerAddress = node.Attributes["address"].Value;
					
			this.Name = nodeName;
			this.Project = parentProject;
			this.ServerPort = Convert.ToInt32(nodeServerPort);
			this.ServerAddress = nodeServerAddress;
			this.ScreensPath = 
				parentProject.FilePath + "screens";
		}
		
		#region P U B L I C   M E T H O D S 
		
		
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
				
				// Executes XSP WebServer
				prcWebServer = Process.Start(infoWebServer);
						
				this.isStarted = true;
			}
			catch(Exception e)
			{
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
					prcWebServer.Kill();
				}
			}
			catch(Exception e)
			{
				throw new Exception("ERROR: Could not stop webservice: " + e.Message);
			}
		}
		
		public string GenerateAppConfigFile (string targetPath, string screensPath)
		{
			
			string targetFile = 
				targetPath + 
				Path.DirectorySeparatorChar + "Ideas.webapp";
			
			// Deletes file if it exists
			if(File.Exists(targetFile))
			{
				File.Delete(targetFile);
			}
			
			string fileContent = "";
			fileContent += "<apps>";
			fileContent += "	<web-application>";
			fileContent += "		<name>Root</name>";
			fileContent += "		<vpath>/</vpath>";
			fileContent += "		<path>.</path>";
			fileContent += "	</web-application>";
			fileContent += "	<web-application>";
			fileContent += "		<name>Screens</name>";
			fileContent += "		<vpath>/screens</vpath>";
			fileContent += "		<path>" + screensPath + "</path>";
			fileContent += "	</web-application>";
			fileContent += "</apps>";
			
			// Write the file back with the correct screens path
			StreamWriter writer = new StreamWriter(File.OpenWrite(targetFile));
			writer.Write(fileContent);
			writer.Close();
			
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

