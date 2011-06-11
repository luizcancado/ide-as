using System;
using System.Data;
using System.IO;
using System.Xml;
using Mono.Data.Sqlite;
using Ideas.Scada.Common.DataSources;
using System.Collections.Generic;

namespace Ideas.Scada.Common.Tags
{
	public class DataBase
	{
		#region MEMBERS
			
		private IDbConnection dbcon = null;
		private IDbCommand dbcmd = null;
		private string connectionString = "URI=file::memory:,version=3";
		//private string connectionString = "URI=file:Ideas.TagsDatabase.db,version=3";
		
		#endregion
		
		/// <summary>
		/// Constructs the class
		/// </summary>	
		public DataBase ()
		{
			Start();			
		}
				
		~DataBase()
		{
			this.Stop();	
		}
		
		private void Start()
		{
			dbcon = (IDbConnection) new SqliteConnection(connectionString);
			dbcon.Open();
			dbcmd = dbcon.CreateCommand();
		}
		
		private void Stop()
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
		
		void CreateDatasourceStructure (string dataSourceName)
		{
			string sql = "";
			sql += "CREATE TABLE tb" + dataSourceName + " ( ";
            sql += "TagName varchar(200), ";
			sql += "TagAddress varchar(200), ";
            sql += "DataType varchar(200), ";
			sql += "DateTimeUpdate DATETIME, ";
			sql += "ClientAccess varchar(3), ";
			sql += "EngUnits varchar(32), ";
			sql += "Description varchar(255), ";
			sql += "Value varchar(255) ";
			sql += "); ";
			
			dbcmd.Parameters.Clear();
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}

		public void AddDataSource (DataSource datasource)
		{
			try
			{
				CreateDatasourceStructure(datasource.Name);
				InsertTagsListToDatasource(datasource.Tags);
			}
			catch(Exception e)
			{
				string errorMessage = "Could not create database structure for datasource ";
				errorMessage += "'" + datasource.Name + "'. ";
				errorMessage += e.Message;
				
				throw new Exception(errorMessage);
			}
		}
		
		private void InsertTagsListToDatasource(TagGroup tags)
		{
			foreach (Tag t in tags)
			{
				InsertTagToDatasource(t);
			}
		}
		
		/// <summary>
		/// Insert tag information in memory database
		/// </summary>
		/// <param name="tag">
		/// A <see cref="Tag"/>
		/// </param>
		private void InsertTagToDatasource(Tag tag)
		{
			string sql = "";
			sql += "INSERT INTO tb" + tag.datasource + " ( ";
            sql += "TagName, ";
			sql += "TagAddress, ";
            sql += "DataType, ";
			sql += "DateTimeUpdate, ";
			sql += "ClientAccess, ";
			sql += "EngUnits, ";
			sql += "Description, ";
			sql += "Value ";
			sql += " ) values ( ";
			sql += "@TagName, ";
			sql += "@TagAddress, ";
            sql += "@DataType, ";
			sql += "NULL, ";
			sql += "@ClientAccess, ";
			sql += "@EngUnits, ";
			sql += "@Description, ";
			sql += "NULL ";
			sql += " );";
					
			dbcmd.Parameters.Clear();
			
			IDbDataParameter parTagName = dbcmd.CreateParameter();
			parTagName.ParameterName = "TagName";
			parTagName.DbType = DbType.String;
			parTagName.Value = tag.name;
			dbcmd.Parameters.Add(parTagName);
			
			IDbDataParameter parTagAddress = dbcmd.CreateParameter();
			parTagAddress.ParameterName = "TagAddress";
			parTagAddress.DbType = DbType.String;
			parTagAddress.Value = tag.address;
			dbcmd.Parameters.Add(parTagAddress);
			
			IDbDataParameter parDataType = dbcmd.CreateParameter();
			parDataType.ParameterName = "DataType";
			parDataType.DbType = DbType.String;
			parDataType.Value = tag.datatype;
			dbcmd.Parameters.Add(parDataType);
			
			IDbDataParameter parClientAccess = dbcmd.CreateParameter();
			parClientAccess.ParameterName = "ClientAccess";
			parClientAccess.DbType = DbType.String;
			parClientAccess.Value = tag.clientaccess;
			dbcmd.Parameters.Add(parClientAccess);
			
			IDbDataParameter parEngUnits = dbcmd.CreateParameter();
			parEngUnits.ParameterName = "EngUnits";
			parEngUnits.DbType = DbType.String;
			parEngUnits.Value = tag.engunits;
			dbcmd.Parameters.Add(parEngUnits);
			
			IDbDataParameter parDescription = dbcmd.CreateParameter();
			parDescription.ParameterName = "Description";
			parDescription.DbType = DbType.String;
			parDescription.Value = tag.description;
			dbcmd.Parameters.Add(parDescription);
			
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}
		
		public void WriteTagsListValue(TagGroup tags)
		{
			foreach (Tag t in tags)
			{
				WriteTagValue(t);
			}
		}
		
		public void WriteTagValue(Tag tag)
		{
			string sql = "" +
				"UPDATE tb" + tag.datasource + " SET " +
				"Value = @Value " +
				"WHERE " +
				"TagName = @TagName ";
			
			dbcmd.Parameters.Clear();
			
			IDbDataParameter parTagName = dbcmd.CreateParameter();
			parTagName.ParameterName = "TagName";
			parTagName.DbType = DbType.String;
			parTagName.Value = tag.name;
			dbcmd.Parameters.Add(parTagName);
			
			IDbDataParameter parValue = dbcmd.CreateParameter();
			parValue.ParameterName = "Value";
			parValue.DbType = DbType.String;
			parValue.Value = tag.value;
			dbcmd.Parameters.Add(parValue);
			
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}
		
		public void ReadTagsListValue(TagGroup tags)
		{
			for(int i = 0; i < tags.Count; i++)
			{
				Tag t = tags[i];
				ReadTagValue(ref t);
				
			}
		}
		
		public void ReadTagValue(ref Tag tag)
		{
			string sql = "" +
				"INSERT INTO @Value " +
				"SELECT Value " +
			 	"FROM tb" + tag.datasource + " " +
			 	"WHERE " +
			 	"TagName = @TagName ";
			
			dbcmd.Parameters.Clear();
			
			IDbDataParameter parTagName = dbcmd.CreateParameter();
			parTagName.ParameterName = "TagName";
			parTagName.DbType = DbType.String;
			parTagName.Value = tag.name;
			dbcmd.Parameters.Add(parTagName);
			
			IDbDataParameter parValue = dbcmd.CreateParameter();
			parValue.ParameterName = "Value";
			parValue.Direction = ParameterDirection.Output;
			parValue.DbType = DbType.String;
			dbcmd.Parameters.Add(parValue);
			
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
			
			tag.value = parValue.Value.ToString();
		}
		
		#region PROPERTIES
		
				
		#endregion
		
	}
}

