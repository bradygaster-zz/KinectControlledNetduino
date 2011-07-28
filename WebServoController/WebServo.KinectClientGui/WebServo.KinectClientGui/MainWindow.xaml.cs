using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.Kinect.Nui;
using Kinect.Toolkit;
using System.Diagnostics;
using Kinect.Toolkit.Record;

namespace WebServo.KinectClientGui
{
	public partial class MainWindow : Window
	{
		Runtime kinectRuntime;
		TemplateGestureDetector circleGestureRecognizer;
		SkeletonDisplayManager skeletonDisplayManager;
		readonly SwipeGestureDetector swipeGestureRecognizer = new SwipeGestureDetector();
		readonly ColorStreamManager streamManager = new ColorStreamManager();
		readonly BarycenterHelper barycenterHelper = new BarycenterHelper();
		readonly PostureDetector postureRecognizer = new PostureDetector();
		string circleKBPath;
		BindableNUICamera nuiCamera;
		WebServoClient client;

		public MainWindow()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(OnWindowLoaded);
		}

		void OnWindowLoaded(object sender, RoutedEventArgs e)
		{
			kinectRuntime = new Runtime();
			kinectRuntime.Initialize(RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
			kinectRuntime.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
			kinectRuntime.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(OnSkeletonFrameReady);
			kinectRuntime.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(OnVideoFrameReady);
			swipeGestureRecognizer.OnGestureDetected += OnGestureDetected;
			skeletonDisplayManager = new SkeletonDisplayManager(kinectRuntime.SkeletonEngine, kinectCanvas);
			kinectRuntime.SkeletonEngine.TransformSmooth = true;
			var parameters = new TransformSmoothParameters
			{
				Smoothing = 1.0f,
				Correction = 0.1f,
				Prediction = 0.1f,
				JitterRadius = 0.05f,
				MaxDeviationRadius = 0.05f
			};
			kinectRuntime.SkeletonEngine.SmoothParameters = parameters;
			nuiCamera = new BindableNUICamera(kinectRuntime.NuiCamera);
			this.client = new WebServoClient();
			this.client.AngleChanged += new EventHandler(OnAngleChanged);
		}

		void OnAngleChanged(object sender, EventArgs e)
		{
			angle.Text = "Angle: " + this.client.Angle;
		}

		void OnGestureDetected(string gesture)
		{
			Debug.WriteLine(gesture);

			swipeLeft.Background = new SolidColorBrush(Colors.White);
			swipeRight.Background = new SolidColorBrush(Colors.White);

			if (gesture.Equals("SwipeToLeft", StringComparison.CurrentCultureIgnoreCase))
			{
				swipeLeft.Background = new SolidColorBrush(Colors.Red);
				this.client.TurnLeft();
			}

			if (gesture.Equals("SwipeToRight", StringComparison.CurrentCultureIgnoreCase))
			{
				swipeRight.Background = new SolidColorBrush(Colors.Red);
				this.client.TurnRight();
			}
		}

		void OnVideoFrameReady(object sender, ImageFrameReadyEventArgs e)
		{
			kinectDisplay.Source = streamManager.Update(e);
		}

		void OnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
		{
			ProcessFrame(e.SkeletonFrame);
		}

		void ProcessFrame(ReplaySkeletonFrame frame)
		{
			Dictionary<int, string> stabilities = new Dictionary<int, string>();
			foreach (var skeleton in frame.Skeletons)
			{
				if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
					continue;

				barycenterHelper.Add(skeleton.Position.ToVector3(), skeleton.TrackingID);

				stabilities.Add(skeleton.TrackingID, barycenterHelper.IsStable(skeleton.TrackingID) ? "Stable" : "Unstable");
				if (!barycenterHelper.IsStable(skeleton.TrackingID))
					continue;

				swipeGestureRecognizer.Add(skeleton.Joints
					.First(j => j.ID.Equals(JointID.HandRight)).Position, 
						kinectRuntime.SkeletonEngine);

				postureRecognizer.TrackPostures(skeleton);
			}

			skeletonDisplayManager.Draw(frame);
		}
	}
}
