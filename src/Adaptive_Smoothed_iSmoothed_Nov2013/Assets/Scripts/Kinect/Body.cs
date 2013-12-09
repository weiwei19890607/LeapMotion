/*
 * Imtiaj Ahmed
 * Computer Science
 * University of Helsinki
 * iahmed.cs.helsinki@gmail.com
 * phone: +358453538393
 * 21.05.2012
 * modified 21.07.2012
 * You are free to use it
 * Please dont remove the identification of the developer
 */
using UnityEngine;
using System.Collections;
using FubiNET;


public class Body : MonoBehaviour {
	
	
	public static bool isInSession = true;
	public static Vector3 Spine, ShoulderCenter, ShoulderRight, HandRight, ElbowRight;
	public static float HandRightConfidence, ElbowRightConfidence;
	private bool skeletonUpdated = true;
	private uint closestId;
	
	void Start () 
	{
		
	}
	
		
	void FixedUpdate()
	{
		closestId = FubiNET.Fubi.getClosestUserID();
		GetSkeletonPositionInfo();
		BroadcastMessage("SkeletonUpdated", SendMessageOptions.DontRequireReceiver);	
		
	}
	
	float confidence;
	Vector3 SingleJointPosition(FubiUtils.SkeletonJoint openNIJointIndex) //for fubi wrapper
	{
		Vector3 tempJoint;
		double timeStamp;
			
		FubiNET.Fubi.getCurrentSkeletonJointPosition(closestId,openNIJointIndex,
		 out tempJoint.x,out tempJoint.y, out tempJoint.z,out confidence, out timeStamp, false);
		
		//if(confidence > 0.5f) // if confident then update joint
		{
			tempJoint.x = tempJoint.x * 0.001f;
			tempJoint.y = tempJoint.y * 0.001f;
			tempJoint.z = tempJoint.z * 0.001f;
		}
		
		return tempJoint;
	}
	void GetSkeletonPositionInfo()//for fubi wrapper
	{
		Spine = SingleJointPosition(FubiUtils.SkeletonJoint.TORSO);
		ShoulderCenter = SingleJointPosition(FubiUtils.SkeletonJoint.NECK);
		ShoulderRight = SingleJointPosition(FubiUtils.SkeletonJoint.RIGHT_SHOULDER);
		ElbowRight = SingleJointPosition(FubiUtils.SkeletonJoint.RIGHT_ELBOW);
		ElbowRightConfidence = confidence;
		HandRight = SingleJointPosition(FubiUtils.SkeletonJoint.RIGHT_HAND);
		HandRightConfidence = confidence;
	}
	
	
	void SessionManager_InSession()
	{
		isInSession = true;
	}
	
	void SessionManager_NotInSession()
	{
		isInSession = false;
	}
}
