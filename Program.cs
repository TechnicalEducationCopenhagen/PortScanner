using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PortScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            if (args.Length > 0)
            {
                ip = args[0];
            }
            IPAddress ipAdrrToScan = IPAddress.Parse(ip);

            int startPort = 1;
            int endPort = 65535;
            TcpClient tcp = new TcpClient();

            for (int port = startPort; port < endPort; port++)
            {
                tcp = new TcpClient();
                try
                {
                    tcp.Connect(ipAdrrToScan.ToString(), port);
                    Console.WriteLine();
                    Console.WriteLine($"{DateTime.Now} {tcp.Client.LocalEndPoint} {tcp.Client.RemoteEndPoint.ToString()}");
                }
                catch
                {
                    //Console.WriteLine($"{ipAdrrToScan.ToString()}:{port} {tcp.Connected}");
                    Console.Write(".");
                }
                finally
                {
                    tcp.Close();
                }
            }
        }
    }
}
