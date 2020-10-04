using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace BodySee.Tools
{
    class Client
    {
        #region Private Field
        private const Int32 port = 1234;
        private const String IP = "192.168.5.130";
        private TcpClient client;
        #endregion

        #region Public Methods
        /// <summary>
        /// Constructor. Try to connect to a TCP server. 
        /// </summary>
        public Client()
        {
            try
            {
                client = new TcpClient(IP, port);
                Console.WriteLine("Connected to a server!");
                Thread thread = new Thread(Receive);
                thread.Start();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        /// <summary>
        /// Send message to TCP server
        /// Always add a '\n' character on tail to avoid blocking.
        /// </summary>
        /// <param name="msg"> message to send </param>
        public void Send(String msg)
        {
            if (!client.Connected)
                return;
            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg + "\n");
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Receive message from TCP server. 
        /// </summary>
        public void Receive()
        {
            while(true)
            {
                if (!client.Connected)
                    continue;

                try
                {
                    var data = new Byte[256];
                    String response = String.Empty;
                    NetworkStream stream = client.GetStream();
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Debug.WriteLine(response);
                    TaskManager.getInstance().Execute(response);
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            

        }

        /// <summary>
        /// Close the network connection.
        /// </summary>
        public void Close()
        {
            client.Close();
        }
        #endregion
    }
}
