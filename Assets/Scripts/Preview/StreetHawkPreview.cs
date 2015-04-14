using UnityEngine;
using System.Collections;

public class StreetHawkPreview : StreetHawkPreviewGUIBASE
{



	void OnGUI ()
	{
		
		UpdateToStartPos ();
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "StreetHawk", style);
		
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Init")) {
			StreetHawk.Init ("Sudoku_Unlimited", "355613836466", "948704288");
		}
		
		StartX += XButtonStep;

		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetPushNotifSupport True")) {
			StreetHawk.SetPushNotificationSupport (true);
		}
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetPushNotifSupport False")) {
			StreetHawk.SetPushNotificationSupport (false);
		}
		StartY += YButtonStep;
		StartX += XButtonStep;
		StartX = XStartPos;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "IsPushNotifEnabled?")) {
			Debug.Log (StreetHawk.isPushNotificationsEnabled ());
		}
//				StartX += XButtonStep;
//				if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "EnableLogs_True")) {
//						//StreetHawk.SetEnableLogs (true);
//				}
//				StartX += XButtonStep;
//				if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "EnableLogs_False")) {
//						//StreetHawk.SetEnableLogs (false);
//				}
//
//				StartX += XButtonStep;
//				if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "isLogEnabled?")) {
//						//Debug.Log (StreetHawk.IsLogEnabled ());
//				}

		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Tagging", style);


		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Tag Numeric")) {
			StreetHawk.tagNumeric ("TagNumericTest", 99.99);
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Tag String")) {
			StreetHawk.tagString ("TagStringTest", "Test Value");
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Tag DateTime")) {
			StreetHawk.tagDateTime ("TagDateTimeTest", StreetHawk.getCurrentFormattedDateTime ());
		}

		StartX = XStartPos;
		StartY += YButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Increment Tag")) {
			StreetHawk.incrementTag ("IncrementTagTest");
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Remove Tag")) {
			StreetHawk.removeTag ("IncrementTagTest");//Remove An Existing Tag
		}
			

		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Beacons", style);
		

		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetBeaconSupport True")) {
			StreetHawk.SetBeaconSupport (true);
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetBeaconSupport False")) {
			StreetHawk.SetBeaconSupport (false);
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "isUseBeacon?")) {
			Debug.Log (StreetHawk.isUseBeacons ());
		}

		StartX = XStartPos;
		StartY += YButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Enter Beacon")) {
			Debug.Log (StreetHawk.EnterBeacon ("fb0b57a2-8228-44 cd-913a-94a122b", 2, 1, 500, "identifier"));
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Exit Beacon")) {
			Debug.Log (StreetHawk.ExitBeacon ("fb0b57a2-8228-44 cd-913a-94a122b", 2, 1, "identifier"));
		}


		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Location", style);
		
		
		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetLocationSupport_True")) {
			StreetHawk.SetLocationSupport (true);
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetLocationSupport_False")) {
			StreetHawk.SetLocationSupport (false);
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "isUseLocation?")) {
			Debug.Log (StreetHawk.isUseLocation ());
		}
		StartY += YButtonStep;
		StartX += XButtonStep;
		StartX = XStartPos;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetManualLocation")) {
			StreetHawk.SetManualLocation (25.6, 45.36);
		}
			
				
		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Alert Settings", style);
		
		
		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetAlertSettings")) {
			StreetHawk.SetAlertSettings (5);
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "GetAlertSettings")) {
			Debug.Log (StreetHawk.GetAlertSettings ());
		}


		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Analytics", style);
		
		
		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SendSimpleFeedback")) {
			StreetHawk.SendSimpleFeedback ("Feedback", "This is a feedback");
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "NotifyPageEntered")) {
			StreetHawk.NotifyPageEntered ("HomePage");
		}

	}





























}
