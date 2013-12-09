/*Imtiaj Ahmed
 * University of Helsinki
 * 2013
 * */
using UnityEngine;
using System.Collections;

public class EyeTracking : MonoBehaviour {
	private int screenHeight, screenWidth;
	private Vector3 avgVector;
	private float[] xs;
	private float[] ys;
	
	// Use this for initialization
	void Start () {
		screenWidth = Screen.currentResolution.width;
		screenHeight = Screen.currentResolution.height;
		xs = new float[5];
		ys = new float[5];
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 EyePosition;
		
		EyePosition.x = GlobalClass.eyeCoordinate_x * screenWidth;
		EyePosition.y = GlobalClass.eyeCoordinate_y * screenHeight;
		EyePosition.y = screenHeight - EyePosition.y;
		
		xs[0] = xs[1];
		xs[1] = xs[2];
		xs[2] = xs[3];
		xs[3] = xs[4];
		xs[4] = EyePosition.x;

		ys[0] = ys[1];
		ys[1] = ys[2];
		ys[2] = ys[3];
		ys[3] = ys[4];
		ys[4] = EyePosition.y;
		
		EyePosition.x = 0;
		EyePosition.y = 0;
	
		for(int i = 0; i  < 5; i++ ) {
			EyePosition.x += xs[i];
			EyePosition.y += ys[i];
		}
		//EyePosition contains the screen coordinates
		EyePosition.x = EyePosition.x / 5;
		EyePosition.y = EyePosition.y / 5;
		PointingParameters.MotorPoint = EyePosition;		
	}
}
