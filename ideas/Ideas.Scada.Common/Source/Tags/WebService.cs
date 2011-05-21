using System;
using System.Net;
using System.Xml;
using Mono.WebServer;


namespace Ideas.Scada.Common.Tags
{
	public class WebService
	{
		private string name;
		private int serverPort;
		private string serverAddress;
		private string serverRootPath;
		private ApplicationServer webAppServer;	
		
		/// <summary>
		/// Constructs the class
		/// </summary>	
		public WebService ()
		{
			
		}
		
		public WebService (XmlNode xmlTagsWebserviceNode)
		{
			string nodeName = xmlTagsWebserviceNode.Attributes["name"].Value;
			string nodeServerPort = xmlTagsWebserviceNode.Attributes["port"].Value;
			string nodeServerAddress = xmlTagsWebserviceNode.Attributes["address"].Value;
					
			this.Name = nodeName;
			this.ServerPort = Convert.ToInt32(nodeServerPort);
			this.ServerAddress = nodeServerAddress;
			this.ServerRootPath = "screens";
		}
		
		#region PUBLIC METHODS
		
		
		/// <summary>
		/// Initiates tags webservice
		/// </summary>
		public void Start()
		{		
			XSPWebSource websource = new XSPWebSource(IPAddress.Any, this.ServerPort);
			
			webAppServer = new ApplicationServer(websource);
					
			// Adds application to the webserver
			//webAppServer.AddApplication("localhost", this.ServerPort, "/", serverRootPath);
			webAppServer.AddApplicationsFromCommandLine(this.ServerPort + ":/:" + serverRootPath);
			
			// Starts server instance
			webAppServer.Start(true);
		}
		
		/// <summary>
		/// Stops tags webservice
		/// </summary>
		public void Stop()
		{
			if(webAppServer != null)
			{
				webAppServer.Stop();
				webAppServer.UnloadAll();
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
		
		#endregion PROPERTIES
	}
	
}

