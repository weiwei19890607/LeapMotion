using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	private float Dmin, Dmax, Vmin, Vmax, Gmin, Gmax;
	
	//according to the paper "Adaptive Pointing- Implicit gain adaptation for absolute pointing devices"
	private Vector2 d, dCap, m, s, g, disp, disp_1, delta;
	private Vector2[] mot;
	private float v,vCap, dVcap;
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
		if(Input.GetKeyUp(KeyCode.Space))
			Application.LoadLevel(Application.loadedLevel+1);
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
		//v = Mathf.Sqrt((delta.x * delta.x ) + (delta.y * delta.y)) / (maxIndex * UnityEngine.Time.fixedDeltaTime);
		//v = (Vector2.Distance(mot[maxIndex], mot[maxIndex-1])+ Vector2.Distance(mot[maxIndex-1],mot[maxIndex-2]))/(2f*UnityEngine.Time.fixedDeltaTime);
		v = Vector2.Distance(mot[maxIndex], mot[maxIndex-1])/UnityEngine.Time.fixedDeltaTime;
	
	}
	
	void UpdateVcap()
	{
		float vC = (v - Vmin) / (Vmax-Vmin);
		//if(v>(Vmin*1.3f))
			vCap = 0.8f*vC + 0.2f*vCap;
		//else 	
			vCap = vC;
	}
	
	void UpdateDcap()
	{
		dCap.x = Mathf.Abs(d.x)/Dmax;
		dCap.y = Mathf.Abs(d.y)/Dmax;
	}
	
	void UpdateM()
	{
		/*
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
		*/
		dVcap = Vector2.Distance(mot[maxIndex], disp_1)/Dmax;
		if(dVcap>vCap)
			m.x = m.y = dVcap;
		else {
			m.x = m.y = vCap;
		}
		
		
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
		float dt,st;
		dt = Vector2.Distance(mot[maxIndex], disp_1);
		st = Vector2.Distance(mot[maxIndex], mot[maxIndex-1]);
	/*	if(st >0f)
			gMax.x = gMax.y= (dt/st);
		else 
			gMax.x = 1f / (vCap * (1+dVcap));
	*/	//Debug.Log("dt/st "+ (dt/st) + "st "+ st);
		if(d.x * s.x > 0f)
		{
			gMax.x = (dt/st) + (1f/vCap) * (1f - (dt/st));
			Debug.Log("smoothed dx/sx "+gMax.x);
		}
		else 
			gMax.x = 1f / (1f+vCap);
		
		if(d.y * s.y > 0f)
		{ 
			gMax.y = (dt/st) + (1f/vCap) * (1f - (dt/st));
			//Debug.Log("smoothed dy/sy "+gMax.y);
		}
		else 
			gMax.y = 1f / (1f+vCap);
		/*
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
			*/
		return gMax;
	}
	
	void UpdateDisp()
	{
		disp.x = disp_1.x + g.x * s.x;
		disp.y = disp_1.y + g.y * s.y;
		
	}
	
}



/*
 * v = (Vector2.Distance(mot[maxIndex], mot[maxIndex-1])+ Vector2.Distance(mot[maxIndex-1],mot[maxIndex-2]))/(2f*UnityEngine.Time.fixedDeltaTime);
		float del = Vector2.Distance(mot[maxIndex], disp_1) / UnityEngine.Time.fixedDeltaTime;
		
 */