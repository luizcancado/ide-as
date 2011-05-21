using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Ideas.Scada.Common
{
	public class Screen
	{
		private string name;
		private string filePath;
		private IdeasScadaScreenType type;
		private IdeasScadaScreenServerScriptLanguage serverScriptLanguage;
		private IdeasScadaScreenClientScriptLanguage clientScriptLanguage;
			
		/// <summary>
		/// Constructs the class
		/// </summary>	
		public Screen ()
		{
			
		}
		
		public Screen (XmlNode xmlScreenNode)
		{
			string nodeName = xmlScreenNode.Attributes["name"].Value;
			string nodePath = xmlScreenNode.Attributes["path"].Value;
			string nodeStringType = xmlScreenNode.Attributes["type"].Value;
			string nodeStringServerScriptLanguage = xmlScreenNode.Attributes["serverscriptlanguage"].Value;
			string nodeStringClientScriptLanguage = xmlScreenNode.Attributes["clientscriptlanguage"].Value;
			
			IdeasScadaScreenType nodeType = convertScreenTypeFromString(nodeStringType);
			IdeasScadaScreenServerScriptLanguage nodeServerScriptLanguage = convertScreenServerScriptLanguageFromString(nodeStringServerScriptLanguage);
			IdeasScadaScreenClientScriptLanguage nodeClientScriptLanguage = convertScreenClientScriptLanguageFromString(nodeStringClientScriptLanguage);
			
			this.Name = nodeName;
			this.FilePath = "screens" + Path.DirectorySeparatorChar;
			this.FilePath += nodePath + Path.DirectorySeparatorChar;
			
			this.Type = nodeType;
			this.ServerScriptLanguage = nodeServerScriptLanguage;
			this.ClientScriptLanguage = nodeClientScriptLanguage;
		}
			
	
		#region S T A T I C   M E T H O D S 

		public static IdeasScadaScreenType convertScreenTypeFromString(string strType)
		{
			switch(strType.ToLower())
			{
				case "svg": 
					return IdeasScadaScreenType.SVG;
				default:
					throw new Exception("Unkown screen type: " + strType);
			}
		}
		
		public static IdeasScadaScreenClientScriptLanguage convertScreenClientScriptLanguageFromString(string strClientScriptLanguage)
		{
			switch(strClientScriptLanguage.ToLower())
			{
				case "javascript": 
				case "js": 
					return IdeasScadaScreenClientScriptLanguage.Javascript;
				default:
					throw new Exception("Unkown client script language: " + strClientScriptLanguage);
			}
		}
		
		public static IdeasScadaScreenServerScriptLanguage convertScreenServerScriptLanguageFromString(string strServerScriptLanguage)
		{
			switch(strServerScriptLanguage.ToLower())
			{
				case "csharp": 
				case "c#": 
				case "cs": 
					return IdeasScadaScreenServerScriptLanguage.CSharp;
				case "visualbasic": 
				case "vb": 
					return IdeasScadaScreenServerScriptLanguage.VisualBasic;
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
		
		public IdeasScadaScreenClientScriptLanguage ClientScriptLanguage 
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

		public IdeasScadaScreenServerScriptLanguage ServerScriptLanguage 
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

		public IdeasScadaScreenType Type 
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
	
	public enum IdeasScadaScreenType 
	{
		SVG
	}
	
	public enum IdeasScadaScreenServerScriptLanguage 
	{
		CSharp,
		VisualBasic
	}
	
	public enum IdeasScadaScreenClientScriptLanguage 
	{
		Javascript
	}
}

