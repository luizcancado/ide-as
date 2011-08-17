using System;
using System.Collections.Generic;
using System.IO;
using Ideas.Scada.Common.Tags;
using LumenWorks.Framework.IO.Csv;
using System.Xml;
using System.Timers;

namespace Ideas.Scada.Common.DataSources
{
	public abstract class DataSource
	{
		private string name;
		private string filePath;
		private Project project;
		private DataSourceFileType fileType;
		private DataSourceType type;		
		private TagGroup tags = new TagGroup();
		protected bool isOpen;
		protected Timer updateDataBaseTimer;
		
		public DataSource ()
		{
			// Create a Timer instance
			updateDataBaseTimer = new Timer (500);
			updateDataBaseTimer.Elapsed += OnUpdateDataBaseTimerElapsed;
			updateDataBaseTimer.Enabled = false;
			
			this.Name = "";
			this.FilePath = "";
			this.Project = null;
			
		}
		
		public DataSource(
			XmlNode node, 
			Project parentProject) : this()
		{
			this.Name = node.Attributes["name"].Value;
			this.Project = parentProject;
			
			filePath = 
				parentProject.FilePath +
				"datasources" + Path.DirectorySeparatorChar +
				node.Attributes["filename"].Value;
			
			string fileType = node.Attributes["filetype"].Value;	
			this.FileType = ConvertToDataSourceFileType(fileType);
		}
		
		public virtual void Open()
		{
			// Set instance as with an open connection
			updateDataBaseTimer.Enabled = true;
			//readTimer.Start();
			this.IsOpen = true;
		}
		
		public virtual void Close()
		{
			// Set instance as with an closed connection
			updateDataBaseTimer.Enabled = false;
			this.IsOpen = false;
		}
		
		public abstract TagGroup Read();
		
		public abstract void Write(Tag tag);
		
		private void OnUpdateDataBaseTimerElapsed(object sender, ElapsedEventArgs args)
		{
//			foreach(Tag t in this.Tags)
//			{
//				this.Project.Write(t);
//			}
		}
		
		protected void ReadSourceFile ()
		{
			switch(this.FileType)
			{
				case DataSourceFileType.CSV:
					ReadCSVFile();
					break;
				case DataSourceFileType.None:
					break;
				default:
					throw new Exception("Unknown datasource file type");
			}
		}
		
		private void ReadCSVFile()
		{
			TextReader textReader = new StreamReader(this.FilePath);
			CsvReader csvReader = new CsvReader(textReader, true, ',');
					
			while(csvReader.ReadNextRecord())
			{
				// Convert CSV record to Tag
				Tag tag = new Tag();
				tag.datasource = this.Name;
				tag.name = csvReader["TagName"];
				tag.address = csvReader["Address"];
				tag.datatype = csvReader["DataType"];
				tag.clientaccess = csvReader["ClientAccess"];
				tag.engunits = csvReader["EngUnits"];
				tag.description = csvReader["Description"];
				
            	this.Tags.Add(tag);
			}
		}
		
		public static DataSourceFileType ConvertToDataSourceFileType(string strSourceType)
		{
			switch(strSourceType.ToLower())
			{
				case "csv": 
					return DataSourceFileType.CSV;
				case "none":
					return DataSourceFileType.None;
				default:
					throw new Exception("Unkown server script language: " + strSourceType);
			}		
		}

		public void UpdateDataBase (Tag tag)
		{
			this.Project.Write(tag);
		}
		
		public string Name {
			get {
				return this.name;
			}
			set {
				name = value;
			}
		}

		public DataSourceType Type {
			get {
				return this.type;
			}
			set {
				type = value;
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
		
		public string FilePath 
		{
			get 
			{
				return this.filePath;
			}
			set
			{
				this.filePath = value;
			}
		}
		
			public DataSourceFileType FileType 
		{
			get 
			{
				return this.fileType;
			}
			set 
			{
				fileType = value;
			}
		}		
		
		public TagGroup Tags {
			get {
				return this.tags;
			}
			set {
				tags = value;
			}
		}

		public bool IsOpen {
			get {
				return this.isOpen;
			}
			set {
				isOpen = value;
			}
		}
	}
	
	public enum DataSourceFileType
	{
		None,
		CSV
	}
}	
		