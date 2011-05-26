using System;
using System.Collections.Generic;

namespace Ideas.Scada.Common
{
	public class ProjectCollection : List<Project>
	{		
		public ProjectCollection () : base()
		{
		}
				
		private Project GetByProjectName(string projName)
		{
			for(int i =0; i< base.Count ;i++)
			{
				if(base[i].Name == projName)
				{
					return base[i];
				}
			}
			
			return null;
		}
		
		public Project this[string tagName]
		{
			get
			{
				return GetByProjectName(tagName);
			}
		}
		
		public Project this[int i]
		{
			get
			{
				return base[i];
			}
		}
	}
}

