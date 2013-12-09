using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Log: MonoBehaviour {
/*
	public static string fileName = String.Empty;
	public static string questionFileName = string.Empty;
	
	private static int qCount = 0;
	// Use this for initialization
	public static void Write(string str)
	{
		if(fileName==string.Empty) return;
		if(SessionManager.isKinectStarted)
		{
			string s = System.Environment.NewLine+"Time = " + System.DateTime.Now.ToString("HH:mm:ss.fff")+ ", " + str ;
			System.IO.File.AppendAllText(fileName, s);
		}
	}
	public static void log(string str)
	{
		if(fileName==string.Empty) return;
		string s = System.Environment.NewLine +System.DateTime.Now.ToString("HH:mm:ss.fff")+ ", " + str ;
		System.IO.File.AppendAllText(fileName, s);		
	}
	
	public static void log3Dpoint(string str, Vector3 pnt)
	{
		if(fileName==string.Empty) return;
		string s = System.Environment.NewLine+ System.DateTime.Now.ToString("HH:mm:ss.fff")+ ", " + str + pnt.x.ToString("F7")+ ", "+ pnt.y.ToString("F7")+ ", "+ pnt.z.ToString("F7");
		System.IO.File.AppendAllText(fileName, s);		
	}
	
	public static void Write2DPoint(string handEye,Vector2 point)
	{
		if(fileName==string.Empty) return;
		string s = System.Environment.NewLine+ System.DateTime.Now.ToString("HH:mm:ss.fff")+ ", " + handEye + ", X = " + point.x.ToString() + " Y = "+ point.y.ToString() ;
		System.IO.File.AppendAllText(fileName, s);
	}
	
	public static void WriteQuestion(string str)
	{
		if(SessionManager.isKinectStarted)
		{
			qCount++;
			string s = System.Environment.NewLine +"Time = " + System.DateTime.Now.ToString("HH:mm:ss.fff")+ ", " + qCount.ToString()+". "+str ;
			System.IO.File.AppendAllText(questionFileName, s);
		}
	}
	
	public static void LogQuestion(string actionName)
	{
		if(!actionName.Contains("_text_")) return;
		if(!SessionManager.isKinectStarted) return;
		switch(actionName)
		{
		case "Fridge_wineCellar_leftDown_door_text_acousticAlarms":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_door_text_capacity":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_filter_text_newFunctionalities":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_doorShelf_text_Decoration":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_filter_text_dynamicAir":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_handle_text_matchingColors":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_slidingShelf_text_position":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_open_text_coolmatic":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_handle_text_newHandle":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_door_text_frostmatic":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_text_designIcon":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_display_text_touchControl":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_vegetableDrawer_text_55_litres":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_handle_text_matchingColors":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_cabinetShelves_text_metalLook":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_text_ledLighting":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_display_text_childLock":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_cabinetShelves_text_shape":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_text_matchingColors":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_handle_text_new_handle":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_text_perfectFit":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_door_text_antiVibration":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_close_text_coolmatic":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_text_new_handle":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_text_warningLIghts":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_text_ledLighting":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_doorShelf_text_layout":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_text_foodStorage":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_display_text_lcdDisplay":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_text_temperature":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_halfDepthShelf_text_organization":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_display_text_minuteMinder":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_shelves_text_newGlass":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_close_text_dynamicAir":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_text_holidayFunction":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_door_text_twoParts":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_text_frostmatic":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_text_refrigerant":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_text_stainlessSteel":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_text_acousticAlarms":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_open_text_dynamicAir":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_shelves_text_WoodenShelves":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_text_energyClass":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_door_text_temperedGlass":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_door_text_temperature":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_drawerSecond_text_newDrawers":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_text_americanDream":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_vegetableDrawer_text_regulation":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_doorShelf_text_shape":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDown_vegetableDrawer_text_maxiBox":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_text_empowerment":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_door_text_noFrost":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_text_fridgeTemperature":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightUp_filter_text_fan":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftDown_drawerThird_text_freezerDrawers":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_rightDoor_text_newFunctionalities":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_shelves_text_Capacity":	
			WriteQuestion("");
			break;
		case "Fridge_wineCellar_leftUp_text_acousticSignal":	
			WriteQuestion("");
			break;
		
		}
	}
	
	public static void LogTextActions(Action action)
	{
		if(!action.actionName.Contains("_text_")) return;
		if(!SessionManager.isKinectStarted) return;
		
		TimeSpan t = DateTime.Now - DateTime.MinValue;
		TextMeshAppearAction textAction = (TextMeshAppearAction) action;
		qCount++;
		string s = System.Environment.NewLine +qCount.ToString()+"; Time = " + System.DateTime.Now.ToString("MMM ddd d yyyy HH:mm:ss.fff")+ "; "+t.TotalMilliseconds.ToString()+"; Action "+ action.actionName+ "; " +textAction.text + ";";
		System.IO.File.AppendAllText(questionFileName, s);
		
		Debug.Log(DateTime.MinValue.ToString());
	}
	*/
}
