/*
 * Imtiaj Ahmed
 * Computer Science
 * University of Helsinki
 * iahmed.cs.helsinki@gmail.com
 * phone: +358453538393
 * 15.01.2012
 * 
 * 
 */
using UnityEngine;
using System.Collections;

public class SessionManager : MonoBehaviour {
	
	private bool isInSessionMsgSend, isNotInSessionMsgSend;
 	public static bool isKinectStarted;
	
	private GameObject pointerObject;
	
	void Start()
	{
		pointerObject = GameObject.Find("PointerObjects");
	}
	
	void KinectFailed()
	{
		isKinectStarted = false;
	}
	
	void KinectStarted()
	{
		isKinectStarted = true;
	}
	
	
	// Update is called once per frame
	void UpdateWithKinectFPS () 
	{
		if(!isKinectStarted)return;
		
		if((Body.Spine.x > -0.7f) && (Body.Spine.x < 0.7f) && (Body.Spine.z > 1.2f)) //make sure user is in safe gestural position
			{
				if(!isInSessionMsgSend)
				{
					BroadcastMessage("SessionManager_InSession",SendMessageOptions.DontRequireReceiver);
					BroadcastMessage("LabelMsgVisualize", "Session Started",SendMessageOptions.DontRequireReceiver);
					pointerObject.BroadcastMessage("SessionManager_InSession",SendMessageOptions.DontRequireReceiver);
					isInSessionMsgSend = true;
					isNotInSessionMsgSend = false;
				}
			}
			else if(!isNotInSessionMsgSend)
			{
				BroadcastMessage("SessionManager_NotInSession",SendMessageOptions.DontRequireReceiver);
				BroadcastMessage("LabelMsgVisualize", "Please stand between -0.6 meter left and 0.6 meter  right , and 1.2 meter front of the Kinect",SendMessageOptions.DontRequireReceiver);
				pointerObject.BroadcastMessage("SessionManager_NotInSession",SendMessageOptions.DontRequireReceiver);
				isInSessionMsgSend = false;
				isNotInSessionMsgSend = true;
			}
	}
	
}
