/*
*Imtiaj Ahmed
*University of Helsinki
*2013
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FubiNET;

public class GestureReciever : MonoBehaviour {
	
	public static bool skipPinchGestureRecognition;
	
	private int GrabFilterThreshold = 3, UnGrabFilterThreshold = 3;
	
	private int rightHandGrabHistory, rightHandUnGrabHistory;
	
	private Dictionary<uint, bool> currentPostures  = new Dictionary<uint,bool>();
	
	private GameObject pointerObject;
	private bool lastGestureCheckingCompleted = true;
	
	//private bool[] currentPostures1 = new bool[(int)FubiNET.FubiPostures.Postures.NUM_POSTURES];
	
	void Start()
	{
		//pointerObject = GameObject.Find("PointerObjects");
		SendMessage("SendMaxNumOfFrameToBeChecked",SendMessageOptions.DontRequireReceiver);
	}
	
	IEnumerator SendMaxNumOfFrameToBeChecked()
	{
		yield return new WaitForSeconds(0.5f);
		//if(pointerObject)
		//{
		//	pointerObject.BroadcastMessage("SetMaxNumOfFrameToBeChecked", GrabFilterThreshold, SendMessageOptions.DontRequireReceiver);
		//	pointerObject.BroadcastMessage("SetUnGrabFilterThreshold", UnGrabFilterThreshold, SendMessageOptions.DontRequireReceiver);
		//}
	}
		
	void CheckForGesture() // For Fubi 0.4.0 Wrapper
	{
		uint closestId = FubiNET.Fubi.getClosestUserID();
		//Check postures for the closest user
       		
        if (closestId > 0)
        {
			
// Only user defined postures Fubi.getNumUserDefinedRecognizers()
            for (uint p = 0; p < FubiNET.Fubi.getNumUserDefinedRecognizers() ; ++p)
            {
				if(skipPinchGestureRecognition && (FubiNET.Fubi.getUserDefinedRecognizerName(p).Contains("RightHandCloseFist") || FubiNET.Fubi.getUserDefinedRecognizerName(p).Contains("RightHandOpenFist")))
				{
					rightHandGrabHistory = 0;
					rightHandUnGrabHistory = 0;
					continue;
				}
				
                if (FubiNET.Fubi.recognizeGestureOn(p, closestId) == FubiNET.FubiUtils.RecognitionResult.RECOGNIZED)
                {  
					if(FubiNET.Fubi.getUserDefinedRecognizerName(p).Contains("RightHandCloseFist"))
					{
						if(rightHandGrabHistory < GrabFilterThreshold)
							rightHandGrabHistory += 1;
						rightHandUnGrabHistory = 0;
						if(rightHandGrabHistory < GrabFilterThreshold) continue;
						else rightHandGrabHistory = 0;
					}
					if(FubiNET.Fubi.getUserDefinedRecognizerName(p).Contains("RightHandOpenFist"))
					{
						if(rightHandUnGrabHistory < UnGrabFilterThreshold)
							rightHandUnGrabHistory += 1;
						rightHandGrabHistory = 0;
						if(rightHandUnGrabHistory < UnGrabFilterThreshold) continue;
						else rightHandUnGrabHistory = 0;
					}
					
					
                    // Posture recognized
                   if (!currentPostures.ContainsKey(p) || !currentPostures[p])// && (maxCount == GrabFilterThreshold))
                    {
                        // Posture start
                        currentPostures[p] = true;
						
						//pointerObject.BroadcastMessage("RecognizedGesture", FubiNET.Fubi.getUserDefinedRecognizerName(p),SendMessageOptions.DontRequireReceiver);
					
                    }
                }
                else{ 
						if (currentPostures.ContainsKey(p) && currentPostures[p])
	                	{
	                    // Posture end
							currentPostures[p] = false;
						}
					}
            }
//Predefined postures
			/*
	        for (int p = 0; p < (int) FubiNET.FubiPostures.Postures.NUM_POSTURES; ++p)
	            {
	                if (FubiNET.Fubi.recognizeGestureOn((FubiNET.FubiPostures.Postures)p, closestId) == FubiNET.FubiUtils.RecognitionResult.RECOGNIZED)
	                {
	                    if (!currentPostures1[p])
	                    {
	                        // Posture recognized start
	                        currentPostures1[p] = true;
	                    }
	                }
	                else if (currentPostures1[p])
	                {
						//posture end
						gameObject.BroadcastMessage("BroadcastGesture", FubiNET.FubiPostures.getPostureName((FubiNET.FubiPostures.Postures)p),SendMessageOptions.DontRequireReceiver);
                    	//gameObject.BroadcastMessage("LabelMsgVisualize",FubiNET.FubiPostures.getPostureName((FubiNET.FubiPostures.Postures)p),SendMessageOptions.DontRequireReceiver);
                    
	                    currentPostures1[p] = false;
	                }
	            }
            
           

// posture combinations
            for (uint pc = 0; pc < FubiNET.Fubi.getNumUserDefinedCombinationRecognizers(); ++pc)
            {
             // Only user defined postures
                
                if (FubiNET.Fubi.getCombinationRecognitionProgressOn(FubiNET.Fubi.getUserDefinedCombinationRecognizerName(pc), closestId) == FubiNET.FubiUtils.RecognitionResult.RECOGNIZED)
                {
                    // Posture recognized
                    gameObject.BroadcastMessage("BroadcastGesture", FubiNET.Fubi.getUserDefinedCombinationRecognizerName(pc),SendMessageOptions.DontRequireReceiver);
					//gameObject.BroadcastMessage("LabelMsgVisualize",FubiNET.Fubi.getUserDefinedCombinationRecognizerName(pc),SendMessageOptions.DontRequireReceiver);
                
                }
                else
                    FubiNET.Fubi.enableCombinationRecognition(FubiNET.Fubi.getUserDefinedCombinationRecognizerName(pc), closestId, true);
               
            }

           // for (uint pc = 0; pc < (uint)FubiNET.FubiPostures.Combinations.NUM_POSTURE_COMBINATIONS; ++pc) //Fubi 0.4.0
			 for (uint pc = 0; pc < (uint) FubiNET.FubiPredefinedGestures.Combinations.NUM_COMBINATIONS; ++pc) //Fubi 0.5.1 ; instead of FubiPostures use FubiPredefinedGestures 
            {
                if (FubiNET.Fubi.getCombinationRecognitionProgressOn((FubiNET.FubiPredefinedGestures.Combinations)pc, closestId) == FubiNET.FubiUtils.RecognitionResult.RECOGNIZED)
                    {
                        // Posture recognized
						gameObject.BroadcastMessage("BroadcastGesture", FubiNET.FubiPredefinedGestures.getCombinationName((FubiNET.FubiPredefinedGestures.Combinations)pc),SendMessageOptions.DontRequireReceiver);
						//gameObject.BroadcastMessage("LabelMsgVisualize",FubiNET.FubiPostures.getCombinationName((FubiNET.FubiPostures.Combinations)pc),SendMessageOptions.DontRequireReceiver);
                	}
                else
                    FubiNET.Fubi.enableCombinationRecognition((FubiNET.FubiPredefinedGestures.Combinations)pc, closestId, true);
               
            }
          */  
        }
		
	}
	
	
	
	void UpdateWithKinectFPS()
	{
		if(!lastGestureCheckingCompleted) return;
		
		SendMessage("UpdateGesture", SendMessageOptions.DontRequireReceiver);	
	}	
	
	void FixedUpdate()
	{
		if(!lastGestureCheckingCompleted) return;
		
		SendMessage("UpdateGesture", SendMessageOptions.DontRequireReceiver);	
	}	
	
	void UpdateGesture()
	{
		lastGestureCheckingCompleted = false;
		
		CheckForGesture();
		
		lastGestureCheckingCompleted = true;
	}

}
