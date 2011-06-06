using System;
using Ideas.Scada.Common.Tags;
using System.Collections.Generic;

namespace Ideas.Scada.Common
{
	public class TagGroup : List<Tag>
	{
		private string groupName = "";
		
		public TagGroup () : base()
		{
		}
		
		public TagGroup (string name) : this()
		{
			this.groupName = name;
		}
		
		public Tag GetByTagName(string tagName)
		{
			for(int i =0; i< base.Count ;i++)
			{
				if(base[i].name == tagName)
					return base[i];
			}
			
			return null;
		}
		
		public Tag GetByTagAddress(string tagAddr)
		{
			for(int i =0; i< base.Count ;i++)
			{
				if(base[i].address == tagAddr)
					return base[i];
			}
			
			return null;
		}
		
		public Tag this[string tagName]
		{
			get
			{
				return GetByTagName(tagName);
			}
		}
		
		
		public string GroupName {
			get {
				return this.groupName;
			}
			set {
				groupName = value;
			}
		}
	}
}

