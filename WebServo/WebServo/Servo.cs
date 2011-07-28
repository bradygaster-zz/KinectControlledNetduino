using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace WebServo
{
    public class Servo : IDisposable
    {
        private PWM servo;

        private int[] range = new int[2];

        public bool inverted = false;

        public Servo(Cpu.Pin pin)
        {
            // Init the PWM pin
			servo = new PWM(Pins.GPIO_PIN_D5);

            servo.SetDutyCycle(0);

            // Typical settings
            range[0] = 1000;
            range[1] = 2000;
        }

        public void Dispose()
        {
            disengage();
            servo.Dispose();
        }

        public void setRange(int fullLeft, int fullRight)
        {
            range[1] = fullLeft;
            range[0] = fullRight;
        }

        public void disengage()
        {
            // See what the Netduino team say about this... 
            servo.SetDutyCycle(0);
        }

		double _val; 
        public double Degree
        {
			get
			{
				return _val;
			}
            set
            {
				_val = value;

                /// Range checks
				if (_val > 180)
					_val = 180;

				if (_val < 0)
					_val = 0;

                // Are we inverted?
                if (inverted)
					_val = 180 - _val;

                // Set the pulse
				servo.SetPulse(20000, (uint)map((long)_val, 0, 180, range[0], range[1]));
            }
        }

        private long map(long x, long in_min, long in_max, long out_min, long out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}
