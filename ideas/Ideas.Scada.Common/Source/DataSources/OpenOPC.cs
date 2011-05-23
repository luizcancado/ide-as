using System;
using System.IO;
using System.Diagnostics;
using Ideas.Scada.Common.Tags;
using System.Collections.Generic;
using System.Xml;

namespace Ideas.Scada.Common.DataSources
{
	public class OpenOPC : DataSource
	{
		private Process prcOpenOPC;
		private StreamReader strReader;
		private string serverHost;
		private string openOPCPath;
		private string pythonPath;
		private string serverInstance;
		
		private bool isopen = false;
		
		public OpenOPC () : base()
		{
			base.Type = DataSourceType.OpenOPC;
			base.Name = "openopc";
			base.Tags = null;
			this.pythonPath = "python";
			this.openOPCPath = "/home/luiz/Desktop/OpenOPC/src/opc.py";
			this.serverHost = "192.168.0.169";
			this.serverInstance = "Kepware.KEPServerEX.V5";
			
		}
		
		public OpenOPC (
			string name,
			string server, 
			string instance,
			string pythonpath,
			string openopcpath,
			TagGroup tagslist
			) : this()
		{
			base.Name = name;
			base.Tags = tagslist;
			this.serverHost = server;
			this.serverInstance = instance;
			this.pythonPath = pythonpath;
			this.openOPCPath = openopcpath;
			
		}
		
		public OpenOPC (
			XmlNode node, 
			string projectPath
			) : base(node, projectPath)
		{
			this.serverHost = node.Attributes["server"].Value;
			this.serverInstance = node.Attributes["instance"].Value;
			this.pythonPath = node.Attributes["pythonpath"].Value;
			this.openOPCPath = node.Attributes["openopcpath"].Value;
			
			// Convert datasource file content to the Tag structure
			base.ReadSourceFile();
		}
		
		public override void Open()
		{
			try
			{
				// Mount argument string
				string infoOpenOPCArguments = 
					openOPCPath +
					" -H " + serverHost + 
					" -s " + serverInstance + 
					" -r " + GetTagsAddressList();
				
				// Configurate OpenOPC client execution
				ProcessStartInfo infoOpenOPC = new ProcessStartInfo();	
				infoOpenOPC.FileName = pythonPath;
				infoOpenOPC.Arguments = infoOpenOPCArguments;
				infoOpenOPC.RedirectStandardOutput = true;
				infoOpenOPC.UseShellExecute = false;
				
				// Executes OpenOPC client script
				prcOpenOPC = Process.Start(infoOpenOPC);
				strReader =  prcOpenOPC.StandardOutput;
				
				// Set instance with as with an open connection
				isopen = true;
			}
			catch(Exception e)
			{
				throw new Exception("Could not connect OpenOPC client to server: " + e.Message);
			}
		}
		
		public override void Close()
		{
			try
			{
				if(this.IsOpen)
				{
					strReader.Close();
					strReader.Dispose();
					
					prcOpenOPC.Kill();
				}
			}
			catch(Exception e)
			{
				throw new Exception("Could not close OpenOPC client: " + e.Message);
			}
		}
			
		public override TagGroup Read()
		{
            				
			try
            {
				string retString = strReader.ReadLine();
            }
            catch ( Exception e )
            {
				string errorMessage = "Could not read tag values from datasource ";
				errorMessage += "'" + this.Name + "': ";
				errorMessage += e.Message;
				
                throw new Exception(errorMessage);
            }
			
			return new TagGroup();
		}
		
		public override void Write(Tag tag)
		{
			// Mount argument string
			string infoOpenOPCWriteArguments = 
				openOPCPath +
				" -H " + serverHost + 
				" -s " + serverInstance + 
				" -w " + tag.address + " " + tag.value;
			
			// Configurate OpenOPC client execution
			ProcessStartInfo infoOpenOPCWrite = new ProcessStartInfo();	
			infoOpenOPCWrite.FileName = pythonPath;
			infoOpenOPCWrite.Arguments = infoOpenOPCWriteArguments;
			infoOpenOPCWrite.UseShellExecute = false;
			
			// Executes OpenOPC client script
			prcOpenOPC = Process.Start(infoOpenOPCWrite);
		}
		
		private string GetTagsAddressList ()
		{
			string addresslist = "";
			
			// Create a string with the tags addresses separated by space 
			foreach (Tag t in base.Tags)
			{
				addresslist += " " + t.address;
			}
			
			return addresslist;
		}
		
		public bool IsOpen {
			get {
				return this.isopen;
			}
			set {
				isopen = value;
			}
		}

		public string OpenOPCPath {
			get {
				return this.openOPCPath;
			}
			set {
				openOPCPath = value;
			}
		}

		public string PythonPath {
			get {
				return this.pythonPath;
			}
			set {
				pythonPath = value;
			}
		}

		public string ServerHost {
			get {
				return this.serverHost;
			}
			set {
				serverHost = value;
			}
		}

		public string ServerInstance {
			get {
				return this.serverInstance;
			}
			set {
				serverInstance = value;
			}
		}
	}
}

