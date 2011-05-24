using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Ideas.Scada.Common
{
	public class Screen
	{
		#region M E M B E R S
		
		private string name;
		private string filePath;
		private Project project;
		private ScreenType type;
		private ScreenServerScriptLanguage serverScriptLanguage;
		private ScreenClientScriptLanguage clientScriptLanguage;
		
		#endregion 
		
		/// <summary>
		/// Constructs the class
		/// </summary>	
		public Screen ()
		{
			
		}
		
		public Screen (XmlNode node, Project parentProject)
		{
			string nodeName = node.Attributes["name"].Value;
			string nodePath = node.Attributes["filename"].Value;
			string nodeStringType = node.Attributes["type"].Value;
			string stringServerScriptLanguage = node.Attributes["serverscriptlanguage"].Value;
			string stringClientScriptLanguage = node.Attributes["clientscriptlanguage"].Value;
						
			ScreenType nodeType = convertScreenTypeFromString(nodeStringType);
			ScreenServerScriptLanguage nodeServerScriptLanguage = ConvertStringToScreenServerScriptLanguage(stringServerScriptLanguage);
			ScreenClientScriptLanguage nodeClientScriptLanguage = ConvertStringToScreenClientScriptLanguage(stringClientScriptLanguage);
			
			this.Name = nodeName;
			this.FilePath = parentProject.FilePath + "screens" + Path.DirectorySeparatorChar;
			this.FilePath += nodePath + Path.DirectorySeparatorChar;
			this.Project = parentProject;
			this.Type = nodeType;
			this.ServerScriptLanguage = nodeServerScriptLanguage;
			this.ClientScriptLanguage = nodeClientScriptLanguage;
		}
			
	
		#region S T A T I C   M E T H O D S 

		public static ScreenType convertScreenTypeFromString(string strType)
		{
			switch(strType.ToLower())
			{
				case "svg": 
					return ScreenType.SVG;
				default:
					throw new Exception("Unkown screen type: " + strType);
			}
		}
		
		public static ScreenClientScriptLanguage ConvertStringToScreenClientScriptLanguage(string strClientScriptLanguage)
		{
			switch(strClientScriptLanguage.ToLower())
			{
				case "javascript": 
				case "js": 
					return ScreenClientScriptLanguage.Javascript;
				default:
					throw new Exception("Unkown client script language: " + strClientScriptLanguage);
			}
		}
		
		public static ScreenServerScriptLanguage ConvertStringToScreenServerScriptLanguage(string strServerScriptLanguage)
		{
			switch(strServerScriptLanguage.ToLower())
			{
				case "csharp": 
				case "c#": 
				case "cs": 
					return ScreenServerScriptLanguage.CSharp;
				case "visualbasic": 
				case "vb": 
					return ScreenServerScriptLanguage.VisualBasic;
				default:
					throw new Exception("Unkown server script language: " + strServerScriptLanguage);
			}
		}
		
		#endregion 
		
		
		#region P R O P E R T I E S
		
		public string Name 
		{
			get 
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public Project Project {
			get {
				return this.project;
			}
			set {
				project = value;
			}
		}
		public string FilePath 
		{
			get 
			{
				return this.filePath;
			}
			set
			{
				this.filePath = value;
			}
		}
		
		public ScreenClientScriptLanguage ClientScriptLanguage 
		{
			get 
			{
				return this.clientScriptLanguage;
			}
			set 
			{
				clientScriptLanguage = value;
			}
		}

		public ScreenServerScriptLanguage ServerScriptLanguage 
		{
			get 
			{
				return this.serverScriptLanguage;
			}
			set 
			{
				serverScriptLanguage = value;
			}
		}

		public ScreenType Type 
		{
			get 
			{
				return this.type;
			}
			set 
			{
				type = value;
			}
		}
		
		#endregion
	}
	
	public enum ScreenType 
	{
		SVG
	}
	
	public enum ScreenServerScriptLanguage 
	{
		CSharp,
		VisualBasic
	}
	
	public enum ScreenClientScriptLanguage 
	{
		Javascript
	}
}

