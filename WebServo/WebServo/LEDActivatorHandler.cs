using System;
using Microsoft.SPOT;
using MFToolkit.Net.Web;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace WebServo
{
    public class LEDActivatorHandler : IHttpHandler
    {
        public OutputPort Led { get; set; }

        public LEDActivatorHandler()
        {
            this.Led = new OutputPort(Pins.ONBOARD_LED, false);
        }

        public void ProcessRequest(HttpContext context)
        {
            string output = string.Empty;
            output += "<b>Request Path:</b><br/>";
            output += context.Request.Path;
            output += "<br/><b>Request Params:</b><br/>";

            if (context.Request.Params != null)
            {
                for (int x = 0; x < context.Request.Params.Keys.Count; x++)
                {
                    var s = context.Request.Params.Keys[x] + ": " + context.Request.Params[x];
                    output += s + "<br/>";
                }
            }

            output = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Netduino Web Server</title></head><body>" + output + "</body></html>";

            context.Response.ContentType = "text/html";
            context.Response.HttpStatus = HttpStatusCode.OK;
            context.Response.WriteLine(output);

            var a = context.Request.Path.Split('/');
            if (a.Length == 3)
            {
                if (a[1] == "led") this.Led.Write(a[2] == "true");
            }
        }
    }
}
