using System;
using System.IO;
using System.Diagnostics;
	
namespace Ideas.Scada.Common.DataSources
{
	public class OpenOPC
	{
		Process prcOpenOPC;
		StreamReader reader;
			
		public OpenOPC ()
		{
			const String strPythonScript = 
				"/home/luiz/Desktop/OpenOPC/src/opc.py  -H 192.168.0.169 -s Kepware.KEPServerEX.V5 -r Channel1.S7_1200__1214C.T_MISTURADOR";		
			
			ProcessStartInfo infoOpenOPC = new ProcessStartInfo();	
			infoOpenOPC.FileName = "python";
			infoOpenOPC.Arguments = strPythonScript;
			infoOpenOPC.RedirectStandardOutput = true;
			infoOpenOPC.UseShellExecute = false;
			
			prcOpenOPC = Process.Start(infoOpenOPC);
			
			reader =  prcOpenOPC.StandardOutput;
		}
		
		public string Read()
		{
            
			string retString = "";
						
			try
            {
				retString = reader.ReadToEnd();
            }
            catch ( Exception e )
            {
                //System.Diagnostics.EventLog.WriteEntry("STBUpdate", ex.Message, EventLogEntryType.Error, 1111);
            }

			return retString;
		}		
	}
}

