using System;
using System.Xml;
using System.IO;
using Ideas.Common;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;

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
			dbcon = (IDbConnection) new SqliteConnection(connectionString);
			dbcon.Open();
			dbcmd = dbcon.CreateCommand();
			
			CreateDatabaseStructure();
			
			
			
		}
			
		~DataBase ()
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
			sql += "DateTimeUpdate DATETIME, "
			sql += "ClientAccess varchar(3), ";
			sql += "EngUnits varchar(32), ";
			sql += "Description varchar(255) ";
			sql += " ); ";
								
			dbcmd.CommandText = sql;
			
			IDataReader reader = dbcmd.ExecuteNonQuery();
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

