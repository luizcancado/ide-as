<%@ WebService Language="C#" Class="Ideas.ScadaApplication.TagsServer" %>
 
using System;
using System.Web.Services;
 
namespace Ideas.ScadaApplication
{
	[WebService (Namespace = "http://tempuri.org/TagsServer")]
	public class TagsServer : WebService
	{
		[WebMethod]
		public int GetAllTagsValues ()
		{
            Random random = new Random();
            
            return random.Next(0, 100);
		}
  
  
        [WebMethod]
        public int GetTagValue (string tagname)
        {
            Random random = new Random();
            
            return random.Next(0, 100);
        }      
 
		[WebMethod]
		public void SetTagValue (string tagname, string value)
		{
			// do nothing
		}
	}
}
