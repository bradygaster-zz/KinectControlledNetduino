Kinect Controlled Netduino
=============

This code is an example of how the Kinect's gesture recognition features could be used to control a servo via a Netduino

Notes
-------

David Catuhe has dome some hard work to add gesturing support on top of the Microsoft Kinect SDK. The links below describe how the gesturing library works and from where it can be downloaded. 

* [David Catuhe's excellent summary on how to use it](http://blogs.msdn.com/b/eternalcoding/archive/2011/07/04/gestures-and-tools-for-kinect.aspx) 
* [The Kinect Toolkit CodePlex page](http://kinecttoolkit.codeplex.com/)

This YouTube video demonstrates what the code in this repository will produce when fully used. 
* [Demonstration YouTube video](http://www.youtube.com/watch?v=QWiRGT58BoQ)

Setup/Install Notes
-------------------
* Get the Netduino Plus connected to your machine via USB
* Plug the Plus into the network via an Ethernet cable 
* Configure the Plus using MFDeploy to get the networking connection working
* Hook a servo up to the Netduino Plus on one of the digital pins that supports PWM - 5,6,9, or 10 according to [The Netduino Spec Page](http://www.netduino.com/netduino/specs.htm)
* Deploy the code to the Netduino Plus, or debug on it directly. Note, the default port is 8080, so the URL will be http://[PLUS.IP.Address.Here]:8080/servo/180 where 180 is the angle, between 0 and 180, to which you wish to have the servo turn.
* Modify the Kinect GUI code so that it points to your Netduino Plus URL. Specifically, there is a line of code in the WebServoClient.cs file that reads string urlBase = "http://YOUR-NETDUINO-IP-HERE:8080/servo/{0}"; that you'll need to change.
* Hook up your Kinect
* Try to browse to your Netduino Plus URL until you see your servo respond.
* Run the GUI code. You might want to tinker with the example code provided on the Kinect Toolkit CodePlex page to get a handle on the sensitivity, or disable the code in the GUI that calls the Netduino URL until you're comfortable gesturing and seeing the Kinect pick up your behavior on the screen.
* If everything is communicating properly, you should see your Netduino Servo respond accordingly. 