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
		private string name;
		private bool isStarted = false;
		private int serverPort;
		private string serverAddress;
		private string serverRootPath;
		private Process prcWebServer;
//		private ApplicationServer webAppServer;	
		
		/// <summary>
		/// Constructs the class
		/// </summary>	
		public WebService ()
		{
			
		}
		
		public WebService (
			XmlNode node, 
			string projectPath)
		{
			string nodeName = node.Attributes["name"].Value;
			string nodeServerPort = node.Attributes["port"].Value;
			string nodeServerAddress = node.Attributes["address"].Value;
					
			this.Name = nodeName;
			this.ServerPort = Convert.ToInt32(nodeServerPort);
			this.ServerAddress = nodeServerAddress;
			this.ServerRootPath = 
				projectPath + Path.DirectorySeparatorChar;
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
				
				// Mount argument string
				string infoWebServerArguments = "" +
					" --port " + serverPort + 
					" --address " + serverAddress + 
					" --root \"" + serverRootPath + "\"";
				
				// Configurate XSP WebServer execution
				ProcessStartInfo infoWebServer = new ProcessStartInfo();	
				infoWebServer.FileName = "xsp2";
				infoWebServer.Arguments = infoWebServerArguments;
				infoWebServer.UseShellExecute = false;
				
				// Executes XSP WebServer
				prcWebServer = Process.Start(infoWebServer);
				
				// Copy infrastructural files to WebServer root directory
				MountWebSiteStructure();
				
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
					MountWebSiteStructure();
					prcWebServer.Kill();
				}
			}
			catch(Exception e)
			{
				throw new Exception("ERROR: Could not stop webservice: " + e.Message);
			}
		}

		public void MountWebSiteStructure ()
		{
			string destPath = 
				this.serverRootPath + Path.DirectorySeparatorChar;
			
			string sourcePath = 
				AppDomain.CurrentDomain.BaseDirectory +
				"Resources" + Path.DirectorySeparatorChar +
				"TagsWebservice" + Path.DirectorySeparatorChar;
			
			string[] sourceFiles = Directory.GetFiles(sourcePath);
						
			foreach(string s in sourceFiles)
			{
				string filename = Path.GetFileName(s);
				File.Copy(s, destPath + filename);	
			}
			
		}
		
		public void UnMountWebSiteStructure ()
		{
			string destPath = 
				this.serverRootPath + Path.DirectorySeparatorChar;
			
			string sourcePath = 
				AppDomain.CurrentDomain.BaseDirectory +
				"Resources" + Path.DirectorySeparatorChar +
				"TagsWebservice" + Path.DirectorySeparatorChar;
			
			string[] sourceFiles = Directory.GetFiles(sourcePath);
						
			foreach(string s in sourceFiles)
			{
				string filename = Path.GetFileName(s);
				File.Delete(destPath + filename);	
			}
			
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
		
		public string ServerRootPath 
		{
			get 
			{
				return this.serverRootPath;
			}
			set
			{
				serverRootPath = value;
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

