/*
 * Jin Jiawei
 * 
 * University of Helsinki
 * 
*/
using UnityEngine;
using System.Collections;
using System;

public class CircleController : MonoBehaviour
{
	public Texture2D TextureCircle;
	private int coordinate_left, coordinate_top, diameter_circle;
	public Vector2 coordinate_circle;
	private Vector2 coordinate_cursor;
	private int radius_circle;
	public bool flag_isClick, flag_isMoveable;
	private int temp_distance_circle_cursor_x;
	private int temp_distance_circle_cursor_y;
	private string circleColor;
	private bool tmp_isClick;
	
	void Start()
	{
		if(gameObject.name.Contains("Object"))
			circleColor = PointingParameters.ObjectColor;
		else
			circleColor = PointingParameters.TargetColor;
		
		switch (circleColor) {
		case "blue":
			TextureCircle = Instantiate (Resources.Load ("TextureCircle_Blue") as Texture2D) as Texture2D;
			break;
		case "green":
			TextureCircle = Instantiate (Resources.Load ("TextureCircle_Green") as Texture2D) as Texture2D;
			break;
		case "pink":
			TextureCircle = Instantiate (Resources.Load ("TextureCircle_Pink") as Texture2D) as Texture2D;
			break;
		case "red":
			TextureCircle = Instantiate (Resources.Load ("TextureCircle_Red") as Texture2D) as Texture2D;
			break;
		case "yellow":
			TextureCircle = Instantiate (Resources.Load ("TextureCircle_Yellow") as Texture2D) as Texture2D;
			break;
		}
	}
	
	//draw the circle
	void OnGUI ()
	{	
		GUI.DrawTexture (new Rect (coordinate_left, coordinate_top, diameter_circle, diameter_circle), TextureCircle);
	}
	
	//0 is a Vector2 coordinate of circle, 1 is radius of circle
	void SetCirclePositionAndSize (object[] parameter)
	{
		coordinate_circle = (Vector2)parameter [0];
		radius_circle = (int)parameter [1];
		coordinate_left = (int)coordinate_circle.x - radius_circle;
		coordinate_top = (int)coordinate_circle.y - radius_circle;
		diameter_circle = radius_circle * 2;
	}
	//check if mouse is click and cursor is in range
	void isClick (bool is_click)
	{
		flag_isClick = is_click;
		double distance_circle_cursor = Math.Sqrt (Math.Pow (coordinate_cursor.x - (int)coordinate_circle.x, 2) + Math.Pow (coordinate_cursor.y - (int)coordinate_circle.y, 2));
		if ((int)distance_circle_cursor <= radius_circle && flag_isClick && !tmp_isClick) {
			flag_isMoveable = true;
			temp_distance_circle_cursor_x = (int)coordinate_cursor.x - coordinate_left;
			temp_distance_circle_cursor_y = (int)coordinate_cursor.y - coordinate_top;
		} else if ((int)distance_circle_cursor <= radius_circle && tmp_isClick && !flag_isClick){
			flag_isMoveable = false;
		}
		tmp_isClick = flag_isClick;
	}
	//Move circle according to cursor's position if the circle is moveable
	void MoveCircle (Vector2 cursor_position)
	{
		coordinate_cursor.x = (int)cursor_position.x;
		coordinate_cursor.y = Screen.height - (int)cursor_position.y;
		if (flag_isMoveable) {
			coordinate_left = (int)coordinate_cursor.x - temp_distance_circle_cursor_x;
			coordinate_top = (int)coordinate_cursor.y - temp_distance_circle_cursor_y;
			coordinate_circle.x = coordinate_left + radius_circle;
			coordinate_circle.y = coordinate_top + radius_circle;
		}
	}

}
