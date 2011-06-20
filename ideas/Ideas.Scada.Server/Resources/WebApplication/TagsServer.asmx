<%@ WebService Language="C#" Class="Ideas.ScadaApplication.TagsServer" %>
 
using System;
using System.Web.Services;
using System.Net.Sockets;

namespace Ideas.ScadaApplication
{
	[WebService (Namespace = "http://tempuri.org/TagsServer")]
	public class TagsServer : WebService
	{
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
       
        [WebMethod]
        public string ReadAll ()
        {
            return "";
        }
  
        [WebMethod]
        public string Read (string tagname)
        {
            if(clientSocket.Connected)
            {
                return SocketRead(tagname);
            }
            else
            {
                clientSocket.Connect("127.0.0.1", 2200);
                return Read(tagname);
            }
        }
 
        [WebMethod]
        public void Write (string tagname, string value)
        {
            // do nothing
        }  
    
    	private string SocketRead(string tagname)
    	{
    		NetworkStream sendStream = new NetworkStream(clientSocket);
                
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("READ|" + tagname + "<EOF>");
            //serverStream.Write(outStream, 0, outStream.Length);
            clientSocket.Send(outStream);
            sendStream.Flush();

            byte[] inStream = new byte[1024];
            clientSocket.Receive(inStream);
            //serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            
            return tagname + "=" + returndata.TrimEnd(new char[] {Convert.ToChar(default(byte))});
    	}   
	}
}
