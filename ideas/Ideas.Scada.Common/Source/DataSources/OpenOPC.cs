using System;
using System.IO;
using System.Diagnostics;
using Ideas.Scada.Common.Tags;
using System.Collections.Generic;
using System.Xml;
using LumenWorks.Framework.IO.Csv;
using System.Globalization;
using log4net;

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
		private static readonly ILog log = LogManager.GetLogger(typeof(OpenOPC));
		
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
			Project parentProject
			) : base(node, parentProject)
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
					" -r " + GetTagsAddressList() +
					" -L 1 -o csv ";
				
				// Configurate OpenOPC client execution
				prcOpenOPC = new Process();
				prcOpenOPC.StartInfo.FileName = pythonPath;
				prcOpenOPC.StartInfo.Arguments = infoOpenOPCArguments;
				prcOpenOPC.StartInfo.RedirectStandardOutput = true;
				prcOpenOPC.StartInfo.UseShellExecute = false;
				
				// Executes OpenOPC client script
				prcOpenOPC.OutputDataReceived += OnOutputDataReceived;
				prcOpenOPC.Start();
				prcOpenOPC.BeginOutputReadLine();
				//strReader =  prcOpenOPC.StandardOutput;
				
				// Call base class Open method
				base.Open();
			}
			catch(Exception e)
			{
				throw new Exception("Could not connect OpenOPC client to server: " + e.Message);
			}
		}
		
		private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			UpdateTagsFromCsv(e.Data);
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
					
					base.Close();
				}
			}
			catch(Exception e)
			{
				throw new Exception("Could not close OpenOPC client: " + e.Message);
			}
		}

		void UpdateTagsFromCsv (string data)
		{
			TextReader textReader = new StringReader(data);
			CsvReader csvReader = new CsvReader(textReader, false, ',');
					
			while(csvReader.ReadNextRecord())
			{
				// The content read has the form:
				// Address,Value,Quality,LastChangeDate
				// Channel1.S7_1200__1214C.S1,False,Good,06/06/11 04:29:57
				
				// Find tag by its address
				Tag tag = this.Tags.GetByTagAddress(csvReader[0]);
				
				if(tag != null)
				{
					// If it was found, update tag's value and last update date
					if(tag.value == null || tag.value.Trim() != csvReader[1].Trim())
					{
						tag.value = csvReader[1];
						tag.lastupdate = csvReader[3];
						//Console.WriteLine(tag);
						log.Info("Read new tag value: " + tag.name + " = " + tag.value);
						
						base.UpdateDataBase(tag);
					}
				}
			}
		}
			
		public override TagGroup Read()
		{
            				
			try
            {
				
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
			string addresslist = " ";
			
			// Create a string with the tags addresses separated by space 
			foreach (Tag t in base.Tags)
			{
				addresslist += "\"" + t.address + "\" ";
			}
			
			return addresslist;
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

