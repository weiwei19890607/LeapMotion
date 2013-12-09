using UnityEngine;
using System.Collections;

public class ShowCursor : MonoBehaviour {

	public bool use3Dcursor = false, use2DmousePointer = false, use2DhandCursor, useMotorPoint = false;
	public Texture2D cursorTexture;
	
	private Vector2 cursorPoint;
	
	private float cameraFixedZ = 0.06f, cursorScale = 20f;
	private GameObject _3DhandCursor;
	
	// Use this for initialization
	IEnumerator Start () 
	{
		cursorPoint = Vector2.zero;
		yield return new WaitForSeconds(0.5f);
		if(PointingParameters.Xpixels > 1f)
			cursorScale *= PointingParameters.Xpixels;
		
		if(use3Dcursor)
		{
			_3DhandCursor = Instantiate(Resources.Load("3DhandCursor") as GameObject) as GameObject;
			_3DhandCursor.transform.parent = this.transform;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		//check which point should use to display cursor
		if(useMotorPoint)
			cursorPoint = PointingParameters.MotorPoint;
		else
			cursorPoint = PointingParameters.DispPoint;
		//check how and which cursor should show
		if(use2DmousePointer)
			LeapAsMouse.SetCursorPos((int)cursorPoint.x, UnityEngine.Screen.height -(int)cursorPoint.y);
		if(use3Dcursor)
			HandCursorPosition();
	} 
	
	void OnDestroy()
	{
		Resources.UnloadAsset(cursorTexture);
	}
	
	void OnGUI()
	{
		if(use2DhandCursor)
		{
			GUI.depth = 0;
			GUI.DrawTexture(new Rect(cursorPoint.x, UnityEngine.Screen.height - cursorPoint.y, cursorScale, cursorScale), cursorTexture);
		}
	}
	
	void HandCursorPosition()
    {
		Vector3 coordinate = new Vector3( cursorPoint.x, cursorPoint.y, -(UnityEngine.Camera.mainCamera.transform.position.z + cameraFixedZ));
		
		coordinate = UnityEngine.Camera.mainCamera.ScreenToWorldPoint(coordinate);
		transform.position = coordinate;
		
		//SendMessage("MotorPointUpdated", SendMessageOptions.DontRequireReceiver);	
    }
}
