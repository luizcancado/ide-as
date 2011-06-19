<%@ WebService Language="C#" Class="Ideas.ScadaApplication.TagsServer" %>
 
using System;
using System.Web.Services;
 
namespace Ideas.ScadaApplication
{
	[WebService (Namespace = "http://tempuri.org/TagsServer")]
	public class TagsServer : WebService
	{
		[WebMethod]
		public int ReadAll ()
		{
            Random random = new Random();
            
            return random.Next(2);
		}
  
        [WebMethod]
        public int Read (string tagname)
        {
            Random random = new Random();
            
            return random.Next(2);
        }
 
		[WebMethod]
		public void Write (string tagname, string value)
		{
			// do nothing
		}
	}
}
