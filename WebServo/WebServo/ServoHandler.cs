using System;
using Microsoft.SPOT;
using MFToolkit.Net.Web;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.IO;

namespace WebServo
{
	public class ServoHandler : IHttpHandler
	{
		public Servo Servo { get; set; }

		public ServoHandler()
		{
			this.Servo = new Servo(Pins.GPIO_PIN_D5);
		}

		#region IHttpHandler Members

		public void ProcessRequest(HttpContext context)
		{
			var a = context.Request.Path.Split('/');
			if (a.Length == 3)
			{
				if (a[1] == "servo")
				{
					try
					{
						var val = Convert.ToDouble(a[2]);
						this.Servo.Degree = val;
					}
					catch { /* a human passed an NaN, probably */ }
				}
			}

			var output = this.Servo.Degree.ToString();
			context.Response.ContentType = "text/plain";
			context.Response.Write(output);
		}

		#endregion
	}
}
