/*
 * Imtiaj Ahmed
 * Computer Science
 * University of Helsinki
 * iahmed.cs.helsinki@gmail.com
 * phone: +358453538393
 * 15.01.2012
 * modified 23.07.2012
 * You are free to use it
 * Please dont remove the identification of the developer
 */

using UnityEngine;
using System.Collections;

public class LabelVisualizer : MonoBehaviour {
	
	public bool visible;
	string labelMsg;
	private GUIStyle style;
	public Font font;
	// Use this for initialization
	void Start () 
	{
		style = new GUIStyle();
	/*	style.wordWrap = true;*/
		style.normal.textColor = Color.blue; 
		style.font = font;
		style.fontSize = 25; 
	}
	
	void LabelMsgVisualize(string lblMsg)
	{
		labelMsg = lblMsg;
	}
	
	// Update is called once per frame
	void OnGUI(){
		if(visible)
			GUILayout.Label(labelMsg,style);
	}
}
