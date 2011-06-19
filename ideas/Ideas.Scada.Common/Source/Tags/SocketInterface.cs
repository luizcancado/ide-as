//  
//  SocketInterface.cs
//  
//  Author:
//       Luiz Cançado <luizcancado@gmail.com>
// 
//  Copyright (c) 2011 Luiz Cançado
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Ideas.Scada.Common;
using log4net;

namespace Ideas.Scada.Common.Tags
{
    public class SocketInterface
    {
        private static Project parentProject;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static readonly ILog log = LogManager.GetLogger(typeof(SocketInterface));
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SocketInterface ()
        {
            
        }
        
        /// <summary>
        /// Start listening with the socket configuration
        /// </summary>
        /// /// <param name="parent">
        /// A <see cref="Project"/> that contains this new instance
        /// </param>
        public static void Start(Project parent)
        {
            parentProject = parent;
            
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            
            try
            {

                IPAddress localAddress = IPAddress.Any;

                // Define the kind of socket we want: Internet, Stream, TCP
                Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Define the address we want to claim: the first IP address found earlier, at port 2200
                IPEndPoint ipEndpoint = new IPEndPoint(localAddress, 2200);

                // Bind the socket to the end point
                listenSocket.Bind(ipEndpoint);

                // Start listening, only allow 1 connection to queue at the same time
                listenSocket.Listen(1);
                listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), listenSocket);
                log.Info("Server is waiting on socket " + listenSocket.LocalEndPoint);
                
            }
            catch (Exception e)
            {
                log.Error("Caught Exception: " + e.ToString());
            }
        }
        
        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();
    
            // Get the socket that handles the client request.
            Socket listener = (Socket) ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
    
            // Create the state object.
            SocketInterfaceStateObject state = new SocketInterfaceStateObject();
            state.workSocket = handler;
            
            Console.WriteLine("Received Connection from {0}", handler.RemoteEndPoint);
            
            handler.BeginReceive(
                state.buffer, 
                0, 
                SocketInterfaceStateObject.BufferSize, 
                0,
                new AsyncCallback(ReadCallback), 
                state);
        }
        
        public static void ReadCallback(IAsyncResult ar) 
        {
            String content = String.Empty;
            
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            SocketInterfaceStateObject state = (SocketInterfaceStateObject) ar.AsyncState;
            Socket handler = state.workSocket;
    
            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);
    
            if (bytesRead > 0) 
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
    
                // Check for end-of-file tag. If it is not there, read more data.
                content = state.sb.ToString();
                
                if (content.IndexOf("<EOF>") > -1) 
                {
                    // Clear the string builder from state object
                    state.sb.Length = 0;
                    
                    // All the command data has been read from the client. 
                    // Display it on the console.
                    
                    //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    //    content.Length, content );
                    
                    content = content.Replace("<EOF>","");
                    
                    if(content.StartsWith("READ|"))
                    {
                        string response = ReadFromDataBase(content);
                        //string response = ReadRandom();
                        
                        // Send tag value to the client
                        Send(handler, response);
                    }
                    else if(content.StartsWith("WRITE|"))
                    {
                        //WriteToDataSource();
                        //parentProject.Write();
                    }
                    else
                    {
                        log.Error("Unkown command received: " + content);
                    }
                } 

                // Continue to listen for commands
                handler.BeginReceive(
                    state.buffer, 
                    0, 
                    SocketInterfaceStateObject.BufferSize, 
                    0,
                    new AsyncCallback(ReadCallback), 
                    state);

            }
        }
        
        private static void Send(Socket handler, String data) 
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            
            //Console.WriteLine("Sending {0} to client.", data);
            
            // Begin sending the data to the remote device.
            handler.BeginSend(
                byteData, 
                0, 
                byteData.Length, 
                0,
                new AsyncCallback(SendCallback), 
                handler);
        }
        
        private static void SendCallback(IAsyncResult ar) 
        {
            try 
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket) ar.AsyncState;
    
                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
    
            } 
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
        }
        
        private static string ReadFromDataBase(string command)
        {
            string tagname = command.Replace("READ|", "");
            
            Tag tag = new Tag();
            
            tag.datasource = "S7_1214C";
            tag.name = tagname;
            

            return parentProject.Read(tag).ToString();
        }
        
        private static string ReadRandom()
        {
            Random random = new Random();
            return random.Next(2).ToString();
        }
    }
    
    public class SocketInterfaceStateObject {
        
        // Client  socket.
        public Socket workSocket = null;
        
        // Size of receive buffer.
        public const int BufferSize = 1024;
        
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}

