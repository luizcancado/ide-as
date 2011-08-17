using System;
using System.Collections.Generic;

namespace Ideas.Scada.Common
{
	public class ScreenCollection : List<Screen>
	{
		
		public ScreenCollection () : base()
		{
		}
		
		private Screen GetByScreenName(string screenName)
		{
			for(int i =0; i< base.Count ;i++)
			{
				if(base[i].Name == screenName)
				{
					return base[i];
				}
			}
			
			return null;
		}
		
		public Screen this[string screenName]
		{
			get
			{
				return GetByScreenName(screenName);
			}
		}
	}
}

