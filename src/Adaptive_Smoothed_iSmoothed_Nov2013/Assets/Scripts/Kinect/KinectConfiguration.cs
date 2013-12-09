/*
 * Imtiaj Ahmed
 * Computer Science
 * University of Helsinki
 * iahmed.cs.helsinki@gmail.com
 * phone: +358453538393
 * 2013
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FubiNET;

public class KinectConfiguration: MonoBehaviour {

	public static string configXmlPath = "MSSDKConfig.xml",
					 recognnizerXmlPath = "UHgestureRecognizers.xml";
	private bool endSession;
	
	private GameObject cam;//, pointerObject;
	
	void Start () 
	{
		FubiNET.Fubi.init(new FubiUtils.SensorOptions(new FubiUtils.StreamOptions(640, 480, 30), new FubiUtils.StreamOptions(-1,-1,-1), new FubiUtils.StreamOptions(-1, -1, -1),FubiUtils.SensorType.KINECTSDK));
      	if(!FubiNET.Fubi.isInitialized())
			if (!FubiNET.Fubi.init(configXmlPath))
				{
					Debug.Log("Fubi can't initialiaze");
					BroadcastMessage("LabelMsgVisualize", "Failed to initialize kinect sensor....", SendMessageOptions.DontRequireReceiver);
					BroadcastMessage("KinectFailed", SendMessageOptions.DontRequireReceiver); //for session manager
				}
		
		if (Fubi.isInitialized())
			{
				FubiNET.Fubi.setAutoStartCombinationRecognition(true); //Fubi 0.5.1 wrapper
				FubiNET.Fubi.loadRecognizersFromXML(recognnizerXmlPath);
				BroadcastMessage("KinectStarted", SendMessageOptions.DontRequireReceiver); //for session manager
			}
		cam = GameObject.Find("Main Camera");	
		//pointerObject = GameObject.Find("PointerObjects");
	}

	void FixedUpdate()
	{
		FubiNET.Fubi.updateSensor();
	}
	
/*	void  OnApplicationQuit ()
	{
		FubiNET.Fubi.release();
		
	}
	*/
	void OnDestroy()
	{
		endSession = true;
		FubiNET.Fubi.release();			
	}
	
}
