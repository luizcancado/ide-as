<%@ WebService Language="C#" Class="Ideas.ScadaApplication.Controles" %>
/*
	Ideas.Scada.Server - 2011
	
	Screen: Controles
	File: Controles.svg.asmx (Server-side script)
	Application: SampleApplication
	Project: SampleProject
	Author: Luiz Cançado
	Description: A sample server-side script to demonstrate IDEAS application structure.
*/

 
using System;
using System.Web.Services;
 
namespace Ideas.ScadaApplication
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.Web.Script.Services.ScriptService]
	public class Controles : WebService
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
