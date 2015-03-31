//using UnityEngine;
//using System.Collections;
//
//public class StreetHawkUI : MonoBehaviour
//{
//	
//
//		public string SenderID = "355613836466";
//		//public string test;
//		// Use this for initialization
//		void Start ()
//		{
//				Debug.Log (test.get ());
//				
//				//	Deeplink ();
//				StreetHawk.SetGcmSenderId (SenderID);
//
//				Debug.Log ("Calling >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
////				streetHawk.SetGcmSenderId (SenderID); 
//				StreetHawk.Init ();
//				Debug.Log ("Caled >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
////				streetHawk.getCurrentFormattedDateTime ();
//		
//		}
//	
//		public void Deeplink ()
//		{
//				string screen = StreetHawk.GetQueryParameter ("screen");
//				string param = StreetHawk.GetQueryParameter ("param");
//		
//				switch (screen) {
//				case "Menu":
//						break;
//			
//				case "Game":
//						break;
//			
//				case "Shop":
//						break;
//				}
//				Debug.Log ("param: " + param);
//		}
//	
//		// Update is called once per frame
//		void Update ()
//		{
//				//Debug.Log (".isPushNotificationsEnabled >>>>>>>>>>>>>>>>" + streetHawk.isPushNotificationsEnabled ());
//				//Debug.Log ("GetApplication: >>>>>>>>>>>>>>>>>>>>" + streetHawk.CurrentApplication);
//				Debug.Log ("isStreethawkInitialized >>>>>>>>>>>>>>>>" + StreetHawk.isStreethawkInitialized ());
//		}
//}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreetHawkUI : MonoBehaviour
{


		

		void Start ()
		{
				Deeplink ();
		}

		public void Deeplink ()
		{
				string screen = StreetHawk.GetQueryParameter ("screen");
				List<string> param = StreetHawk.GetPathSegments ();
	
				switch (screen) {
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
				
				foreach (string s in param) {
						Debug.Log (s + ">>>>>>>>>>>>");
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

