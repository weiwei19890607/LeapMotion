using UnityEngine;
using System.Collections;

public class KinectAbsoluteHandPosition : MonoBehaviour {

	private float cameraFixedZ = 0.06f;
	private float magneticField = 100f;//0.0125
	private bool magnetize = false, foodItemGrabbed = false;
	
	private Vector3 handCursor;
	private float filter = 0.3f, MaxHandSpeedForPinch = 0.02f;
	Vector3 startPoint, dirPoint, lastDirPoint;
	private Vector3[] elb, hnd;
	
	private Vector3 currentHandRight, currentElbowRight, lastHandRight, LastElbowRight;
	
	private float handRightConfidence, elbowRightConfidence;
	
	private float rightHandLastUpdateTime, rightHandCurrentUpdateTime;
	
	
	// Use this for initialization
	void Start () {
		elb = new Vector3[4];
		hnd = new Vector3[4];
	}
	
	void elbPos()
	{
		for(int i=1; i<3; i++)
			elb[i-1] = elb[i]; 
		if(elbowRightConfidence >0.5f)
			elb[3] = (elb[3]+currentElbowRight)/2f;
		else
			elb[3] = filter*currentElbowRight + (1-filter)*elb[3];
		elb[2]= (elb[0]+elb[1]+elb[2]+elb[3])/4f;
	}
	void hndPos()
	{
		for(int i=1; i<3; i++)
			hnd[i-1] = hnd[i]; 
		
		if(handRightConfidence > 0.5f)
		{
			if(Vector3.Distance(lastHandRight,currentHandRight)< 0.006f)
				
					hnd[3] = .05f*currentHandRight + .95f*hnd[3];
				
			else 
				hnd[3] = currentHandRight;
		}
		else 
			hnd[3] = 0.01f*currentHandRight + 0.99f*hnd[3];
		
		hnd[2] = (hnd[0]+hnd[1]+hnd[2]+hnd[3])/4f;
		rightHandCurrentUpdateTime = UnityEngine.Time.time;
		
	}
	
	Vector3 transformYZ(Vector3 pos)
	{
		pos.y+=1.01f;
		pos.z = -pos.z;
		return pos;
	}
	
	// Update is called once per frame
	void SkeletonUpdated () 
	{
		RaycastHit hit;
		lastHandRight = currentHandRight;
		LastElbowRight = currentHandRight;
		currentHandRight = transformYZ(Body.HandRight);
		currentElbowRight = transformYZ(Body.ElbowRight);
		handRightConfidence = Body.HandRightConfidence;
		elbowRightConfidence = Body.ElbowRightConfidence;
		
		
		hndPos();
		if((Vector3.Distance(dirPoint,hnd[2])/(rightHandCurrentUpdateTime-rightHandLastUpdateTime))> MaxHandSpeedForPinch)
			GestureReciever.skipPinchGestureRecognition = true;
		else
			GestureReciever.skipPinchGestureRecognition = false;
		dirPoint = hnd[2];
		rightHandLastUpdateTime = rightHandCurrentUpdateTime;
		
				
		if((Vector3.Distance(dirPoint,lastDirPoint)>0.004f)&&(Vector3.Distance(startPoint, currentElbowRight)>.04F))
		{	
			elbPos();
			startPoint = elb[2];
		}
		
		lastDirPoint = dirPoint;
	
		Vector3 dir = (dirPoint-startPoint);
		int layerMask = 1 << 8;
	
		if (!Physics.Raycast(startPoint, dir, out hit, 10f, layerMask))
            return;
               
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x = (1-pixelUV.x)*Screen.width;
        pixelUV.y = (1-pixelUV.y)*Screen.height;
		
		
		Vector3 t = new Vector3( pixelUV.x, pixelUV.y, -(UnityEngine.Camera.mainCamera.transform.position.z + cameraFixedZ));
		
		if(!foodItemGrabbed && magnetize)
		{
			if(Mathf.Abs(handCursor.x - t.x) > magneticField)
				handCursor.x = t.x;
			if(Mathf.Abs(handCursor.y - t.y) > magneticField)
				handCursor.y = t.y;
		}
		else 
		{
			handCursor.x = t.x;
			handCursor.y = t.y;
		}
		handCursor.z = t.z;
		//transform.position = UnityEngine.Camera.mainCamera.ScreenToWorldPoint(handCursor);
		PointingParameters.MotorPoint.x = handCursor.x;
		PointingParameters.MotorPoint.y = handCursor.y;
		//Debug.Log("kinect xy "+handCursor);
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