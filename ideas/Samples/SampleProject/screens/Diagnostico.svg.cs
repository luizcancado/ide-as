/*
	Ideas.Scada.Server - 2011
	
	Screen: tela
	File: tela.svg.cs (Server-side script)
	Application: SampleApplication
	Project: SampleProject
	Author: Luiz Cançado
	Description: A sample server-side script to demonstrate IDEAS application structure.
*/

<%@ WebService Language="C#" Class="TagsServer.TagsServer" %>
 
using System;
using System.Web.Services;
 
namespace ScadaApplication
{
	[WebService (Namespace = "http://tempuri.org")]
	public class tela : WebService
	{
		[WebMethod]
		public int ServerSideMethod ()
		{
            Random random = new Random();
            
            return random.Next(0, 100);
		}
	}
}
