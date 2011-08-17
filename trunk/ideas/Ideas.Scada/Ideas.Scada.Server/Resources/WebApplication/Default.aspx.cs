//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Web;
using System.Web.UI;

namespace Ideas.Scada.Server.WebApplication
{
	public partial class Default : System.Web.UI.Page
	{
		public Default()
		{
			
		}
		
	    protected override void OnLoad(EventArgs e)
	    {
			if(Request.QueryString["s"] != null)
			{
				DisplayScreen(Request.QueryString["s"].Trim());
			}
			else
			{
				DisplayScreen(System.Configuration.ConfigurationManager.AppSettings["InitialScreen"]);
			}
			
			base.OnLoad(e);
	    }
		
		private void DisplayScreen(string screenName)
		{
			string screens = System.Configuration.ConfigurationManager.AppSettings["Screens"];
			string[] screenList = screens.Split(';');
			
			foreach (string scr in screenList)
			{
				string[] s = scr.Split(',');
				string scrName = s[0];
				string scrPath = s[1];
				
				if(scrName.Equals(screenName, StringComparison.CurrentCultureIgnoreCase))
				{
					WriteSVGScreen(scrPath);	
					break;
				}				
			}
		}
	
	    private void WriteSVGScreen(string screenFile)
	    {
			Response.ContentType = "image/svg+xml";
			Response.WriteFile(screenFile, true);
	    }
	}
}
