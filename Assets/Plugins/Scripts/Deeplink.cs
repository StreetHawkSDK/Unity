using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

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
		if (URL == null)
			return;

		parseURL ();
	}


	void parseURL ()
	{
		try {
			var uri = new Uri (URL);
			scheme = uri.Scheme;
			host = uri.Host;
			path = new List<string> ();
			path.AddRange (Regex.Split (uri.AbsolutePath, "/"));
			path.RemoveAt (0);
			queries = new Dictionary<string, string> ();
			string[] q = Regex.Split (uri.Query.Replace ("?", ""), "&");
			for (int i = 0; i < q.Length; i++) {
				
				string key = Regex.Split (q [i], "=") [0];
				string value = Regex.Split (q [i], "=") [1];
				queries.Add (key, value);
				
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
