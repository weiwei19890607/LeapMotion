using UnityEngine;
using System.Collections;

public class SettingDparameter : MonoBehaviour {

	private float centerX, centerY, boxSide;
	public Texture2D letterTex, canvasTex;
	// Use this for initialization
	void Start () 
	{
		boxSide = 300f;
		centerX = UnityEngine.Screen.width/2f;
		centerY = UnityEngine.Screen.height/2f;
		PointingParameters.Dmax = 100f * PointingParameters.Xpixels;
	}
	
	// Update is called once per frame
	void Update() 
	{
		PointingParameters.Dmin += Input.GetAxis("Horizontal") * UnityEngine.Time.deltaTime *2.5f;
		if(PointingParameters.Dmin < 0f)
			PointingParameters.Dmin = 0f;
		if(Input.GetKeyUp(KeyCode.Space))
			Application.LoadLevel(Application.loadedLevel+1);
	}
	
	void OnGUI () {
		GUI.DrawTexture(new Rect(centerX-(boxSide/2f), centerY-(boxSide/2f), boxSide,boxSide),canvasTex);
		GUI.DrawTexture(new Rect(centerX-(PointingParameters.Dmin/2f), centerY-(PointingParameters.Dmin/2f), PointingParameters.Dmin,PointingParameters.Dmin),letterTex);
		GUI.Label(new Rect(10,10,100,100),(PointingParameters.Dmin).ToString());
		GUI.Label(new Rect(10,30,100,100),(PointingParameters.Dmax).ToString());
	}
}
