using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ExperimentManager : MonoBehaviour
{
	private bool useLeapMotion;
	private bool useAdaptive, useSmoothed, useOur;
	private GameObject device, model, gesture, objectCircle, objectCircle2;
	private CircleController cc;
	private int round;
	private int currentTrail;
	private int PreviousRound;
	private bool isLastTrail;
	private string status;
	public GUIStyle myGuiStyle;
	
	
	//circle information
	private int numberOftrack = PointingParameters.NumberOfTrack, numberOfTargetCircle = PointingParameters.NumberOfTargetCircle;
	private float minRadiusOfTrack = PointingParameters.MinRadiusOfTrack, maxRadiusOfTrack = PointingParameters.MaxRadiusOfTrack, objectRadius = PointingParameters.ObjectRadius, Angle;
	private float minRadiusOfTargetCircle = PointingParameters.MinRadiusOfTargetCircle, maxRadiusOfTargetCircle = PointingParameters.MaxRadiusOfTargetCircle;
	//private RadiusCombination rc;
	//List<RadiusCombination> radiusCombinationList = new List<RadiusCombination> ();
	private List<float> RadiusTargetCombination = new List<float> ();
	private List<float> RadiusTrackCombination = new List<float> ();
	private List<Vector2> CoordinateAllCircle = new List<Vector2> ();
	private System.Random rand = new System.Random ();
	private GameObject go_Target, go_Object;
	//parameter for circle object
	static Vector2 init_circle_position = new Vector2 (Screen.width / 2f, Screen.height / 2f);
	private static Vector2 currentObjectCoordinate;
	private static Vector2 currentTargetCoordinate;
	static int init_circle_r = 50;
	bool tmp_isSelected;
	//private float MinRadiusOfTrack, MaxRadiusOfTrack, MinRadiusOfTargetCircle, MaxRadiusOfTargetCircle, ObjectRadius, Angle;
	//private int NumberOfTrack, NumberOfTargetCircle;
	
	private float currentTargetRadius;
	private float currentTrackRadius;
	
	//define the structure for the logging information of each trial
	
	private struct LogInfo
	{
		//device name, model name,time, strat position, end position, distance,  bla bla bla
	}
	
	void Start ()
	{
		//init round and trail
		round = 0;
		currentTrail = 0;
		
		//init status message
		status ="Experiment is running!";
		
		//set angle
		Angle = 360f / (float)numberOfTargetCircle;
		//Angle = Convert.ToSingle(Math.PI * Angle / 180f);
		
		
		//set target radius
		SetTargetRadiusCombination ();
		
		//set track radius
		SetTrackRadiusCombination ();
		
		//set device
		if (PointingParameters.UsePointingDeivce.Contains ("LeapMotion")) {
			device = Instantiate (Resources.Load ("LeapMotion") as GameObject) as GameObject;
			device.name = "LeapMotion";
			device.transform.parent = this.transform;
		} else if (PointingParameters.UsePointingDeivce.Contains ("Kinect")) {
			device = Instantiate (Resources.Load ("Kinect") as GameObject) as GameObject;
			device.name = "Kinect";
			device.transform.parent = this.transform;
		}else if (PointingParameters.UsePointingDeivce.Contains ("Tobii")) {
			device = Instantiate (Resources.Load ("Tobii") as GameObject) as GameObject;
			device.name = "Tobii";
			device.transform.parent = this.transform;
		}else if (PointingParameters.UsePointingDeivce.Contains ("Mirametrix")) {
			device = Instantiate (Resources.Load ("Mirametrix") as GameObject) as GameObject;
			device.name = "Mirametrix";
			device.transform.parent = this.transform;
		}
		
		//set Model
		
		if (PointingParameters.UseModel.Contains ("Adaptive")) {
			model = Instantiate (Resources.Load ("Adaptive") as GameObject) as GameObject;
			model.name = "Adaptive";
			model.transform.parent = this.transform;
		} else if (PointingParameters.UseModel.Contains ("Smoothed")) {
			model = Instantiate (Resources.Load ("Smoothed") as GameObject) as GameObject;
			model.name = "Smoothed";
			model.transform.parent = this.transform;
		} else {
			model = Instantiate (Resources.Load ("Our") as GameObject) as GameObject;
			model.name = "Our";
			model.transform.parent = this.transform;
		}
		
		//set selection gesture
		
		if (PointingParameters.UseSelectionDevice.Contains ("LeapMotion")) {
			if (PointingParameters.UseSelectionMethod.Contains ("Trigger")) {
				gesture = Instantiate (Resources.Load ("LeapMotionTrigger") as GameObject) as GameObject;
				gesture.name = "LeapMotionTrigger";
				gesture.transform.parent = this.transform;
			}
		}else if(PointingParameters.UseSelectionDevice.Contains ("Kinect")){
			if (PointingParameters.UseSelectionMethod.Contains ("Pinch")) {
				gesture = Instantiate (Resources.Load ("KinectPinch") as GameObject) as GameObject;
				gesture.name = "KinectPinch";
				gesture.transform.parent = this.transform;
			}
		}
		
		
		LoadNextTrail ();
		LoadNextObjectAndTarget ();
		//object circle
		objectCircle = Instantiate (Resources.Load ("Circle2D") as GameObject) as GameObject;
		objectCircle.name = "Object";
		objectCircle.transform.parent = this.transform;
		object[] parameter_SetCirclePosition = new object[2];
		parameter_SetCirclePosition [0] = currentObjectCoordinate;
		parameter_SetCirclePosition [1] = Convert.ToInt32 (objectRadius);
		objectCircle.SendMessage ("SetCirclePositionAndSize", parameter_SetCirclePosition, SendMessageOptions.DontRequireReceiver);
		
		//target circle
		objectCircle2 = Instantiate (Resources.Load ("Circle2D") as GameObject) as GameObject;
		objectCircle2.name = "Target";
		objectCircle2.transform.parent = this.transform;
		object[] parameter_SetCirclePosition2 = new object[2];
		parameter_SetCirclePosition2 [0] = currentTargetCoordinate;
		parameter_SetCirclePosition2 [1] = Convert.ToInt32 (currentTargetRadius);
		objectCircle2.SendMessage ("SetCirclePositionAndSize", parameter_SetCirclePosition2, SendMessageOptions.DontRequireReceiver);
		
		
		
		
		//generate all possible radius combination
		/*SetRadiusCombination();*/
		
		
			
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (20, 40, 200, 50), "Current Trail: " + currentTrail,myGuiStyle);
		GUI.Label (new Rect (20, 70, 200, 50), "Current Object Radius: " + objectRadius,myGuiStyle);
		GUI.Label (new Rect (20, 100, 200, 50), "Current Target Radius: " + currentTargetRadius,myGuiStyle);
		GUI.Label (new Rect (20, 130, 200, 50), "Current Track Radius: " + currentTrackRadius,myGuiStyle);
		GUI.Label (new Rect (20, 200, 500, 50), "Status: " + status,myGuiStyle);
	}
	
	void Update ()
	{
		
		objectCircle.SendMessage ("MoveCircle", PointingParameters.DispPoint, SendMessageOptions.DontRequireReceiver);
		
		objectCircle.SendMessage ("isClick", PointingParameters.isSelected, SendMessageOptions.DontRequireReceiver);

		
		if (Vector2.Distance (objectCircle.GetComponent<CircleController> ().coordinate_circle, objectCircle2.GetComponent<CircleController> ().coordinate_circle) < Convert.ToInt32 (currentTargetRadius) && !objectCircle.GetComponent<CircleController> ().flag_isClick) {
			LoadNextObjectAndTarget ();
			object[] parameter_SetCirclePosition = new object[2];
			parameter_SetCirclePosition [0] = currentObjectCoordinate;
			parameter_SetCirclePosition [1] = Convert.ToInt32 (objectRadius);
			objectCircle.SendMessage ("SetCirclePositionAndSize", parameter_SetCirclePosition, SendMessageOptions.DontRequireReceiver);
			
			object[] parameter_SetCirclePosition2 = new object[2];
			parameter_SetCirclePosition2 [0] = currentTargetCoordinate;
			parameter_SetCirclePosition2 [1] = Convert.ToInt32 (currentTargetRadius);
			objectCircle2.SendMessage ("SetCirclePositionAndSize", parameter_SetCirclePosition2, SendMessageOptions.DontRequireReceiver);
		}
		
	}
	
	void SetTargetRadiusCombination ()
	{
		float deltaTargetRadius = minRadiusOfTargetCircle;
		
		for (int i = 0; i<numberOftrack; i++) {
			if (i == 0) {
				RadiusTargetCombination.Add (deltaTargetRadius);	
			} else {
				deltaTargetRadius = deltaTargetRadius + (maxRadiusOfTargetCircle - minRadiusOfTargetCircle) / (numberOftrack - 1);
				RadiusTargetCombination.Add (deltaTargetRadius);
			}	
		}
	}
	
	void SetTrackRadiusCombination ()
	{
		float deltaTrackRadius = minRadiusOfTrack;
		
		for (int i = 0; i<numberOftrack; i++) {
			if (i == 0) {
				RadiusTrackCombination.Add (deltaTrackRadius);	
			} else {
				deltaTrackRadius = deltaTrackRadius + (maxRadiusOfTrack - minRadiusOfTrack) / (numberOftrack - 1);
				RadiusTrackCombination.Add (deltaTrackRadius);
			}	
		}
		//foreach (float radius in RadiusTrackCombination){
		//	Debug.Log(radius);
		//}
	}
	
	void LoadNextTrail ()
	{
		currentTrail++;
		if (CoordinateAllCircle.Count > 0) {
			CoordinateAllCircle.Clear ();
		}
		int numTarget = rand.Next (0, RadiusTargetCombination.Count);
		int numTrack = rand.Next (0, RadiusTrackCombination.Count);
		currentTargetRadius = RadiusTargetCombination [numTarget];
		currentTrackRadius = RadiusTrackCombination [numTrack];
	
		for (int i = 0; i < numberOfTargetCircle; i++) {
			Vector2 tmp_coordinate;
			tmp_coordinate.x = Screen.width / 2f + Convert.ToSingle (currentTrackRadius * Math.Sin (Convert.ToSingle (Math.PI * Angle * i / 180f)));
			tmp_coordinate.y = Screen.height / 2f - Convert.ToSingle (currentTrackRadius * Math.Cos (Convert.ToSingle (Math.PI * Angle * i / 180f)));
			CoordinateAllCircle.Add (tmp_coordinate);
		}
		
		if (RadiusTargetCombination.Count > 1) {
			RadiusTargetCombination.RemoveAt (numTarget);
			RadiusTrackCombination.RemoveAt (numTrack);
		} else {
			isLastTrail = true;
		}
		
		
	}
	
	void LoadNextObjectAndTarget ()
	{
		if (PreviousRound == (numberOfTargetCircle - 1) && !isLastTrail) {
			LoadNextTrail ();
			round = 0;
		}else if (PreviousRound == (numberOfTargetCircle - 1) && isLastTrail){
			Destroy (objectCircle);
			Destroy (objectCircle2);
			SendMessage("ExperimentFinished",SendMessageOptions.DontRequireReceiver);
		}
		
		currentObjectCoordinate = CoordinateAllCircle [(round * (numberOfTargetCircle / 2 - 1)) % numberOfTargetCircle];
		
		currentTargetCoordinate = CoordinateAllCircle [((round + 1) * (numberOfTargetCircle / 2 - 1)) % numberOfTargetCircle];
		PreviousRound = round;
		round++;
		//if (((round + 1) * (numberOfTargetCircle / 2 - 1)) % numberOfTargetCircle == 0) {
		//	LoadNextTrail ();
			
		//	round = 0;
		//}
	}
	
	IEnumerator ExperimentFinished()
	{
		status = "Experiment finished. Load UI in 3 seconds!";

		yield return new WaitForSeconds(3f);

		
		Application.LoadLevel("UserInterface");
		
	}
	
	/*
	void AddRadiusCombination( RadiusCombination rc)
	{
		lock (radiusCombinationList) {
			radiusCombinationList.Add (rc);
		}
	}
	
	void RemoveRadiusCombination(int indx)
	{
		radiusCombinationList.RemoveAt (indx);
	}
	
	void LoadTrial()
	{		
		if(radiusCombinationList.Count > 0)
		{
			trialCount +=1;
			GameObject contextFree = Instantiate(Resources.Load("ContextFreeCircle")) as GameObject;
			contextFree.SendMessage("SetNumberOfTargetLocation", numberOfTargetLocation, SendMessageOptions.DontRequireReceiver);
			
			int randIndex = UnityEngine.Random.Range(0,radiusCombinationList.Count);
			
			rc = radiusCombinationList[randIndex];
			radiusCombinationList.RemoveAt(randIndex);
			
			contextFree.SendMessage("LoadCircleAndTargetObject", rc, SendMessageOptions.DontRequireReceiver);
			Log.log("PathCircleRadius = "+rc.get_circleRadius().ToString()+ " TargetCircleRadius = "+rc.get_targetRadius().ToString());
		}
		else 
		{
			Debug.Log("Trials Complete");
			Log.log("ContextFreeExperimentComplete.");
			Application.LoadLevel("UserStudyIntermediateBreak"); //jump to controller scene
		}
	}
	
	void DestroyPreviousCircle()
	{
		if(go_Target!=null)
		{
			Destroy(go_Target);
			go_Target = null;
		}
		if(go_Object!=null)
		{
			Destroy(go_Object);
			go_Object = null;
		}
	}
	
	
	
	IEnumerator NextTrial()
	{
		SendMessage("DestroyPreviousCircle", SendMessageOptions.DontRequireReceiver);
		
		yield return new WaitForSeconds(0.5f);
		SendMessage("LoadTrial", SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds(0.3f);
		startTargetCircleNumber = 0;
		SendMessage("FoodIntitPosition",startTargetCircleNumber, SendMessageOptions.DontRequireReceiver);
	}
	

	
	void FoodItemUnGrabbed()
	{
		startTargetCircleNumber +=1;
		
		if(startTargetCircleNumber == numberOfTargetLocation)
		{
			LogDetails(startTargetCircleNumber-1, 0); //log details 
			SendMessage("CallTrialCircle", SendMessageOptions.DontRequireReceiver); 
			
		}
		else
		{
			LogDetails(startTargetCircleNumber-1, startTargetCircleNumber); //log details 
		
			//then start from the last location
			SendMessage("FoodIntitPosition",startTargetCircleNumber, SendMessageOptions.DontRequireReceiver); 
		}	
		Resources.UnloadUnusedAssets();
	}
	
	void FoodItemGrabbed()
	{
		grabMoveStartTime = System.DateTime.Now;
		Log.log("FoodObjectGrabbed");
	}
	
	
/*	
	// Update is called once per frame
	
	private float targetRadiusMin = 0.4f, targetRadiusMax = 0.55f, circleRadiusMin = 2f, circleRadiusMax = 2.7f;
	private int numberOfTargetRadius = 3, numberOfCircleRadius = 3, numberOfTargetLocation = 5;
	
	private string startTargetName= string.Empty, foodObjectName = "Orange";
	private GameObject foodObject, PointerObjects;
	private int startTargetCircleNumber = 0, trialCount = 0;
	
	private System.DateTime grabMoveStartTime;
	
	
	
	
	
	void ColorTarger(int startTarget)
	{
		GameObject gStart = GameObject.Find(startTarget.ToString());
		GameObject gEnd = GameObject.Find(((startTarget + 1)%numberOfTargetLocation).ToString());
		if((startTarget-1)>-1)
		{
			GameObject.Find((startTarget-1).ToString()).renderer.material = imageMaterials[0];//Resources.Load("Number/Materials/TransCircle") as Material;
		}
		if(gStart)
			//gStart.renderer.material = Resources.Load("Number/Materials/"+startTarget.ToString()) as Material;
			gStart.renderer.material = imageMaterials[0];//Resources.Load("Number/Materials/TransCircle") as Material;
		
		if(gEnd)
			//gEnd.renderer.material = Resources.Load("Number/Materials/Target") as Material;
			gEnd.renderer.material = imageMaterials[1];//Resources.Load("Number/Materials/TargetGreen") as Material;
			
	}
	IEnumerator FoodIntitPosition(int start)
	{
		//GameObject gf = GameObject.Find(foodObjectName);
		GameObject gt = GameObject.Find(start.ToString());
		//now skip grab ungrab detection for a while
		PointerObjects.BroadcastMessage("skipGrab_UnGrabDetection", true, SendMessageOptions.DontRequireReceiver);
		
		yield return new WaitForSeconds(0.2f);
		foodObject.renderer.enabled = false;
		yield return new WaitForSeconds(0.3f);
		foodObject.renderer.enabled = true;	
		foodObject.transform.position = gt.transform.position;
		ColorTarger(start);
		//allow grab ungrab detection from now
		PointerObjects.BroadcastMessage("skipGrab_UnGrabDetection", false, SendMessageOptions.DontRequireReceiver);	
	}
	
	
	void LogDetails(int startNumber, int targetNumber)
	{
		GameObject startObj = GameObject.Find(startNumber.ToString()), targetObj = GameObject.Find(targetNumber.ToString());
		if(startObj == null || targetObj == null) return;
		
		System.DateTime t = System.DateTime.Now;
		System.TimeSpan duration = t-grabMoveStartTime;
		Log.log("FoodObjectUnGrabbed, trial = "+trialCount.ToString()+ " move = "+ targetNumber.ToString());
		Log.log("StartCircle = "+ (targetNumber-1).ToString() + " TargetCircle "+targetNumber.ToString());
		
		//Debug.Log("Dur "+ duration.TotalSeconds);
		Log.log("MovementTime = "+ duration.TotalSeconds+" Sec.");
		//now log the world and screen coordinates
		
		Log.log3Dpoint("StartCircleWorldPoint = ", startObj.transform.position);
		Log.log3Dpoint("StartCircleScreenPoint = ", Camera.main.WorldToScreenPoint(startObj.transform.position));
		Log.log3Dpoint("TargetCircleWorldPoint = ", targetObj.transform.position);
		Log.log3Dpoint("TargetCircleScreenPoint = ", Camera.main.WorldToScreenPoint(targetObj.transform.position));
		Log.log3Dpoint("FoodObjectUngrabbedWorldPoint = ", foodObject.transform.position);
		Log.log3Dpoint("FoodObjectUngrabbedScreenPoint = ", Camera.main.WorldToScreenPoint(foodObject.transform.position));
		
		
		float start2targetCircleDistance = Vector3.Distance(targetObj.transform.position, startObj.transform.position);
		Log.log("StartCircleToTargetCircleWorldDistance = "+ start2targetCircleDistance.ToString());
		float start2targetCircleScreenDistance = Vector3.Distance(Camera.main.WorldToScreenPoint(targetObj.transform.position),Camera.main.WorldToScreenPoint(startObj.transform.position));
		Log.log("StartCircleToTargetCircleScreenDistance = "+ start2targetCircleScreenDistance.ToString());
		
		
		float start2objectDistance = Vector3.Distance(foodObject.transform.position, startObj.transform.position);
		Log.log("StartCircleToFoodObjectWorldDistance = "+ start2objectDistance.ToString());
		float start2objectScreenDistance = Vector3.Distance(Camera.main.WorldToScreenPoint(foodObject.transform.position),Camera.main.WorldToScreenPoint(startObj.transform.position));
		Log.log("StartCircleToFoodObjectScreenDistance = "+ start2objectScreenDistance.ToString());
		
		float object2targetDistance = Vector3.Distance(foodObject.transform.position, targetObj.transform.position);
		Log.log("TargetToFoodObjectWorldDistance = "+ object2targetDistance.ToString());
		float object2targetScreenDistance = Vector3.Distance(Camera.main.WorldToScreenPoint(foodObject.transform.position),Camera.main.WorldToScreenPoint(targetObj.transform.position));
		Log.log("TargetToFoodObjectScreenDistance = "+ object2targetScreenDistance.ToString());
		
		
		if((object2targetDistance + (foodObject.collider.bounds.size.x/2)) <= rc.get_targetRadius())
			Log.log("FoodObjectPlacedInsideTheTarget");
		else if((object2targetDistance - (foodObject.collider.bounds.size.x/2)) > rc.get_targetRadius())
			Log.log("FoodObjectPlacedOutsideTheTarget");
		else if(object2targetDistance <= rc.get_targetRadius())
			Log.log("FoodObjectPlaced >= 50% InsideTheTarget");
		else 
			Log.log("FoodObjectPlacedOnTheTargetBoundary.");		
	}
	
	void LogOnlyPixels() //extra for documentation and paper writing for UIST
	{
		Vector3 startCircle  = GameObject.Find("0").transform.position;
		Vector3 targetCircle = GameObject.Find("1").transform.position;
		Vector3 tmp = startCircle;
		
		float pathCircleRadiusInPixel = Vector3.Distance(Camera.main.WorldToScreenPoint(Vector3.zero),Camera.main.WorldToScreenPoint(startCircle));
		Log.log("pathCircleRadiusInPixel = "+ pathCircleRadiusInPixel.ToString());
		tmp.x += (GameObject.Find("0").transform.localScale.x/2f);
		float targetCircleRadiusInPixel = Vector3.Distance(Camera.main.WorldToScreenPoint(startCircle),Camera.main.WorldToScreenPoint(tmp));
		Log.log("targetCircleRadiusInPixel = "+ targetCircleRadiusInPixel.ToString());
		
		float start2targetCircleScreenDistance = Vector3.Distance(Camera.main.WorldToScreenPoint(startCircle),Camera.main.WorldToScreenPoint(targetCircle));
		Log.log("StartCircleToTargetCircleScreenDistance = "+ start2targetCircleScreenDistance.ToString());
		
	}
	void LogCirclesInPixel()
	{
		
		LogOnlyPixels();
		SendMessage("CallTrialCircle", SendMessageOptions.DontRequireReceiver);
	}
	// Update is called once per frame
	void Update () 
	{
		/*
		if(Input.GetKeyUp(KeyCode.U))
			SendMessage("FoodItemUnGrabbed", SendMessageOptions.DontRequireReceiver);
				
		if(Input.GetKeyUp(KeyCode.G))
			SendMessage("FoodItemGrabbed", SendMessageOptions.DontRequireReceiver);
		if(Input.GetKeyUp(KeyCode.B)) Application.LoadLevel("UserStudyIntermediateBreak");
		if(Input.GetKeyUp(KeyCode.L)) LogCirclesInPixel();
	
	}*/	

}
