<%@ WebService Language="C#" Class="Ideas.ScadaApplication.MisturadorTintas" %>
 
using System;
using System.Web.Services;
 
namespace Ideas.ScadaApplication
{
	[WebService (Namespace = "http://tempuri.org/", 
	             Description = "Server-side code for the screen \"MisturadorTintas\"")]
	public class MisturadorTintas : WebService
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
