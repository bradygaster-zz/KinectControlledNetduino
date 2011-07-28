using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WebServo.KinectClientGui
{
	public class WebServoClient
	{
		string urlBase = "http://YOUR-NETDUINO-IP-HERE:8080/servo/{0}";

		public event EventHandler AngleChanged;

		int delta = 90;

		protected void OnAngleChanged()
		{
			if (this.AngleChanged != null)
				this.AngleChanged(this, EventArgs.Empty);
		}

		private void SetWebServoAngle()
		{
			var url = string.Format(urlBase, this.Angle);
			var newAngle = new WebClient().DownloadString(url);

			try
			{
				this.Angle = Convert.ToInt32(newAngle);
				this.OnAngleChanged();
			}
			catch
			{
				// angle didn't come back as a number for some odd reason
			}
		}

		public void Initialize()
		{
			this.Angle = 0;
			SetWebServoAngle();
		}

		public void TurnLeft()
		{
			if (this.Angle == 0 || this.Angle - delta <= 0)
				this.Angle = 0;
			else
				this.Angle -= delta;

			SetWebServoAngle();
		}

		public void TurnRight()
		{
			if (this.Angle == 180 || this.Angle + delta >= 180)
				this.Angle = 180;
			else
				this.Angle += delta;

			SetWebServoAngle();
		}

		public int Angle { get; private set; }
	}
}
