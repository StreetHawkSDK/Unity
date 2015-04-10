using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreetHawkUI : MonoBehaviour
{


		

	void Start ()
	{
		Deeplinking ();



		Dictionary<string,string> customSceneList = new Dictionary<string, string> ();
		customSceneList.Add ("FRIENDLY_NAME", "SCENE_NAME");
		customSceneList.Add ("menu", "MainMenu");
		customSceneList.Add ("shop", "Shop");
		customSceneList.Add ("game", "Gameplay");

		StreetHawk.CustomActivityList (customSceneList);


	}

	public void Deeplinking ()
	{

		Debug.Log ("Scheme = " + Deeplink.Instance.Scheme);
		Debug.Log ("Host = " + Deeplink.Instance.Host);
		foreach (var str in Deeplink.Instance.PathSegments) {
			Debug.Log ("Path : " + str);
		}
		foreach (KeyValuePair<string, string> pair in Deeplink.Instance.Queries) {
			Debug.Log ("Key : " + pair.Key + " Value : " + pair.Value);
		}

	

	
		switch (Deeplink.Instance.Queries ["screen"]) {
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

	// Update is called once per frame
	void Update ()
	{
		//Debug.Log (".isPushNotificationsEnabled >>>>>>>>>>>>>>>>" + StreetHawk.isPushNotificationsEnabled ());
		//Debug.Log ("GetApplication: >>>>>>>>>>>>>>>>>>>>" + StreetHawk.CurrentApplication);
		//Debug.Log ("isStreethawkInitialized >>>>>>>>>>>>>>>>" + StreetHawk.isStreethawkInitialized ());
	}
}

