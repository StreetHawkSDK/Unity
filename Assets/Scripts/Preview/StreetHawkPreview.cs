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
		
			StreetHawk.Instance.streethawkinit ("Sudoku_Unlimited", "355613836466", "948704288");
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "RegisterObserver")) {
			
			StreetHawk.Instance.registerObserver ();
		}
		
		StartY += YButtonStep;
		StartX += XButtonStep;
		StartX = XStartPos;

		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetPushNotifSupport True")) {
			StreetHawk.Instance.shSetIsPushNotificationEnabled (true);
		}
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetPushNotifSupport False")) {
			StreetHawk.Instance.shSetIsPushNotificationEnabled (false);
		}
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "IsPushNotifEnabled?")) {
			Debug.Log (StreetHawk.Instance.shIsPushNotificationEnabled ());
		}

		StartY += YButtonStep;
		StartX += XButtonStep;
		StartX = XStartPos;

		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "EnableLogs_True")) {
			StreetHawk.Instance.shSetEnableLogs (true);
		}
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "EnableLogs_False")) {
			StreetHawk.Instance.shSetEnableLogs (false);
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "isLogEnabled?")) {
			Debug.Log (StreetHawk.Instance.shEnableLogs ());
		}

		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Tagging", style);


		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Tag Numeric")) {
			StreetHawk.Instance.tagNumeric ("TagNumericTest", 99.99);
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Tag String")) {
			StreetHawk.Instance.tagString ("TagStringTest", "Test Value");
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Tag DateTime")) {
			StreetHawk.Instance.tagDateTime ("TagDateTimeTest", StreetHawk.Instance.getCurrentFormattedDateTime ());
		}

		StartX = XStartPos;
		StartY += YButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Increment Tag")) {
			StreetHawk.Instance.incrementTag ("IncrementTagTest");
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Remove Tag")) {
			StreetHawk.Instance.removeTag ("IncrementTagTest");//Remove An Existing Tag
		}
			

		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Beacons", style);
		

		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetBeaconSupport True")) {
			StreetHawk.Instance.shSetBeaconSupport (true);
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetBeaconSupport False")) {
			StreetHawk.Instance.shSetBeaconSupport (false);
		}

		StartX = XStartPos;
		StartY += YButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Enter Beacon")) {
			Debug.Log (StreetHawk.Instance.shEnterBeacon ("fb0b57a2-8228-44 cd-913a-94a122b", 2, 1, 500));
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "Exit Beacon")) {
			Debug.Log (StreetHawk.Instance.shExitBeacon ("fb0b57a2-8228-44 cd-913a-94a122b", 2, 1));
		}


		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Location", style);
		
		
		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetLocationSupport_True")) {
			StreetHawk.Instance.shSetIsUseLocation (true);
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetLocationSupport_False")) {
			StreetHawk.Instance.shSetIsUseLocation (false);
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "isUseLocation?")) {
			Debug.Log (StreetHawk.Instance.shIsUseLocation ());
		}

			
				
		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Alert Settings", style);
		
		
		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SetAlertSettings")) {
			StreetHawk.Instance.shSetAlertSetting (5);
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "GetAlertSettings")) {
			Debug.Log (StreetHawk.Instance.shAlertSettings ());
		}


		StartX = XStartPos;
		StartY += YLableStep;
		StartY += YLableStep;
		GUI.Label (new Rect (StartX, StartY, Screen.width, 40), "Analytics", style);
		
		
		StartX = XStartPos;
		StartY += YLableStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "SendSimpleFeedback")) {
			StreetHawk.Instance.shSendSimpleFeedback ("Feedback", "This is a feedback");
		}
		
		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "CurrentPage")) {
			StreetHawk.Instance.currentPage ("HomePage");
		}

		StartX += XButtonStep;
		if (GUI.Button (new Rect (StartX, StartY, buttonWidth, buttonHeight), "GetViewName")) {
			Debug.Log (StreetHawk.Instance.shGetViewName ());
		}

	}





























}
