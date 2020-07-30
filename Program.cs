/*
 * Stærk inspireret af Munir Usman http://munir.wordpress.com
 *                     Contact: munirus@gmail.com 
 *                     https://github.com/munirusman/Simple-TCP-Port-Scanner.git
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
            PortList portlist = new PortList(startPort, endPort);
            TcpClient tcp = new TcpClient();

            int numThreads = 200;
            for (int i = 0; i < numThreads; i++)
            {
                PortScanner scanner = new PortScanner(ipAdrrToScan, portlist);
                Thread th = new Thread(new ThreadStart(scanner.run));
                th.Start();
            }

        }
    }

    class PortScanner
    {
        private int port;
        private PortList portlist;
        IPAddress ipAdrrToScan;

        public PortScanner(IPAddress ipAdrrToScan, PortList portList)
        {
            this.portlist = portList;
            this.ipAdrrToScan = ipAdrrToScan;
        }

        public void run()
        {
            int port;
            TcpClient tcp = new TcpClient();
            while ((port = portlist.getNext()) != -1)
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

    public class PortList
    {
        private int start;
        private int stop;
        private int ptr;

        public PortList(int start, int stop)
        {
            this.start = start;
            this.stop = stop;
            this.ptr = start;
        }

        public PortList() : this(1, 65535)
        {
        }

        public bool hasMore()
        {
            return (stop - ptr) >= 0;
        }

        public int getNext()
        {
            if (hasMore())
                return ptr++;
            return -1;
        }
    }
}
