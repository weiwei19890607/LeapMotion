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

public class FilterGrabGestures : MonoBehaviour {
	
	public Hand whichHand = Hand.Right;
	
	private bool isGrabbed = false;
	private bool skipGrabUngrab = false;
	private int maxNumOfFrameToBeChecked = 5;
   
	// Use this for initialization
	void Start () 
	{
		
	}
	
	void SetMaxNumOfFrameToBeChecked(int maxFrame2beChecked)
	{
		maxNumOfFrameToBeChecked = maxFrame2beChecked;
	}
	
	void deactivateGraspGesture()
	{
		skipGrab_UnGrabDetection(true);
	}
	
	void activateGraspGesture()
	{
		skipGrab_UnGrabDetection(false);
	}
	
	void skipGrab_UnGrabDetection(bool skip)
	{
		skipGrabUngrab = skip;
	}
	
	void RecognizedGesture(string gestureName)
	{
		if(skipGrabUngrab) return; //no need to check grab unGrab now
		
		if(whichHand == Hand.Right)
		{
			
			switch(gestureName)
			{
				case "RightHandOpenFist":
					if(isGrabbed){
						gameObject.SendMessage("UnPinched", SendMessageOptions.DontRequireReceiver);
						isGrabbed = false;
						PointingParameters.isSelected = false;
					}
					break;
				case "RightHandCloseFist": 	
					if(!isGrabbed){
						gameObject.SendMessage("Pinched", maxNumOfFrameToBeChecked,SendMessageOptions.DontRequireReceiver);
						isGrabbed = true;
						PointingParameters.isSelected = true;
					}
					break;					
			}
		}
		else if(whichHand == Hand.Left)
		{
			switch(gestureName)
			{
				case "LeftHandOpenFist": 	
					if(isGrabbed){
						gameObject.SendMessage("UnPinched", SendMessageOptions.DontRequireReceiver);
						PointingParameters.isSelected = false;
						isGrabbed = false;
					}
					break;
				case "LeftHandCloseFist": 	
					if(!isGrabbed){
											
						gameObject.SendMessage("Pinched", maxNumOfFrameToBeChecked,SendMessageOptions.DontRequireReceiver);
						isGrabbed = true;
						PointingParameters.isSelected = true;
					}
					break;
			}
		}
	}
	
}
