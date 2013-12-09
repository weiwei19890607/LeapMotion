using UnityEngine;
using System.Collections;

public class FPSupdate : MonoBehaviour {
	
	public enum Device{Leap, Kinect};
	public Device whichDevice = Device.Leap;
	private bool endSession;
	private float fpsDeltaTime;
	// Use this for initialization
	void Start () 
	{
		if(whichDevice == Device.Leap)
			fpsDeltaTime = 1.0f / PointingParameters.LeapFPS;
		else 
			fpsDeltaTime = 1.0f / PointingParameters.KinectFPS;
		SendMessage("BroadcastUpdateFrame", SendMessageOptions.DontRequireReceiver);
	}
	
	
	IEnumerator BroadcastUpdateFrame()
	{
		while(!endSession)
		{
			yield return new WaitForSeconds(fpsDeltaTime);
			BroadcastMessage("UpdateFrame",SendMessageOptions.DontRequireReceiver);
		}
		yield return null;
	}
	
	void OnDestroy()
	{
		endSession = true;		
	}
}
