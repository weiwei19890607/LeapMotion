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

public class KinectRelativeHandPosition : MonoBehaviour {
	
	private float X,Y;
	private float magneticField = 100f;//0.0125
	private bool magnetize = false;
	
	private bool foodItemGrabbed = false, hideCursor, lastHandCursorUpdateCompleted = true;
	Vector3 handCursor, lastHandCursor;
	private float centerX, centerY;
	private float cameraFixedZ = 0.06f;
	private GameObject pointerObject;
	// Use this for initialization
	void Start () 
	{
	 	handCursor = lastHandCursor = Vector3.zero;
		handCursor.z = -(UnityEngine.Camera.mainCamera.transform.position.z + cameraFixedZ);
		pointerObject = GameObject.Find("PointerObjects");
	}
	
	void hideHandCursor(bool hide)
	{
		if(hide)
		{
			Vector3 pos = new Vector3(100f,100f,0f);
			transform.position = pos;
		}
		
		hideCursor = hide;
	}
	
/*	
	// Update is called once per frame
	void Update () {
		UpdateHandCursor();	
	//Debug.Log("XXX "+ handCursor);
		transform.position = UnityEngine.Camera.mainCamera.ScreenToWorldPoint(handCursor);
		//UnityEngine.Camera.mainCamera.ScreenToWorldPoint(ScaleXY(Body.ShoulderCenter, true, Body.HandRight));
	}
*/	
	void SkeletonUpdated()
	{
		if(hideCursor) return;
		
		//if(!lastHandCursorUpdateCompleted) return;
		
		SendMessage("UpdateHandCursor",SendMessageOptions.DontRequireReceiver);
	}
	
	
	float ScaleY(Vector3 joint)
    {/*
        float y = ((Screen.height / 0.4f) * (joint.y) * 1.3f) +
                   (Screen.height / 2f);
				  */
		float cntrY = (Body.ShoulderCenter.y + Body.ShoulderRight.y)/2f;
		if(Mathf.Abs(cntrY-centerY) > 0.25f)
			centerY = cntrY;
		float y = ((Screen.height / 0.4f) * (joint.y-centerY) *1.3f) +
                   (Screen.height / 2f);
		//Debug.Log("Joint Y "+ joint.y);
        return y;
		
    }

     Vector2 ScaleXY(Vector3 shoulderCenter, bool rightHand, Vector3 joint)
    {
        float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		Vector2 ScaledXY;
		
        float x = 0;
        float y = ScaleY(joint);

        // if rightHand then place shouldCenter on left of screen
        // else place shouldCenter on right of screen
		
		if(Mathf.Abs(centerX-shoulderCenter.x)>0.1f)
			centerX = shoulderCenter.x;
		
        if (rightHand) 
        {
            x = (joint.x - centerX) * screenWidth * 3f;
        }
        else 
        {
            x = screenWidth - (((centerX ) - joint.x) * (screenWidth * 3f));
        }


        if (x < 0)
        {
            x = 0;
        }
        else if (x > screenWidth - 5)
        {
            x = screenWidth - 5;
        }
		
		if(y > screenHeight -5)
		{
			y = screenHeight - 5;
		}
		/*
        if (y < 0)
        {
            y = 0;
        }
		 */
        ScaledXY.x = x;
        ScaledXY.y = y;
		return ScaledXY;	
    }
	
	void UpdateHandCursor()
	{
		lastHandCursorUpdateCompleted = false;
		
		handCursor.z = -(UnityEngine.Camera.mainCamera.transform.position.z + cameraFixedZ);
		
		Vector2 tempXY = ScaleXY(Body.ShoulderRight, true, Body.HandRight);
		
		if(!foodItemGrabbed && magnetize)
		{
			if(Mathf.Abs(handCursor.x - tempXY.x) > magneticField)
				handCursor.x = tempXY.x;
			if(Mathf.Abs(handCursor.y - tempXY.y) > magneticField)
				handCursor.y = tempXY.y;
		}
		else 
		{
			handCursor.x = tempXY.x;
			handCursor.y = tempXY.y;
		}
	
		transform.position = UnityEngine.Camera.mainCamera.ScreenToWorldPoint(handCursor);
		lastHandCursor = handCursor;
		
		lastHandCursorUpdateCompleted = true;
		
		pointerObject.BroadcastMessage("HandCursorUpdated",SendMessageOptions.DontRequireReceiver);
	}
	
	void SetMagnetize(bool magnetism)
	{
		//magnetize = magnetism;
	}
	
	void SetFoodItemGrabbedAndResetMagnetize()
	{
		foodItemGrabbed = true;
		magnetize = false;
		//Debug.Log("SetFoodItemGrabbedAndResetMagnetize");
	}
	
	void ResetFoodItemGrabbed()
	{
		foodItemGrabbed = false;
		//Debug.Log("ResetFoodItemGrabbed");
	}
}
