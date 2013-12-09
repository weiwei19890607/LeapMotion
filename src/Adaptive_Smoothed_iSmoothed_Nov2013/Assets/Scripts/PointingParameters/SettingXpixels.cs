using UnityEngine;
using System.Collections;

public class SettingXpixels : MonoBehaviour {
	
	private float centerX, centerY, boxSide;
	public Texture2D letterTex, canvasTex;
	// Use this for initialization
	void Start () 
	{
		boxSide = 300f;
		centerX = UnityEngine.Screen.width/2f;
		centerY = UnityEngine.Screen.height/2f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		PointingParameters.Xpixels += Input.GetAxis("Horizontal") * UnityEngine.Time.deltaTime;
		if(PointingParameters.Xpixels < 0f)
			PointingParameters.Xpixels = 0f;
		
	}
	
	void OnGUI () {
		GUI.DrawTexture(new Rect(centerX-(boxSide/2f), centerY-(boxSide/2f), boxSide,boxSide),canvasTex);
		GUI.DrawTexture(new Rect(centerX-(PointingParameters.Xpixels/2f), centerY-(PointingParameters.Xpixels/2f), PointingParameters.Xpixels,PointingParameters.Xpixels),letterTex);
		GUI.Label(new Rect(10,10,100,100),(PointingParameters.Xpixels).ToString());
	}
}
