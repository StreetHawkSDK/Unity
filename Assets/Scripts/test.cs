using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class test : MonoBehaviour
{



	// Use this for initialization
	void OnEnable ()
	{
		StreetHawk.shDeeplinking += deeplink;
		StreetHawk.launchAppPage += launchPage;
		StreetHawk.shRawJsonCallback += receiveJSON;


		Dictionary<string,string> customSceneList = new Dictionary<string, string> ();
		customSceneList.Add ("FRIENDLY_NAME", "SCENE_NAME");
		customSceneList.Add ("menu", "MainMenu");
		customSceneList.Add ("shop", "Shop");
		customSceneList.Add ("game", "Gameplay");

		StreetHawk.Instance.shCustomActivityList (customSceneList);
	}

	void receiveJSON (string arg1, string arg2, string arg3)
	{
		Debug.Log (arg3);
	}

	void launchPage (string page)
	{

		switch (page) {
		case "Menu":
			Debug.Log ("Opening Menu Screen >>>>>>>>>");
			break;
				
		case "Game":
			Debug.Log ("Opening Game Screen >>>>>>>>>");
			break;
				
		case "Shop":
			Debug.Log ("Opening Shop Screen >>>>>>>>>");
			break;
				
		default:
			Debug.Log ("DEFAULT");
			break;
		}
			

	}

	void deeplink ()
	{

		Debug.Log ("Scheme = " + StreetHawk.Instance.Scheme);
		Debug.Log ("Host = " + StreetHawk.Instance.Host);
		foreach (var str in StreetHawk.Instance.PathSegments) {
			Debug.Log ("Path : " + str);
		}
		foreach (KeyValuePair<string, string> pair in StreetHawk.Instance.Queries) {
			Debug.Log ("Key : " + pair.Key + " Value : " + pair.Value);
		}


		if (StreetHawk.Instance.Queries.ContainsKey ("screen")) {
			switch (StreetHawk.Instance.Queries ["screen"]) {
			case "Menu":
				Debug.Log ("Opening Menu Screen >>>>>>>>>");
				break;
				
			case "Game":
				Debug.Log ("Opening Game Screen >>>>>>>>>");
				break;
				
			case "Shop":
				Debug.Log ("Opening Shop Screen >>>>>>>>>");
				break;
				
			default:
				Debug.Log ("DEFAULT");
				break;
			}
			
		}

	}
	// Update is called once per frame
	void Update ()
	{
	
	}
}
