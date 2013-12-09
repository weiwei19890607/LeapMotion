/*
 * Jin Jiawei
 * 
 * University of Helsinki
 * 
*/
using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.ComponentModel;

using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

public class LeapGestureTrigger : MonoBehaviour
{
	public static int fingercount = 0;
	Controller controller;

	// Use this for initialization
	void Start ()
	{
		controller = new Controller ();
	}
	

	// Update is called once per frame
	void Update ()
	{
		Frame frame = controller.Frame ();
		var finger = frame.Fingers;
		fingercount = frame.Fingers.Count;
		if (fingercount == 2){
				PointingParameters.isSelected = true;
			}
			else{
				PointingParameters.isSelected = false;
			}		
	}
	
	void OnClosing (object sender, CancelEventArgs cancelEventArgs)
	{
		controller.Dispose ();
	} 
}

