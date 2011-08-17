using System;
using System.Collections.Generic;

namespace Ideas.Scada.Common.DataSources
{
	public class DataSourceCollection : List<DataSource>
	{		
		public DataSourceCollection () : base()
		{
		}
				
		private DataSource GetByDataSourceName(string name)
		{
			for(int i = 0; i< base.Count; i++)
			{
				if(base[i].Name == name)
				{
					return base[i];
				}
			}
			
			return null;
		}
		
		public DataSource this[string name]
		{
			get
			{
				return GetByDataSourceName(name);
			}
		}
	}
}

