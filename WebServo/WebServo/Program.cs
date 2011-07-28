using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using MFToolkit.Net.Web;

namespace WebServo
{
    public class Program
    {
        public static void Main()
        {
			var ip = Microsoft.SPOT.Net.NetworkInformation
						.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress;

            Debug.Print(ip);
            
            // start the web server
            HttpServer server = new HttpServer(8080, /*new LEDActivatorHandler()*/ new ServoHandler());
            server.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
