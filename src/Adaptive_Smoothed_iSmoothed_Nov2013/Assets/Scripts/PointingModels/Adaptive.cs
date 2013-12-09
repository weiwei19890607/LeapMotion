using UnityEngine;
using System.Collections;

public class Adaptive : MonoBehaviour {
	
	
	private float Dmin, Dmax, Vmin, Vmax, Gmin, Gmax;
	
	//according to the paper "Adaptive Pointing- Implicit gain adaptation for absolute pointing devices"
	private Vector2 v,vCap, d, dCap, m, s, mot, mot_1, g, gCap, disp, disp_1;
	
	// Use this for initialization
	void Start () 
	{
		PointingParameters.GetParameters();
		InitializeFixedParameters();
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
		mot_1 = mot;
		mot = PointingParameters.MotorPoint;
		disp_1 = disp;
		
		//update s
		s = mot - mot_1; //eq-6
		//update v
		UpdateV();
		//update vCap
		UpdateVcap(); // eq-1
		//update d
		d = mot - disp_1; // eq-2
		//update dCap
		UpdateDcap(); //eq-3
		//update m
		UpdateM(); //eq-4
		//update g
		UpdateG(); //eq-5
		//update gCap
		UpdateGcap(); // eq-7
		//update disp
		UpdateDisp(); // eq-8
		
		//set the updated values
		
		PointingParameters.DispPoint = disp;
			
	}
	
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Space))
			Application.LoadLevel(Application.loadedLevel+1);
	}
		
	void UpdateV()
	{
		v.x = Mathf.Abs(s.x)/UnityEngine.Time.fixedDeltaTime;
		v.y = Mathf.Abs(s.y)/UnityEngine.Time.fixedDeltaTime;
	}
	
	void UpdateVcap()
	{
		if(v.x > Vmax)
			vCap.x = 1f;
		else if(v.x < Vmin)
			vCap.x = 0f;
		else
			vCap.x = (v.x - Vmin)/(Vmax-Vmin);
		
		if(v.y > Vmax)
			vCap.y = 1f;
		else if(v.y < Vmin)
			vCap.y = 0f;
		else
			vCap.y = (v.y - Vmin)/(Vmax-Vmin);
	}
	
	void UpdateDcap()
	{
		if(Mathf.Abs(d.x) > Dmax)
			dCap.x = 1f;
		else if(Mathf.Abs(d.x) < Dmin)
			dCap.x = 0f;
		else
			vCap.x = (Mathf.Abs(d.x) - Dmin)/(Dmax-Dmin);
		
		if(Mathf.Abs(d.y) > Dmax)
			dCap.y = 1f;
		else if(Mathf.Abs(d.y) < Dmin)
			dCap.y = 0f;
		else
			vCap.y = (Mathf.Abs(d.y) - Dmin)/(Dmax-Dmin);
	}
	
	void UpdateM()
	{
		if(dCap.x > vCap.x)
			m.x = dCap.x;
		else m.x = vCap.x;
		
		if(dCap.y > vCap.y)
			m.y = dCap.y;
		else m.y = vCap.y;
	}
	
	void UpdateG()
	{
		g.x = Gmin + 0.5f*( Mathf.Sin((m.x * Mathf.PI) - (Mathf.PI * 0.5f)) + 1) * (Gmax-Gmin);
		g.y = Gmin + 0.5f*( Mathf.Sin((m.y * Mathf.PI) - (Mathf.PI * 0.5f)) + 1) * (Gmax-Gmin);
	}
	
	void UpdateGcap()
	{
		if(g.x > 1f && d.x > 0f && s.x < 0f)
			gCap.x = 1f - (g.x - 1f);
		else if(g.x > 1f && d.x < 0f && s.x > 0f)
			gCap.x = 1f - (g.x - 1f);
		else gCap.x = g.x;
		
		if(g.y > 1f && d.y > 0f && s.y < 0f)
			gCap.y = 1f - (g.y - 1f);
		else if(g.y > 1f && d.y < 0f && s.y > 0f)
			gCap.y = 1f - (g.y - 1f);
		else gCap.y = g.y;
	}
	
	void UpdateDisp()
	{
		disp.x = disp_1.x + gCap.x * s.x;
		disp.y = disp_1.y + gCap.y * s.y;
	}
}
