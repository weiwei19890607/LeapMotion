using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour {
	
	public Texture2D icon;

	void OnGUI () {
		GUI.DrawTexture (new Rect (0,0, 50, 50), icon);
			
	}
	
}
