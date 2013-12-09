using UnityEngine;
using System.Collections;

public class ReturnToUI : MonoBehaviour {

	
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
			Application.LoadLevel("UserInterface");
		
	}
}
