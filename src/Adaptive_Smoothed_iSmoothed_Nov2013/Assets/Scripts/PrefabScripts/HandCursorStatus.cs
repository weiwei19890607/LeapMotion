using UnityEngine;
using System.Collections;

public class HandCursorStatus : MonoBehaviour {
	
	public GameObject openHand, fist;
	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(1f);
		fist.renderer.enabled = false;
		openHand.renderer.enabled = true;
	}
	
	void Pinch_Gesture()
	{
		fist.renderer.enabled = true;
		openHand.renderer.enabled = false;
	}
	
	void UnPinch_Gesture()
	{
		fist.renderer.enabled = false;
		openHand.renderer.enabled = true;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
