using System.Net;
using Mono.WebServer;


namespace Ideas.Common.Tags
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
		
		#region PUBLIC METHODS
		
		
		/// <summary>
		/// Initiates tags webservice
		/// </summary>
		public void Start()
		{		
			XSPWebSource websource = new XSPWebSource(IPAddress.Any, this.ServerPort);
			
			webAppServer = new ApplicationServer(websource, serverRootPath);
					
			// Adds application to the webserver
			webAppServer.AddApplication("localhost", this.ServerPort, "/", serverRootPath);
			
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

