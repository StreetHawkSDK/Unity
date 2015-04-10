using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Deeplink : MonoBehaviour
{
	public static string URL;
	string scheme;
	string host;
	List<string> path;
	Dictionary<string,string> queries;

	public static Deeplink Instance ;

	void Awake ()
	{
		Instance = this;
		#if UNITY_ANDROID
		URLHandler (StreetHawk.GetURI ());
		#elif UNITY_EDITOR
		URL = null;
		#endif	

	}

	public void URLHandler (string url)
	{
		//Format for URL : "scheme://host/path?query"
		//Where format for query = "key=value"
		URL = url;
		parseURL ();
	}


	void parseURL ()
	{
		try {
			string[] urlInPieces = Regex.Split (URL, "//");
			scheme = urlInPieces [0].Replace (":", "");

			string[] urlInPieces2 = Regex.Split (Regex.Split (URL, "//") [1], "/");
			host = urlInPieces2 [0];

			path = new List<string> ();
			for (int i = 1; i < urlInPieces2.Length; i++) {
				string _path = urlInPieces2 [i].Split ('?') [0];
				path.Add (_path);
			}
			queries = new Dictionary<string, string> ();
			for (int i = 1; i < urlInPieces2.Length; i++) {

				string _queries = urlInPieces2 [i].Split ('?') [1];
				string[] __queries = _queries.Split ('&');
				for (int j = 0; j < __queries.Length; j++) {
					string key = Regex.Split (__queries [j], "=") [0];
					string value = Regex.Split (__queries [j], "=") [1];
					queries.Add (key, value);
				}
			}
		} catch {
			Debug.Log ("Unknown URL Format");
			Debug.Log ("Bad Format : " + URL);
			URL = null;
		}
	}

	public string Scheme {
		get {
			return (URL != null) ? scheme : null;
		}
	}

	public string Host {
		get {
			return (URL != null) ? host : null;
		}
	}

	public List<string> PathSegments {
		get {
			return (URL != null) ? path : new List<string> ();
		}
	}

	public Dictionary<string,string> Queries {
		get {
			return (URL != null) ? queries : new Dictionary<string,string> ();
		}
	}

}
