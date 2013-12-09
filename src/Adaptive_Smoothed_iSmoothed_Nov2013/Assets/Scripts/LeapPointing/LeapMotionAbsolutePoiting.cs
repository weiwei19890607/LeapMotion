/* 
 * Imtiaj Ahmed
 * Jin Jiawei
 * University of Helsinki
 * 2013
 */

using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;


public class LeapMotionAbsolutePoiting : MonoBehaviour {
	public Texture crossHair;
	public Texture2D cursorTexture;
	
	private int fingercount = 0;
	private string status;
	
	private int calibrationState = 0;
	private bool calibrateDirection;
	private float calibrationWaitTime = 3f, pointCalibrationRunningTime;
	private string calibrationPoint = "LeftDown";
	private Vector2 avgLeftDownCalibrationPoint, avgRightUpCalibrationPoint;
	private Vector normalizedPoint;
	
    Controller controller;
	
	void Start()
	{
		SendMessage("SendMaxNumOfFrameToBeChecked",SendMessageOptions.DontRequireReceiver);
		controller = new Controller();
		status = string.Empty;
		avgLeftDownCalibrationPoint = PointingParameters.LeapCalibrationLeftDownPoint;
		avgRightUpCalibrationPoint = PointingParameters.LeapCalibrationRightUpPoint;
		normalizedPoint = new Vector(0f,0f,0f);
		if (avgLeftDownCalibrationPoint == new Vector2(0,0) && avgRightUpCalibrationPoint ==new Vector2(1,1)){
			status = "Press 'L' to start calibration!";
		}
	}
	
	IEnumerator SendMaxNumOfFrameToBeChecked()
	{
		yield return new WaitForSeconds(0.5f);
		BroadcastMessage("SetMaxNumOfFrameToBeChecked", 2, SendMessageOptions.DontRequireReceiver);
		BroadcastMessage("SetUnGrabFilterThreshold", 2, SendMessageOptions.DontRequireReceiver);
		
	}
	
	void OnGUI () 
	{
		GUI.Label(new Rect(UnityEngine.Screen.width/2 -100,UnityEngine.Screen.height/2 - 200,200,80),status);
		if(calibrationState == 1)
			GUI.DrawTexture(new Rect(0f, UnityEngine.Screen.height-50f,50f,50f),crossHair);
		else if(calibrationState == 2)
			GUI.DrawTexture(new Rect(UnityEngine.Screen.width-50, 0f,50f,50f),crossHair);
		//GUI.DrawTexture(new Rect(PointingParameters.MotorPoint.x, UnityEngine.Screen.height - PointingParameters.MotorPoint.y, 20f, 20f), cursorTexture);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.L))
		{
			calibrateDirection = true;
			pointCalibrationRunningTime = 0f;
			status ="Pleae point your index finger to the CrossHair";
			calibrationPoint = "LeftDown";
			calibrationState = 1;
			
		}	
    }
	
	void FixedUpdate()//UpdateFrame()
	{
		Frame frame = controller.Frame();
		var finger = frame.Fingers;
		fingercount = frame.Fingers.Count;
	    OnFingersRegistered(finger);
		
	}
	
	void OnDestroy()
    {
        controller.Dispose();
    }

	void OnFingersRegistered(FingerList fingers)
    {
        var screen = controller.CalibratedScreens.ClosestScreenHit(fingers[0]);
        //var coordinate; 
		NormalizeFingerPoint(fingers, screen);
		
		PointingParameters.MotorPoint.x = normalizedPoint.x * screen.WidthPixels;
		PointingParameters.MotorPoint.y = normalizedPoint.y * screen.HeightPixels;
		
		//GUI.Label(new Rect(5,50,200,80),status);

    }
	
	
	
    void SetLeapAsMouse(FingerList fingers, Vector coordinate)
    {
        var screen = controller.CalibratedScreens.ClosestScreenHit(fingers[0]);
        if (screen == null || !screen.IsValid) return;
       // EnableLeapAsCursor(coordinate, fingers);
    }

	void NormalizeFingerPoint(FingerList fingers, Leap.Screen screen)
    {	
        Vector tempNormPoint = screen.Intersect(fingers[0].TipPosition, fingers[0].Direction, true, 10.0F);
        
		if((tempNormPoint.x.ToString()=="NaN")||tempNormPoint.y.ToString()=="NaN")
			return;
		
		normalizedPoint = tempNormPoint;
		
		if (calibrateDirection)
		{
			Calibration(new Vector2(normalizedPoint.x, normalizedPoint.y));
		}
		
	//	Calibration(xNormalized,yNormalized);
		normalizedPoint.x = (normalizedPoint.x - avgLeftDownCalibrationPoint.x) / (avgRightUpCalibrationPoint.x - avgLeftDownCalibrationPoint.x);
		normalizedPoint.y = (normalizedPoint.y - avgLeftDownCalibrationPoint.y) / (avgRightUpCalibrationPoint.y - avgLeftDownCalibrationPoint.y);
		
		
		if (normalizedPoint.x < 0) {
			normalizedPoint.x = 0;
		}
		else if (normalizedPoint.x > 1) {
			normalizedPoint.x = 1;
		}
		if (normalizedPoint.y < 0) {
			normalizedPoint.y = 0;
		}
		else if (normalizedPoint.y > 1) {
			normalizedPoint.y = 1;
		}		
		/*
        var x = (normalizedPoint.x * screen.WidthPixels);
        var y = screen.HeightPixels - (normalizedPoint.y * screen.HeightPixels);
		var screenRatios = new Vector(x, y, 0);
        return screenRatios;
        */
    }
	
	void Calibration(Vector2 rawPoint)
	{
		pointCalibrationRunningTime += UnityEngine.Time.deltaTime;
		
		if(calibrationPoint == "LeftDown")
		{
			if(pointCalibrationRunningTime > calibrationWaitTime)
			{
				pointCalibrationRunningTime = -0.2f;
				calibrationPoint = "RightUp";
				calibrationState = 2;
			}
			else
			{
				avgLeftDownCalibrationPoint = (avgLeftDownCalibrationPoint + rawPoint) / 2f; 
				PointingParameters.LeapCalibrationLeftDownPoint = avgLeftDownCalibrationPoint;
			}
		}
		else if(pointCalibrationRunningTime>=0f)
		{
			if(pointCalibrationRunningTime > calibrationWaitTime)
			{
				SendMessage("CalibrationFinished",SendMessageOptions.DontRequireReceiver);
				
				calibrateDirection = false;
			}
			else
			{
				avgRightUpCalibrationPoint = (avgRightUpCalibrationPoint + rawPoint) / 2f; 
				PointingParameters.LeapCalibrationRightUpPoint = avgRightUpCalibrationPoint;
			}
		}
	}
	
	IEnumerator CalibrationFinished()
	{
		status = "Thanks. Calibration finished.";
		calibrationState = 0;
		yield return new WaitForSeconds(1f);
		if(!calibrateDirection){
			status = "";
		}
		
		Application.LoadLevel("LeapMotion_PointingParameters");
		
	}
	
	
}



