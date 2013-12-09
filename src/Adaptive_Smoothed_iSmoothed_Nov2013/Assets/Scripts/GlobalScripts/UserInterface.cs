/*
 *Jin Jiawei 
 *
 *University of Helsinki
*/
using UnityEngine;
using System.Collections;
using System;
using System.Xml;

public class UserInterface : MonoBehaviour
{
	public static Texture TextureCircleBlue;
	public static Texture TextureCircleGreen;
	public static Texture TextureCirclePink;
	public static Texture TextureCircleRed;
	public static Texture TextureCircleYellow;
	private int selGridIntMovableCircle = 0;
	private int selGridIntTargetCircle = 0;
	private int selGridIntPointing = 0;
	private int selGridIntDevice = 0;
	private int selGridIntExperiment = 0;
	private string[] selTexture = new string[] {"Blue", "Green", "Pink", "Red", "Yellow"};
	private string[] selPointing = new string[] {"Adaptive Pointing","Smooth Pointing", "Our new Pointing"};
	private string[] selDevice = new string[] {"LeapMotion","Kinect","Tobii X120","Mirametrix"};
	private string[] selExperiment = new string[] {"Fitts's Law","To be added"};
	private string CircleColor;
	private string FilePath;
	private string FileName;
	private string buttonSelectionDevice;
	private string buttonSelectionMiddle = "Click me";
	private string buttonSelectionMethod;
	private bool optSelection = true;
	private bool optSelectionDevice, optSelectionMethod;
	public string NumberOfTrack, NumberOfTargetCircle, MinRadiusOfTrack, MaxRadiusOfTrack, MinRadiusOfTargetCircle, MaxRadiusOfTargetCircle, ObjectRadius;

	void Start ()
	{	//Load saved file
		if (System.IO.File.Exists ("UI_Parameter.xml")) {
			XmlReader reader = XmlReader.Create ("UI_Parameter.xml");
			while (reader.Read()) {
				if (reader.Name == "UI_Parameter_Settings") {
					while (reader.NodeType != XmlNodeType.EndElement) {
						reader.Read ();
						if (reader.Name == "NumberOfTrack") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									NumberOfTrack = reader.Value;
								}
							}
							reader.Read ();
						} //end if

						if (reader.Name == "NumberOfTargetCircle") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									NumberOfTargetCircle = reader.Value;
								}
							}
							reader.Read ();
						} //end if
						if (reader.Name == "MinRadiusOfTrack") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									MinRadiusOfTrack = reader.Value;
								}
							}
							reader.Read ();
						} //end if
						if (reader.Name == "MaxRadiusOfTrack") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									MaxRadiusOfTrack = reader.Value;
								}
							}
							reader.Read ();
						} //end if
						if (reader.Name == "MinRadiusOfTargetCircle") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									MinRadiusOfTargetCircle = reader.Value;
								}
							}
							reader.Read ();
						} //end if
						if (reader.Name == "MaxRadiusOfTargetCircle") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									MaxRadiusOfTargetCircle = reader.Value;
								}
							}
							reader.Read ();
						} //end if
						if (reader.Name == "ObjectRadius") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									ObjectRadius = reader.Value;
								}
							}
							reader.Read ();
						} //end if
						if (reader.Name == "selGridIntMovableCircle") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									selGridIntMovableCircle = Convert.ToInt32 (reader.Value);
								}
							}
							reader.Read ();
						} //end if
						if (reader.Name == "selGridIntTargetCircle") {
							while (reader.NodeType != XmlNodeType.EndElement) {
								reader.Read ();
								if (reader.NodeType == XmlNodeType.Text) {
									selGridIntTargetCircle = Convert.ToInt32 (reader.Value);
								}
							}

						} //end if
						
					}
				} //end if
			}//end while
		}
		//
		PointingParameters.NumberOfTrack = Convert.ToInt32 (NumberOfTrack);
		PointingParameters.NumberOfTargetCircle = Convert.ToInt32 (NumberOfTargetCircle);
		PointingParameters.MinRadiusOfTrack = Convert.ToSingle (MinRadiusOfTrack);
		PointingParameters.MaxRadiusOfTrack = Convert.ToSingle (MaxRadiusOfTrack);
		PointingParameters.MinRadiusOfTargetCircle = Convert.ToSingle (MinRadiusOfTargetCircle);
		PointingParameters.MaxRadiusOfTargetCircle = Convert.ToSingle (MaxRadiusOfTargetCircle);
		PointingParameters.ObjectRadius = Convert.ToSingle (ObjectRadius);
	}
	
	//Draw user interface
	void OnGUI ()
	{	
		GUI.backgroundColor = Color.cyan;
		//Part 1
		//Calibration
		GUI.Box (new Rect (10, 40, 220, 160), "Calibration:");
		if (GUI.Button (new Rect (20, 80, 200, 50), "Calibration(Leap Motion)")) {
			//load calibration scene for Leap Motion
			
			Application.LoadLevel ("LeapMotion_Calibration");
		}
		if (GUI.Button (new Rect (20, 135, 200, 50), "Calibration(Kinect)")) {
			//load dwelling scene for Leap Motion
			Application.LoadLevel ("Kinect_PointingParameters");
		}
	
		
		//Part 2
		GUI.Box (new Rect (10, 210, 220, 230), "Manually Parameter Setting:");
		GUI.Label (new Rect (20, 250, 220, 50), "D min:");
		PointingParameters.Dmin = Convert.ToSingle (GUI.TextField (new Rect (100, 250, 40, 20), PointingParameters.Dmin.ToString (), 4));
		GUI.Label (new Rect (20, 280, 220, 50), "D max:");
		PointingParameters.Dmax = Convert.ToSingle (GUI.TextField (new Rect (100, 280, 40, 20), PointingParameters.Dmax.ToString (), 4));
		GUI.Label (new Rect (20, 310, 220, 50), "V min:");
		PointingParameters.Vmin = Convert.ToSingle (GUI.TextField (new Rect (100, 310, 40, 20), PointingParameters.Vmin.ToString (), 4));
		GUI.Label (new Rect (20, 340, 220, 50), "V max:");
		PointingParameters.Vmax = Convert.ToSingle (GUI.TextField (new Rect (100, 340, 40, 20), PointingParameters.Vmax.ToString (), 4));
		GUI.Label (new Rect (20, 370, 220, 50), "G min:");
		PointingParameters.Gmin = Convert.ToSingle (GUI.TextField (new Rect (100, 370, 40, 20), PointingParameters.Gmin.ToString (), 4));
		GUI.Label (new Rect (20, 400, 220, 50), "G max:");
		PointingParameters.Gmax = Convert.ToSingle (GUI.TextField (new Rect (100, 400, 40, 20), PointingParameters.Gmax.ToString (), 4));
		
		//Part 3
		//dwelling
		GUI.Box (new Rect (280, 40, 440, 330), "Select object circle color:");	
		selGridIntMovableCircle = GUI.SelectionGrid (new Rect (290, 70, 400, 20), selGridIntMovableCircle, selTexture, 7, "Toggle");
		GUI.Label (new Rect (430, 100, 220, 50), "Select target circle color: ");
		selGridIntTargetCircle = GUI.SelectionGrid (new Rect (290, 130, 400, 20), selGridIntTargetCircle, selTexture, 7, "Toggle");
		
		GUI.Label (new Rect (430, 160, 220, 50), "Define experiment:");
		GUI.Label (new Rect (290, 190, 220, 50), "Num of track:");
		NumberOfTrack = GUI.TextField (new Rect (420, 190, 40, 20), NumberOfTrack, 2);
		
		GUI.Label (new Rect (290, 220, 220, 50), "Max Radius of track:");
		MaxRadiusOfTrack = GUI.TextField (new Rect (420, 220, 40, 20), MaxRadiusOfTrack, 4);
		
		GUI.Label (new Rect (290, 250, 220, 50), "Min Radius of track:");
		MinRadiusOfTrack = GUI.TextField (new Rect (420, 250, 40, 20), MinRadiusOfTrack, 4);
		
		GUI.Label (new Rect (290, 280, 220, 50), "Radius of object:");
		ObjectRadius = GUI.TextField (new Rect (420, 280, 40, 20), ObjectRadius, 4);
		
		GUI.Label (new Rect (500, 190, 220, 50), "Num of target:");
		NumberOfTargetCircle = GUI.TextField (new Rect (630, 190, 40, 20), NumberOfTargetCircle, 2);
		
		GUI.Label (new Rect (500, 220, 220, 50), "Max Radius of target:");
		MaxRadiusOfTargetCircle = GUI.TextField (new Rect (630, 220, 40, 20), MaxRadiusOfTargetCircle, 4);
		
		GUI.Label (new Rect (500, 250, 220, 50), "Min Radius of target:");
		MinRadiusOfTargetCircle = GUI.TextField (new Rect (630, 250, 40, 20), MinRadiusOfTargetCircle, 4);
		
		if (GUI.Button (new Rect (290, 330, 70, 30), "Reset")) {
			if (System.IO.File.Exists ("UI_Parameter.xml")) {
				System.IO.File.Delete ("UI_Parameter.xml");
			}
			NumberOfTrack = "";
			NumberOfTargetCircle = "";
			MinRadiusOfTrack = "";
			MaxRadiusOfTrack = "";
			MinRadiusOfTargetCircle = "";
			MaxRadiusOfTargetCircle = "";
			ObjectRadius = "";
			selGridIntMovableCircle = 0;
			selGridIntTargetCircle = 0;
			
		}
		
		if (GUI.Button (new Rect (640, 330, 70, 30), "Save")) {
			/*FileName = @"/UI_Parameter.xml";
			FilePath = Application.dataPath + FileName;
			if (System.IO.File.Exists (FilePath)) {
				System.IO.File.Delete (FilePath);
			}
			System.IO.File.Create (FilePath);
			*/
			//set parameter
			PointingParameters.NumberOfTrack = Convert.ToInt32 (NumberOfTrack);
			PointingParameters.NumberOfTargetCircle = Convert.ToInt32 (NumberOfTargetCircle);
			PointingParameters.MinRadiusOfTrack = Convert.ToSingle (MinRadiusOfTrack);
			PointingParameters.MaxRadiusOfTrack = Convert.ToSingle (MaxRadiusOfTrack);
			PointingParameters.MinRadiusOfTargetCircle = Convert.ToSingle (MinRadiusOfTargetCircle);
			PointingParameters.MaxRadiusOfTargetCircle = Convert.ToSingle (MaxRadiusOfTargetCircle);
			PointingParameters.ObjectRadius = Convert.ToSingle (ObjectRadius);
			//Write setting to XML file
			XmlWriterSettings settings = new XmlWriterSettings ();
			settings.Indent = true;

			XmlWriter writer = XmlWriter.Create ("UI_Parameter.xml", settings);
			writer.WriteStartDocument ();
			writer.WriteComment ("This file is generated by the UI.");
			writer.WriteStartElement ("UI_Parameter_Settings");
			writer.WriteElementString ("NumberOfTrack", NumberOfTrack);
			writer.WriteElementString ("NumberOfTargetCircle", NumberOfTargetCircle);
			writer.WriteElementString ("MinRadiusOfTrack", MinRadiusOfTrack);
			writer.WriteElementString ("MaxRadiusOfTrack", MaxRadiusOfTrack);
			writer.WriteElementString ("MinRadiusOfTargetCircle", MinRadiusOfTargetCircle);
			writer.WriteElementString ("MaxRadiusOfTargetCircle", MaxRadiusOfTargetCircle);
			writer.WriteElementString ("ObjectRadius", ObjectRadius);
			writer.WriteElementString ("selGridIntMovableCircle", selGridIntMovableCircle.ToString ());
			writer.WriteElementString ("selGridIntTargetCircle", selGridIntTargetCircle.ToString ());
			writer.WriteEndElement ();
			writer.WriteEndDocument ();
			
			writer.Flush ();
			writer.Close ();
			
		}
		
		
		//Part 4
		//select movable circle color
		GUI.Box (new Rect (770, 40, 440, 330), "");
		
		//select target circle color
		//GUI.Label (new Rect (920, 100, 220, 50), "Select target circle color: ");
		
		//choose pointing device
		GUI.Label (new Rect (800, 40, 220, 50), "Choose pointing device: ");
		selGridIntDevice = GUI.SelectionGrid (new Rect (780, 70, 400, 90), selGridIntDevice, selDevice, 3);
		
		//choose pointing method
		GUI.Label (new Rect (800, 170, 220, 50), "Choose pointing method: ");
		selGridIntPointing = GUI.SelectionGrid (new Rect (780, 200, 400, 40), selGridIntPointing, selPointing, 3);
		
		//choose selection gesture
		GUI.Label (new Rect (800, 250, 220, 50), "Choose selection device and method: ");
		if (optSelection) {
			if (GUI.Button (new Rect (780, 280, 200, 40), buttonSelectionDevice + buttonSelectionMiddle + buttonSelectionMethod)) {
				buttonSelectionMiddle = "with ";
				optSelection = !optSelection;
				optSelectionDevice = !optSelectionDevice;
			}
		}
		if (optSelectionDevice) {
			if (GUI.Button (new Rect (780, 280, 133, 40), "Leap Motion")) {
				buttonSelectionDevice = "Leap Motion ";
				PointingParameters.UseSelectionDevice = "LeapMotion";
				optSelectionDevice = !optSelectionDevice;
				optSelectionMethod = !optSelectionMethod;
				//use leap as selection device
			}
			if (GUI.Button (new Rect (917, 280, 133, 40), "Kinect")) {
				buttonSelectionDevice = "Kinect ";
				PointingParameters.UseSelectionDevice = "Kinect";
				optSelectionDevice = !optSelectionDevice;
				optSelectionMethod = !optSelectionMethod;
				//use leap as selection device
			}
			if (GUI.Button (new Rect (1054, 280, 133, 40), "Glove")) {
				buttonSelectionDevice = "Glove ";
				PointingParameters.UseSelectionDevice = "Glove";
				optSelectionDevice = !optSelectionDevice;
				optSelectionMethod = !optSelectionMethod;
				//use glove as selection device
			}
			
		}
		if (optSelectionMethod) {
			if (PointingParameters.UseSelectionDevice == "LeapMotion") {
				//selection gesture for leap motion
				if (GUI.Button (new Rect (780, 280, 133, 40), "Trigger")) {
					PointingParameters.UseSelectionMethod = "Trigger";
					buttonSelectionMethod = "trigger";
					optSelectionMethod = !optSelectionMethod;
					optSelection = !optSelection;
				}
			} else if (PointingParameters.UseSelectionDevice == "Kinect") {
				//selection gesture for leap motion
				if (GUI.Button (new Rect (780, 280, 133, 40), "Grab")) {
					PointingParameters.UseSelectionMethod = "Grab";
					buttonSelectionMethod = "grab";
					optSelectionMethod = !optSelectionMethod;
					optSelection = !optSelection;
				}
				if (GUI.Button (new Rect (917, 280, 133, 40), "Pinch")) {
					PointingParameters.UseSelectionMethod = "Pinch";
					buttonSelectionMethod = "pinch";
					optSelectionMethod = !optSelectionMethod;
					optSelection = !optSelection;
				}
			} else if (PointingParameters.UseSelectionDevice == "Glove") {
				//selection gesture for leap motion
				if (GUI.Button (new Rect (780, 280, 133, 40), "Unknow")) {
					PointingParameters.UseSelectionMethod = "Unknow";//need to be modified soon
					buttonSelectionMethod = "unknow";
					optSelectionMethod = !optSelectionMethod;
					optSelection = !optSelection;
				}
		
			}
			
		}

		//part 5
		GUI.Box (new Rect (280, 400, 440, 100), "Select experiment task:");	
		selGridIntExperiment = GUI.SelectionGrid (new Rect (290, 430, 267, 40), selGridIntExperiment, selExperiment, 2);
		
		
		//start experiment
		if (GUI.Button (new Rect (640, 460, 70, 30), "Confirm")) {
			//switch for the color of movable circle
			switch (selGridIntMovableCircle) {
			case 0:
				PointingParameters.ObjectColor = "blue";
				//blue for moveable circle
				//do something
				break;
			case 1:
				PointingParameters.ObjectColor = "green";
				//green for moveable circle
				//do something
				break;
			case 2:
				PointingParameters.ObjectColor = "pink";
				//pink for moveable circle
				//do something
				break;
			case 3:
				PointingParameters.ObjectColor = "red";
				//red for moveable circle
				//do something
				break;
			case 4:
				PointingParameters.ObjectColor = "yellow";
				//yellow for moveable circle
				//do something
				break;
			}
			
			//switch for the color of target circle
			switch (selGridIntTargetCircle) {
			case 0:
				PointingParameters.TargetColor = "blue";
				//blue for target circle
				//do something
				break;
			case 1:
				PointingParameters.TargetColor = "green";
				//green for target circle
				//do something
				break;
			case 2:
				PointingParameters.TargetColor = "pink";
				//pink for target circle
				//do something
				break;
			case 3:
				PointingParameters.TargetColor = "red";
				//red for target circle
				//do something
				break;
			case 4:
				PointingParameters.TargetColor = "yellow";
				//yellow for target circle
				//do something
				break;
			}
			
			//switch for selecting pointing method
			switch (selGridIntPointing) {
			case 0:
				PointingParameters.UseModel = "Adaptive";
				//use adaptive pointing
				//do something
				break;
			case 1:
				PointingParameters.UseModel = "Smoothed";
				//use smooth pointing
				//do something
				break;
			case 2:
				PointingParameters.UseModel = "Our";
				//use our new pointing
				//do something
				break;
			
			}
			
			//switch for selecting pointing device to start experiment
			switch (selGridIntDevice) {
			case 0:
				PointingParameters.UsePointingDeivce = "LeapMotion";
				//use leap motion as pointing device
				break;
			case 1:
				PointingParameters.UsePointingDeivce = "Kinect";
				//use kinect as pointing device
				break;
			case 2:
				PointingParameters.UsePointingDeivce = "Tobii";
				break;
			case 3:
				PointingParameters.UsePointingDeivce = "Mirametrix";
				break;
			}
			//switch for selecting pointing device to start experiment
			switch (selGridIntExperiment) {
			case 0:
				PointingParameters.UseExperimentTask = "Fitts";
				//start the experiment!
				Application.LoadLevel ("ExperimentFitts");
				break;
			case 1:
				PointingParameters.UseExperimentTask = "Unknow";//need to be modified soon
				//use kinect as pointing device
				//start another experiment
				//do something here
				break;
			}
			
			
		}
		
	}

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.End)) {
			Application.Quit ();
		}
	}
}
