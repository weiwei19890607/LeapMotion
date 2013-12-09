using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingOtherParameters : MonoBehaviour {
	
	public Texture2D letterTex, canvasTex;
	
	List<float> distances = new List<float> ();  
	List<float> velocities = new List<float> (); 
	
	private float centerX, centerY, boxSize, letterSize = 120f;
	private bool startDwell;
	private float dwellTime = 5f, tmpDwellTime;
	private string status;
	private Vector2 lastMotorPoint;
	private int startFlag = 1;
		
	// Use this for initialization
	void Start ()
	{
		
		boxSize = 300f;
		centerX = UnityEngine.Screen.width/2f;
		centerY = UnityEngine.Screen.height/2f;
		
		lastMotorPoint = Vector2.zero;
		status = "Try to point at the middle of the screen to start calibration, and then hold for a few seconds!";
		
		
	}
	
	void Update()
	{
		if (PointingParameters.MotorPoint.x < (UnityEngine.Screen.width/2 + 5) && PointingParameters.MotorPoint.x > (UnityEngine.Screen.width/2 - 5) && PointingParameters.MotorPoint.y < (UnityEngine.Screen.height/2 + 5) && PointingParameters.MotorPoint.y > (UnityEngine.Screen.height/2 -5) && startFlag == 1)
		{
			status = "Calibration started!";
			tmpDwellTime = dwellTime;
			startDwell = !startDwell;
			startFlag--;
		}	
	}
	
	
	
	
	
	void OnGUI () {
		GUI.Label(new Rect(UnityEngine.Screen.width/2 -100,UnityEngine.Screen.height/2 - 200,200,80),status);
		GUI.DrawTexture(new Rect(centerX-(boxSize/2f), centerY-(boxSize/2f), boxSize,boxSize),canvasTex);
		GUI.DrawTexture(new Rect(centerX-(letterSize/2f), centerY-(letterSize/2f), letterSize,letterSize),letterTex);
		//GUI.Label(new Rect(10,10,100,100),(PointingParameters.Xpixels).ToString());
	}
	
		
	void FixedUpdate()
	{
		Vector2 tmpVec = PointingParameters.MotorPoint;
		if(startDwell)
		{
			tmpDwellTime -= UnityEngine.Time.fixedDeltaTime;
			
			if(tmpDwellTime > 0f)
			{
				float dist = Vector2.Distance(lastMotorPoint, tmpVec);				
				float vel = dist / UnityEngine.Time.fixedDeltaTime;
				distances.Add(dist);
				velocities.Add(vel);
			}
			else
			{
				//set the parameters
				PointingParameters.D90 = Percentile(distances, 0.9f);
				PointingParameters.Vmin = Percentile(velocities, 0.9f);
				PointingParameters.Vmax = 5f * PointingParameters.Vmin;
				PointingParameters.Gmin = PointingParameters.Xpixels / PointingParameters.D90;
				PointingParameters.Gmax = 1.055f;
				status = "Calibration Done, return to UI in 3 seconds.";
				SendMessage("DwellingFinished",SendMessageOptions.DontRequireReceiver);
				distances.Clear();
				velocities.Clear();
				startDwell = false;	
				PointingParameters.SetParameters();
			}
		}
		lastMotorPoint = tmpVec;
	}
	
	//http://en.wikipedia.org/wiki/Percentile
	public float Percentile(List<float> sequence, float excelPercentile)
	{
		sequence.Sort();
	    
	    int N = sequence.Count;
	    float n = (N - 1) * excelPercentile + 1;
	    // Another method: double n = (N + 1) * excelPercentile;
	    if (n == 1d) return sequence[0];
	    else if (n == N) return sequence[N - 1];
	    else
	    {
	         int k = (int)n;
	         float d = n - k;
	         return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
	    }
	}
	IEnumerator DwellingFinished()
	{
		yield return new WaitForSeconds(3f);
		Application.LoadLevel("UserInterface");
		
	}
}
