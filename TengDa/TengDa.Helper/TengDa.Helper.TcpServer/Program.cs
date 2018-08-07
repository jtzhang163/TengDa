using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace jtzhang163.Helper.TcpServer
{
  class Program
  {

    static int flag = 0;

    static void Main(string[] args)
    {
      TcpListener server = null;
      try
      {
        // Set the TcpListener on port 13000.
        Int32 port = Int32.Parse(ConfigurationManager.AppSettings["port"]);
        IPAddress localAddr = IPAddress.Parse(ConfigurationManager.AppSettings["host"]);

        // TcpListener server = new TcpListener(port);
        server = new TcpListener(localAddr, port);

        // Start listening for client requests.
        server.Start();

        // Buffer for reading data
        Byte[] bytes = new Byte[256];
        String data = null;

        // Enter the listening loop.
        while (true)
        {
          Console.Write("Waiting for a connection... ");

          // Perform a blocking call to accept requests.
          // You could also user server.AcceptSocket() here.
          TcpClient client = server.AcceptTcpClient();
          Console.WriteLine("Connected!");

          data = null;

          // Get a stream object for reading and writing
          NetworkStream stream = client.GetStream();

          int i;

          // Loop to receive all the data sent by the client.
          while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
          {
            // Translate data bytes to a ASCII string.
            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine("Received: {0}", data);

            // Process the data sent by the client.
            data = data.ToUpper();

            Thread.Sleep(100);

            if (data.IndexOf("RDD") > -1)
            {

              string IsReadyStr = flag > 6 ? ConfigurationManager.AppSettings["IsReadyStr"] : ConfigurationManager.AppSettings["IsNotReadyStr"];

              byte[] msg = System.Text.Encoding.ASCII.GetBytes(IsReadyStr + "\r");
              stream.Write(msg, 0, msg.Length);
              Console.WriteLine("Sent: {0}", IsReadyStr);

            }
            else if (data.IndexOf("WDD") > -1)
            {
              string WriteReturnStr = ConfigurationManager.AppSettings["WriteReturnStr"];
              byte[] msg = System.Text.Encoding.ASCII.GetBytes(WriteReturnStr + "\r");
              stream.Write(msg, 0, msg.Length);
              Console.WriteLine("Sent: {0}", WriteReturnStr);
            }
            else if (data.IndexOf("RCP") > -1 || data.IndexOf("RCS") > -1)
            {

              string IsReadyStr = flag > 6 ? ConfigurationManager.AppSettings["IsReadyStr_R"] : ConfigurationManager.AppSettings["IsNotReadyStr_R"];

              byte[] msg = System.Text.Encoding.ASCII.GetBytes(IsReadyStr + "\r");
              stream.Write(msg, 0, msg.Length);
              Console.WriteLine("Sent: {0}", IsReadyStr);
            }
            else if (data.IndexOf("WCP") > -1)
            {
              string WriteReturnStr = ConfigurationManager.AppSettings["WriteReturnStr_R"];
              byte[] msg = System.Text.Encoding.ASCII.GetBytes(WriteReturnStr + "\r");
              stream.Write(msg, 0, msg.Length);
              Console.WriteLine("Sent: {0}", WriteReturnStr);
            }
            else
            {
              string IsReadyStr = flag > 6 ? ConfigurationManager.AppSettings["IsReadyStr_R"] : ConfigurationManager.AppSettings["IsNotReadyStr_R"];

              byte[] msg = System.Text.Encoding.ASCII.GetBytes(IsReadyStr + "\r");
              stream.Write(msg, 0, msg.Length);
              Console.WriteLine("Sent: {0}", IsReadyStr);
            }


            flag = (++flag) % 10;
            // Send back a response.

          }


          // Shutdown and end connection
          client.Close();

        }
      }
      catch (Exception e)
      {
        Console.WriteLine("SocketException: {0}", e);
      }
      finally
      {
        // Stop listening for new clients.
        server.Stop();
      }


      Console.WriteLine("\nHit enter to continue...");
      Console.Read();

    }
  }
}
