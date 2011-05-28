<%@ WebService Language="C#" Class="Ideas.ScadaApplication.Diagnostico" %>
 
using System;
using System.Web.Services;
 
namespace Ideas.ScadaApplication
{
	[WebService (Namespace = "http://tempuri.org/", 
	             Description = "Server-side code for the screen \"Diagnostico\"")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class Diagnostico : WebService
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
