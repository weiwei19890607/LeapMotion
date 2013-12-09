using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public class PointingParameters{
	
	public static float Xpixels = 1f, Dmin = 1f, Dmax = 150f;
	
	public static float D90, Vmin, Vmax, Gmin, Gmax;
	public static float LeapMotion_D90, LeapMotion_Vmin = 130f, LeapMotion_Vmax= 650f, LeapMotion_Gmin = 0.40f, LeapMotion_Gmax =1.05f;
	public static float Kinect_D90, Kinect_Vmin = 130f, Kinect_Vmax= 650f, Kinect_Gmin = 0.40f, Kinect_Gmax =1.05f;
	
	public static Vector2 MotorPoint, DispPoint; //motor point of the device/sensor  => (Xmot,Ymot), DispPoint (Xdisp, Ydisp)
	public static Vector2 LeapCalibrationLeftDownPoint = new Vector2(0f,0f), LeapCalibrationRightUpPoint = new Vector2(1f,1f);
	public static int LeapFPS = 30, KinectFPS = 30;
	public static bool isSelected;
	public static string UseModel = "";
	public static string UseSelectionDevice, UseSelectionMethod;
	public static string UsePointingDeivce;
	public static string UseExperimentTask;
	public static string ObjectColor = "green", TargetColor = "yellow";
	
	public static int NumberOfTrack = 3, NumberOfTargetCircle = 8;
	public static float MinRadiusOfTrack, MaxRadiusOfTrack, MinRadiusOfTargetCircle, MaxRadiusOfTargetCircle, ObjectRadius;
	
	public static void SetParameters()
	{
		if(UsePointingDeivce.Contains("LeapMotion"))
		{
			LeapMotion_D90 = D90;
			LeapMotion_Vmin = Vmin;
			LeapMotion_Vmax = Vmax;
			LeapMotion_Gmin = Gmin;
			LeapMotion_Gmax = Gmax;
		}
		else if(UsePointingDeivce.Contains("Kinect"))
		{
			Kinect_D90 = D90;
			Kinect_Vmin = Vmin;
			Kinect_Vmax = Vmax;
			Kinect_Gmin = Gmin;
			Kinect_Gmax = Gmax;
		}
	}
	public static void GetParameters()
	{
		if(UsePointingDeivce.Contains("LeapMotion"))
		{
			D90 = LeapMotion_D90;
			Vmin = LeapMotion_Vmin;
			Vmax = LeapMotion_Vmax;
			Gmin = LeapMotion_Gmin;
			Gmax = LeapMotion_Gmax;
		}
		else if(UsePointingDeivce.Contains("Kinect"))
		{
			D90 = Kinect_D90;
			Vmin = Kinect_Vmin;
			Vmax = Kinect_Vmax;
			Gmin = Kinect_Gmin;
			Gmax = Kinect_Gmax;
		}
	}
	
}

public class LeapAsMouse
{
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
}