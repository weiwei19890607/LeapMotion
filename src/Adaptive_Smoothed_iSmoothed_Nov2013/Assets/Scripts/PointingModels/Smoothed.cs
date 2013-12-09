using UnityEngine;
using System.Collections;

public class Smoothed : MonoBehaviour {
	public Texture2D cursorTexture;
	private float Dmin, Dmax, Vmin, Vmax, Gmin, Gmax;
	
	//according to the paper "Adaptive Pointing- Implicit gain adaptation for absolute pointing devices"
	private Vector2 d, dCap, m, s, g, disp, disp_1, delta;
	private Vector2[] mot;
	private float v,vCap;
	private int maxIndex;
	
	// Use this for initialization
	void Start () 
	{
		PointingParameters.GetParameters();
		InitializeFixedParameters();
		maxIndex = (int)(0.1f / UnityEngine.Time.fixedDeltaTime);
		mot = new Vector2[maxIndex + 1];
	}
	
	void InitializeFixedParameters()
	{
		Dmin = PointingParameters.Dmin;
		Dmax = PointingParameters.Dmax;
		Vmin = PointingParameters.Vmin;
		Vmax = PointingParameters.Vmax;
		Gmin = PointingParameters.Gmin;
		Gmax = PointingParameters.Gmax;
	}
		
	void Update()
	{
		
	}
	
	void FixedUpdate () 
	{
		//get the current values
		disp_1 = disp;
		//update motor point
		UpdateMot();
		
		//update delta
		delta = mot[maxIndex] - mot[0];
		//update d
		d = mot[maxIndex] - disp_1; 
		//update s
		s = mot[maxIndex] - mot[maxIndex-1]; 
		//update v
		UpdateV();
		//update vCap
		UpdateVcap(); 
		//update dCap
		UpdateDcap(); 
		//update m
		UpdateM(); 
		//update g
		UpdateG(); 
		
		//update disp
		UpdateDisp(); 
		
		//set the updated values
		
		PointingParameters.DispPoint = disp;
			
	}
	
		
	void UpdateMot()
	{
		for(int i = 1; i <= maxIndex; i++)
			mot[i-1] = mot[i];
		mot[maxIndex] =  PointingParameters.MotorPoint;
	}
	
		
	void UpdateV()
	{
		v = Mathf.Sqrt((delta.x * delta.x ) + (delta.y * delta.y)) / (maxIndex * UnityEngine.Time.fixedDeltaTime);
	}
	
	void UpdateVcap()
	{
		vCap = (v - Vmin) / (Vmax-Vmin);
	}
	
	void UpdateDcap()
	{
		dCap.x = Mathf.Abs(d.x)/Dmax;
		dCap.y = Mathf.Abs(d.y)/Dmax;
	}
	
	void UpdateM()
	{
		if(dCap.x > vCap)
		{ 
			m.x = dCap.x;
			//Debug.Log("dcap x "+dCap.x + "vcap "+ vCap);
		}
		else 
		{
			m.x = vCap;
			//Debug.Log("v cap  "+vCap);
		}
		
		if(dCap.y > vCap)
		{
			m.y = dCap.y;
			//Debug.Log("dcap y "+dCap.y);
		}
		else m.y = vCap;
	}
	
	void UpdateG()
	{
		if(v < Vmax)
		{
			g.x = Gmin + 0.5f* (1-Gmin)*( Mathf.Sin((m.x * Mathf.PI) - (Mathf.PI * 0.5f)) + 1);
			g.y = Gmin + 0.5f* (1-Gmin)*( Mathf.Sin((m.y * Mathf.PI) - (Mathf.PI * 0.5f)) + 1);
		}
		else
		{
			g = EstimateGmax();
		}
		
	}
	
	Vector2 EstimateGmax()
	{
		Vector2 gMax = Vector2.zero;
		if(d.x * s.x > 0f)
		{
			gMax.x = (d.x/s.x) + (1f/vCap) * (1f - (d.x/s.x));
			//Debug.Log("smoothed dx/sx "+gMax.x);
		}
		else 
			gMax.x = 1f / (vCap * (1+dCap.x));
		
		if(d.y * s.y > 0f)
		{ 
			gMax.y = (d.y/s.y) + (1f/vCap) * (1f - (d.y/s.y));
			//Debug.Log("smoothed dy/sy "+gMax.y);
		}
		else 
			gMax.y = 1f / (vCap * (1+dCap.y));
		return gMax;
	}
	
	void UpdateDisp()
	{
		disp.x = disp_1.x + g.x * s.x;
		disp.y = disp_1.y + g.y * s.y;
		
	}
	void OnGUI()
	{
		//GUI.DrawTexture(new Rect(disp.x, UnityEngine.Screen.height - disp.y, 20f, 20f), cursorTexture);
	}
}
