using System;
using System.Collections;

namespace Ideas.Scada.Common.Tags
{
	public class Tag
	{
		public string datasource;
		public string name;
		public string address;
 	    public string datatype;
        public string lastupdate;
        public string clientaccess;
        public string engunits;
        public string description;
		public string value;
		
		public override string ToString ()
		{
			return "Tag " +
				"{ DataSource: " + datasource +
				", Address: " + address +
				", Datatype: " + datatype +
				", LastUpdate: " + lastupdate +
				", ClientAccess: " + clientaccess +
				", Engunits: " + engunits +
				", Description: " + description +
				", Value: " + value +	
				" }";
		}
	}
}