using System;
using System.Xml;
using System.IO;
using Ideas.Common;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using LumenWorks.Framework.IO.Csv;

namespace Ideas.Common.Tags
{
	public class DataBase
	{
		private string name;
		private string filePath;
		private IdeasScadaTagsDataBaseSourceType sourceType;
		private IDbConnection dbcon = null;
		private IDbCommand dbcmd = null;
		private string connectionString = "URI=file::memory:,version=3";
		
		/// <summary>
		/// Constructs the class
		/// </summary>	
		public DataBase ()
		{
					
		}
			
		~DataBase ()
		{
			this.Stop();	
		}
		
		public void Start()
		{
			dbcon = (IDbConnection) new SqliteConnection(connectionString);
			dbcon.Open();
			dbcmd = dbcon.CreateCommand();
			
			CreateDatabaseStructure();
			
			ReadSourceFile(); 
		}
		
		public void Stop()
		{
			if(dbcon != null)
			{
				if(dbcmd != null)
				{
					dbcmd.Dispose();
       				dbcmd = null;	
				}
				
				dbcon.Close();
				dbcon = null;
			}
		}
		
		void CreateDatabaseStructure ()
		{
			string sql = "";
			sql += "CREATE TABLE tb"+ this.Name + " ( ";
            sql += "TagName varchar(50), ";
            sql += "DataType varchar(50), ";
			sql += "DateTimeUpdate DATETIME, ";
			sql += "ClientAccess varchar(3), ";
			sql += "EngUnits varchar(32), ";
			sql += "Description varchar(255) ";
			sql += " ); ";
								
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}

		void ReadSourceFile ()
		{
			TextReader textReader = new StreamReader(this.FilePath);
			CsvReader csvReader = new CsvReader(textReader, true, ',');
					
			while(csvReader.ReadNextRecord())
			{
				// Convert CSV record to Tag
				Tag tag = new Tag();
				tag.TagName = csvReader[0];
				tag.DataType = csvReader[1];
				tag.ClientAccess = csvReader[2];
				tag.EngUnits = csvReader[3];
				tag.Description = csvReader[4];
				
            	InsertTagToDatabase(tag);				
			}
		}

		/// <summary>
		/// Insert tag information in memory database
		/// </summary>
		/// <param name="tag">
		/// A <see cref="Tag"/>
		/// </param>
		void InsertTagToDatabase(Tag tag)
		{
			string sql = "";
			sql += "INSERT INTO tb"+ this.Name + " ( ";
            sql += "TagName, ";
            sql += "DataType, ";
			sql += "DateTimeUpdate, ";
			sql += "ClientAccess, ";
			sql += "EngUnits, ";
			sql += "Description ";
			sql += " ) values ( ";
			sql += "@TagName, ";
            sql += "@DataType, ";
			sql += "NULL, ";
			sql += "@ClientAccess, ";
			sql += "@EngUnits, ";
			sql += "@Description ";
			sql += " );";
			
			IDbDataParameter parTagName = dbcmd.CreateParameter();
			parTagName.ParameterName = "TagName";
			parTagName.DbType = DbType.String;
			parTagName.Value = tag.TagName;
			dbcmd.Parameters.Add(parTagName);
			
			IDbDataParameter parDataType = dbcmd.CreateParameter();
			parDataType.ParameterName = "DataType";
			parDataType.DbType = DbType.String;
			parDataType.Value = tag.DataType;
			dbcmd.Parameters.Add(parDataType);
			
			IDbDataParameter parClientAccess = dbcmd.CreateParameter();
			parClientAccess.ParameterName = "ClientAccess";
			parClientAccess.DbType = DbType.String;
			parClientAccess.Value = tag.ClientAccess;
			dbcmd.Parameters.Add(parClientAccess);
			
			IDbDataParameter parEngUnits = dbcmd.CreateParameter();
			parEngUnits.ParameterName = "EngUnits";
			parEngUnits.DbType = DbType.String;
			parEngUnits.Value = tag.EngUnits;
			dbcmd.Parameters.Add(parEngUnits);
			
			IDbDataParameter parDescription = dbcmd.CreateParameter();
			parDescription.ParameterName = "Description";
			parDescription.DbType = DbType.String;
			parDescription.Value = tag.Description;
			dbcmd.Parameters.Add(parDescription);
			
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}
		
		#region P R O P E R T I E S
		
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
		
		public IdeasScadaTagsDataBaseSourceType SourceType 
		{
			get 
			{
				return this.sourceType;
			}
			set 
			{
				sourceType = value;
			}
		}		
		
		#endregion
		
		public static IdeasScadaTagsDataBaseSourceType convertSourceTypeFromString(string strSourceType)
		{
			switch(strSourceType.ToLower())
			{
				case "csv": 
					return IdeasScadaTagsDataBaseSourceType.CSV;
				default:
					throw new Exception("Unkown server script language: " + strSourceType);
			}		
		}
	}
	
		
	public enum IdeasScadaTagsDataBaseSourceType
	{
		CSV
	}
	
}

