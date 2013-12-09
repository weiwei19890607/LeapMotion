using UnityEngine;
using System.Collections;

public class Iadaptive : MonoBehaviour {

	private float Dmin, Dmax, Vmin, Vmax, Gmin, Gmax;
	
	//according to the paper "Adaptive Pointing- Implicit gain adaptation for absolute pointing devices"
	private Vector2 d, dCap, m, s, g, g_1, disp, disp_1;
	private float v,vCap;
	private Vector2[] mot;
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
		
	
	
	void FixedUpdate () 
	{
		//get the current values
		
		//update motor point
		UpdateMot();
		disp_1 = disp;
		g_1 = g;
		
		//update d
		d = mot[maxIndex] - disp_1; 
		//update s
		s = mot[maxIndex] - mot[maxIndex-1]; 
		//update v
		UpdateV();
		//update vCap
		UpdateVcap(); 
		//update g
		UpdateG(); 
		
		//FilterG();
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
		/*float sum_v = 0f;
		for(int i=1; i<=maxIndex; i++)
			sum_v += Vector2.Distance(mot[i-1], mot[i]);
		v = sum_v / (maxIndex * UnityEngine.Time.fixedDeltaTime);
		//v =  Vector2.Distance(mot, mot_1)/UnityEngine.Time.fixedDeltaTime;
	
		//float v1 = ( (Vector2.Distance(mot[maxIndex], mot[maxIndex-1]) / UnityEngine.Time.fixedDeltaTime)+v)/2f;
		*/ //v = (Vector2.Distance(mot[maxIndex], mot[maxIndex-1]) / UnityEngine.Time.fixedDeltaTime);
	
		//v = (Vector2.Distance(mot[maxIndex], mot[0])/(maxIndex * UnityEngine.Time.fixedDeltaTime));
		
		Debug.Log("speed "+v);
	}
	
	void UpdateVcap()
	{
		vCap = (v - Vmin) / (Vmax-Vmin);
	}
		
	void UpdateG()
	{
		if(v <= 0f)
			g = Vector2.zero;
		else if(v <= Vmin)
			g.x = g.y = (v * Gmin) / Vmin;
		if((v > Vmin) && (v<= Vmax))
			g.x = g.y = Gmin + 0.5f* (1-Gmin)*( Mathf.Sin((vCap * Mathf.PI) - (Mathf.PI * 0.5f)) + 1);
		else
		{
			g = EstimateGmax();
			//Debug.Log("g = "+g);
		}
		
	}
	
	Vector2 EstimateGmax()
	{
		Vector2 gMax = Vector2.zero;
		if(d.x * s.x > 0f)
		{
			gMax.x = (d.x/s.x);
			//if(gMax.x>1f)
			//	Debug.Log("dx/sx "+gMax.x);
		}
		else 
			gMax.x =  0.5f + (1f/(2f*vCap));
		
		if(d.y * s.y > 0f)
		{
			gMax.y = (d.y/s.y);
			//gMax.y = (d.y/s.y) + (1f/vCap) * (1f - (d.y/s.y));
			//if(gMax.y>1f)
			//	Debug.Log("dy/sy "+gMax.y);
		}
		else 
			gMax.y =  0.5f + (1f/(2f*vCap));
		
		return gMax;
	}
	
	void FilterG()
	{
		float alpha = 0.5f + 0.5f * (1/vCap);
		
		g.x = alpha * g.x + (1-alpha)* g_1.x;
		g.y = alpha * g.y + (1-alpha)* g_1.y;
	}
	
	void UpdateDisp()
	{
		disp.x = disp_1.x + g.x * s.x;
		disp.y = disp_1.y + g.y * s.y;
		//Debug.Log("disp = "+disp);
	}
}
