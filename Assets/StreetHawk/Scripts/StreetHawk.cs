using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public struct SHFriendlyNameObject
{
	public string friendlyName;
	public string vc;
	public string xib_iphone;
	public string xib_ipad;
	
	public SHFriendlyNameObject (string _friendlyName, string _vc, string _xib_iphone = "", string _xib_ipad = "")
	{
		this.friendlyName = _friendlyName;
		this.vc = _vc;
		this.xib_iphone = _xib_iphone;
		this.xib_ipad = _xib_ipad;
	}
	
	
}

public class StreetHawk : MonoBehaviour
{
	public static StreetHawk Instance;

	string URL;

	string scheme;
	string host;
	List<string> path;
	Dictionary<string,string> queries;

	public static event Action shDeeplinking;
	public static event Action<string,string,string> shRawJsonCallback;
	public static event Action<string> launchAppPage;

	#if UNITY_IPHONE || UNITY_IOS
	[DllImport ("__Internal")]
	private static extern void _registerInstallForApp (string appID, bool isDebugMode, string iTunesId);
	
	[DllImport ("__Internal")]
	private static extern void _tagNumeric (double value, string key);
	
	[DllImport ("__Internal")]
	private static extern void _tagString (string value, string key);
	
	[DllImport ("__Internal")]
	private static extern void _tagDateTime (string date, string key);
	
	[DllImport ("__Internal")]
	private static extern void _incrementTag (string keyToIncrement);
	
	[DllImport ("__Internal")]
	private static extern void _removeTag (string keyToRemove);

	[DllImport ("__Internal")]
	private static extern void _shNotifyPageEnter (string page_name);

	[DllImport ("__Internal")]
	private static extern void _shSetiTunesId (string itunesId);
	
	[DllImport ("__Internal")]
	private static extern void _shSetEnableLogs (bool debugMode);
	
	[DllImport ("__Internal")]
	private static extern void _shSetIsPushNotificationEnabled (bool isEnable);
	
	[DllImport ("__Internal")]
	private static extern void _shSetDefaultPushNotificationSupport (bool isEnable);
	
	[DllImport ("__Internal")]
	private static extern void _shSetDefaultLocationSupport (bool isEnable);
	
	[DllImport ("__Internal")]
	private static extern void _shSetIsUseLocation (bool isEnable);
	
	[DllImport ("__Internal")]
	private static extern void _shSetAlertSetting (int minutes);
	
	[DllImport ("__Internal")]
	private static extern void _shSendSimpleFeedback (string title, string message);
	
	[DllImport ("__Internal")]
	private static extern void _shCustomActivityList (string[] friendlyName, string[] vc, string[] xib_iphone, string[] xib_ipad, int count);
	
	[DllImport ("__Internal")]
	private static extern int _shAlertSettings ();
	
	[DllImport ("__Internal")]
	private static extern string _shiTunesId ();
	
	[DllImport ("__Internal")]
	private static extern bool _shEnableLogs ();
	
	[DllImport ("__Internal")]
	private static extern bool _shIsUseLocation ();
	
	[DllImport ("__Internal")]
	private static extern bool _shIsPushNotificationEnabled ();
	
	[DllImport ("__Internal")]
	private static extern string _getSHLibraryVersion ();
	
	[DllImport ("__Internal")]
	private static extern string _shGetViewName ();
	
	
	#endif

	#if UNITY_ANDROID
	//UnityPlayer Activity
	private AndroidJavaClass unityPlayer;
	//Current Activity 
	private AndroidJavaObject CurrentActivity; 
	//Current Application
	private AndroidJavaObject CurrentApplication;
	//Reference to StreetHawk Enum
	private AndroidJavaClass StreetHawkClass;
	//Instance of StreetHawk Enum
	private AndroidJavaObject INSTANCE; 
	//Instance of UnityWrapper Class
	private AndroidJavaObject Callback; 
	#endif



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


	void Awake ()
	{
		Instance = this;
		#if UNITY_ANDROID
		unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		CurrentActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		CurrentApplication = CurrentActivity.Call<AndroidJavaObject> ("getApplication");
		StreetHawkClass = new AndroidJavaClass ("com.streethawk.library.StreetHawk");
		INSTANCE = StreetHawkClass.GetStatic<AndroidJavaObject> ("INSTANCE");
		Callback = new AndroidJavaObject ("com.streethawk.library.unity.UnityWrapper");
		#endif
	}

	void Start ()
	{
		#if UNITY_ANDROID
		URLHandler (GetURI ());
		#elif UNITY_EDITOR
		URL = null;
		#endif	
	}

	void URLHandler (string url)
	{
		//Format for URL : "scheme://host/path?query"
		//Where format for query = "key=value"
		URL = url;
		if (URL == null)
			return;
		
		if (parseURL ()) {
			shDeeplinking ();
		}
	}

	bool parseURL ()
	{
		bool success = false;
		try {
			var uri = new Uri (URL);
			scheme = uri.Scheme;
			host = uri.Host;
			path = new List<string> ();
			if (uri.AbsolutePath != "") {
				path.AddRange (Regex.Split (uri.AbsolutePath, "/"));
				path.RemoveAt (0);
			}
			queries = new Dictionary<string, string> ();
			string[] q = Regex.Split (uri.Query.Replace ("?", ""), "&");
			for (int i = 0; i < q.Length; i++) {
				if (q [i] != "") {
					string key = Regex.Split (q [i], "=") [0];
					string value = Regex.Split (q [i], "=") [1];
					queries.Add (key, value);
				}
				
			}

			success = true;

		} catch {
			Debug.Log ("Unknown URL Format");
			Debug.Log ("Bad Format : " + URL);
			URL = null;
		}
		return success;
	}

	void OnApplicationPause (bool pause)
	{
		if (pause) {

		} else {
			string ViewName = shGetViewName ();
			if (ViewName != null) {
				launchAppPage (ViewName);
			}
		}
	}


	/**
	 * Use this API for check if streethawk is initialized especially in non native platforms returns true is Streethawk is initialized else return false
	 * 
	 * @returns bool 
	 */
	public bool isStreethawkInitialized ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return false;
		#elif UNITY_ANDROID
		return StreetHawkClass.Call<bool> ("isStreethawkInitialized");
		#elif UNITY_EDITOR
		return false;
		#endif
	}

	/**
	 * Initialise Streethawk SDK
	 */
	public void streethawkinit (string appKey, string SenderID, string iTunesId)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_registerInstallForApp (appKey, true, iTunesId);
		#elif UNITY_ANDROID
		INSTANCE.Call ("setGcmSenderId", SenderID);
		INSTANCE.Call ("init", CurrentApplication);
		#endif
	}
	
/**
	 * For Android
	 * Call this API for submitting map of friendly names and fully qualified names to Streethawk library.
	 * 
	 * @param Dictionary<string, string> _ActivityList
	 */
	public void shCustomActivityList (Dictionary<string, string> _ActivityList)
	{
		#if UNITY_ANDROID
		using (AndroidJavaObject obj_HashMap = new AndroidJavaObject("java.util.HashMap")) {
			
			// Call 'put' via the JNI instead of using helper classes to avoid:
			//  "JNI: Init'd AndroidJavaObject with null ptr!"
			System.IntPtr method_Put = AndroidJNIHelper.GetMethodID (obj_HashMap.GetRawClass (), "put", 
			                                                         
			                                                         "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			
			object[] args = new object[2];
			
			foreach (KeyValuePair<string, string> kvp in _ActivityList) {
				using (AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key)) {
					using (AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value)) {
						args [0] = k;
						args [1] = v;
						AndroidJNI.CallObjectMethod (obj_HashMap.GetRawObject (), 
						                             method_Put, AndroidJNIHelper.CreateJNIArgArray (args));
					}
				}
			}
			INSTANCE.Call ("shCustomActivityList", obj_HashMap);
		}
		#endif
	}

	/**
	 * For iOS
	 * Lets you register with Streethawk Server with an array of Friendly Name Definitions which define 
	 * friendly name string, the mapping view controller, and xib for iPhone or iPad if necessary 
	 * so that you can launch any activity/Scene of your application on your user’s device from StreetHawk console
	 * @param SHFriendlyNameObject[] list
	 */
	public void shCustomActivityList (params SHFriendlyNameObject[] list)
	{
		#if UNITY_IPHONE || UNITY_IOS
		string[] nameList = new string[list.Length];
		string[] vcList = new string[list.Length];
		string[] xib_iphoneList = new string[list.Length];
		string[] xib_ipadList = new string[list.Length];
		for (int i = 0; i < list.Length; i++) {
			nameList [i] = list [i].friendlyName;
			vcList [i] = list [i].vc;
			xib_iphoneList [i] = list [i].xib_iphone;
			xib_ipadList [i] = list [i].xib_ipad;
		}
		_shCustomActivityList (nameList, vcList, xib_iphoneList, xib_ipadList, list.Length);
		#endif
	}
	
	public string GetURI ()
	{
		string value = null;
		#if UNITY_ANDROID

		try {
			using (AndroidJavaObject Intent = CurrentActivity.Call<AndroidJavaObject>("getIntent")) {
				using (AndroidJavaObject Data = Intent.Call<AndroidJavaObject>("getData")) {
					value = Data.Call<string> ("toString"); 
				}
			}
		} catch (Exception e) {
			Debug.Log (e.Message);
			value = null;
		}

		#endif
		return value;
	}

	/**
	 * API to numeric tag a profile
	 * 
	 * @param string key
	 * @param double numeric_Value
	 *	 
	 * @returns bool - true for success false if failed 
	 */
	public void tagNumeric (string key, double numeric_Value)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_tagNumeric (numeric_Value, key);
		#elif UNITY_ANDROID
		INSTANCE.Call<bool> ("tagNumeric", key, numeric_Value);
		#endif	
	}

	/**
	 * API to string tag a profile
	 * 
	 * @param string key
	 * @param string string_Value
	 *	 
	 * @returns bool - true for success false if failed 
	 */
	public void tagString (string key, string string_Value)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_tagString (string_Value, key);
		#elif UNITY_ANDROID
		INSTANCE.Call<bool> ("tagString", key, string_Value);
		#endif	
	}
	
	/**
	 * API to tag using date-time
	 * 
	 * @param string key
	 * @param string datetime_Value
	 *	 
	 * @returns bool - true for success false if failed 
	 */
	public void tagDateTime (string key, string datetime_Value)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_tagDateTime (datetime_Value, key);
		#elif UNITY_ANDROID
		INSTANCE.Call<bool> ("tagDatetime", key, datetime_Value);
		#endif	
	}

	/**
	 * API to current formatted dateTime in UTC
	 * 
	 * @returns string UTC Time
	 */
	public string getCurrentFormattedDateTime ()
	{
		DateTime time = DateTime.Now;           
		string format = "yyyy-MM-dd HH:mm:ss";   
		return time.ToString (format);
	}
	
	/**
	 * Api to increment a tag
	 * 
	 * @param string key
	 */
	public void incrementTag (string key)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_incrementTag (key);
		#elif UNITY_ANDROID
		INSTANCE.Call ("incrementTag", key);
		#endif
	}
	
	/**
	 * Api to delete a custom profile tag
	 * 
	 * @param string key
	 */
	public void removeTag (string key)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_removeTag (key);
		#elif UNITY_ANDROID
		INSTANCE.Call ("removeTag", key);
		#endif
				
	}
	
	/**
	 * API to set beacon feature, pass true to enable this feature,  by default Beacon feature is disabled 
	 * Call this API before Init()
	 * 
	 * @param bool enable
	 */
	public void shSetBeaconSupport (bool enable)
	{
		#if UNITY_ANDROID
		INSTANCE.Call ("shSetBeaconSupport", enable);
		#endif
	}
	
/**
	* API to notify streethawk server when device enters a beacon region.
	* Use this API if you are using third party library support for beacons.
	* Please note that respective beacon's parameters (UUID major and minor) needs to be registered with Streethawk server
	* 
	* @param string UUID - UUID of the beacon detected
	* @param int major - major number of beacon detected
	* @param int minor - minor number of beacon detected
	* @param double distance - distance of beacon from device
	*	 
	* @returns int - true for successful reporting, false for error, Check log messages for details
	*/
	public bool shEnterBeacon (string UUID, int major, int minor, double distance)
	{
		#if UNITY_IPHONE || UNITY_IOS
		return true;
		#elif UNITY_ANDROID
		return INSTANCE.Call <int> ("shEnterBeacon", CurrentActivity, UUID, major, minor, distance) == 0 ? true : false;
		#elif UNITY_EDITOR
		return false;
		#endif


	}
	
	/**
	* API to notify streethawk server when device exits a beacon region
	* Use this API if you are using third party library support for beacons
	* 
	* @param string UUID - UUID of the beacon detected
	* @param int major - major number of beacon detected
	* @param int minor - minor number of beacon detected
	*	 
	* @returns bool - true for successful reporting, false for error, Check log messages for details
	*/
	public bool shExitBeacon (string UUID, int major, int minor)
	{
		#if UNITY_IPHONE || UNITY_IOS
		return true;
		#elif UNITY_ANDROID
		return INSTANCE.Call <int> ("shExitBeacon", CurrentActivity, UUID, major, minor) == 0 ? true : false;
		#elif UNITY_EDITOR
		return false;
		#endif
	
	}
	
	/**
	 * Use this API to notify streethawk when user enters a new page.
	 * App developer is expected to call this API for all the pages whose analytics is of interest to him.
	 * 
	 * @param string page_name
	 */
	public void currentPage (string page_name)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shNotifyPageEnter (page_name);
		#elif UNITY_ANDROID
		if (isStreethawkInitialized ())
			INSTANCE.Call ("shNotifyPageEntered", page_name);
		#endif
				
	}
	
	/**
	 * Api to send simple feedback to server.
	 * 
	 * @param string title
	 * @param string message
	 */
	public void shSendSimpleFeedback (string title, string message)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shSendSimpleFeedback (title, message);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shSendSimpleFeedback", title, message);
		#endif
				
	}
	
	/**
	 * API to get version of StreetHawk Library
	 * 
	 * @returns string - version of Streethawk Library
	 */
	public string getSHLibraryVersion ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _getSHLibraryVersion ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<string> ("getSHLibraryVersion");
		#elif UNITY_EDITOR
		return null;
		#endif	
	}
	
	public void shSetEnableLogs (bool enable)
	{	
		#if UNITY_IPHONE || UNITY_IOS
		_shSetEnableLogs (enable);
		#elif UNITY_ANDROID
		INSTANCE.Set<bool> ("ENABLE_STREETHAWK_LOGS", enable);
		#endif	
	}
	
	public bool shEnableLogs ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _shEnableLogs ();
		#elif UNITY_ANDROID
		return INSTANCE.Get<bool> ("ENABLE_STREETHAWK_LOGS");
		#elif UNITY_EDITOR
		return false;
		#endif
	}
	
	public void shSetiTunesId (string itunesId)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shSetiTunesId (itunesId);
		#endif
	}

	/**
	 * API to set application specific sender key for push notifications.
	 * 
	 * @param string SenderID - Project Number
	 */ 
	public void SetGcmSenderId (string SenderID)
	{
		#if UNITY_ANDROID
		INSTANCE.Call ("setGcmSenderId", SenderID);
		#endif
	}
	
	public string shiTunesId ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _shiTunesId ();
		#elif UNITY_ANDROID
		return null;
		#elif UNITY_EDITOR
		return null;
		#endif
	}
	
	/**
	 * API to disable push notification feature pass false to disable this features. By default push notifications are enabled
	 * 
	 * @param bool enable
	 */
	public void shSetDefaultPushNotificationSupport (bool enable)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shSetDefaultPushNotificationSupport (enable);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shSetGcmSupport", enable);
		#endif
	}
	
	/**
	 * API to disable push notification feature pass false to disable this features. By default push notifications are enabled
	 * 
	 * @param bool enable
	 */
	public void shSetIsPushNotificationEnabled (bool enable)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shSetIsPushNotificationEnabled (enable);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shSetPushNotificationSupport", enable);
		#endif
	}
	
	/**
	 * API to query status of push notifications.
	 * 
	 * @returns bool
	 */
	public bool shIsPushNotificationEnabled ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _shIsPushNotificationEnabled ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<bool> ("isPushNotificationsEnabled");
		#elif UNITY_EDITOR
		return false;
		#endif
	}

	/**
	 * Call this API to set the Location.
	 * 
	 * @param double latitude
	 * @param double longitude
	 */
	public void shSetDefaultLocationSupport (bool enable)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shSetDefaultLocationSupport (enable);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shSetLocationSupport", enable);
		#endif
	}
	
	/**
	 * API to set location feature. Set false to disable location , true to resume location reporting by application by default locations feature is enabled
	 * Call this API before Init()
	 * 
	 * @param bool enable
	 */
	public void shSetIsUseLocation (bool enable)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shSetIsUseLocation (enable);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shSetLocationSupport", enable);
		#endif
				
	}

	/**
	 * Returns true if Streethawk's location is enabled in your code else returns false
	 * 
	 * @returns bool - true when enabled and false when disabled.
	 */
	public bool shIsUseLocation ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _shIsUseLocation ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<bool> ("isUseLocation");
		#elif UNITY_EDITOR
		return false;
		#endif
				
	}

	/**
	 * call AlertSetting() for pausing push messages for given duration of time in minutes.
	 * 
	 * @param int pauseMinutes
	 */
	public void shSetAlertSetting (int pauseMinutes)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shSetAlertSetting (pauseMinutes);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shAlertSetting", pauseMinutes);
		#endif
	}

/** 
	 * GetAlertSettings returns the time remaining in minutes before pause minutes set in AlertSetting(int) expires
	 *	 
	 * @returns int - time remaining before alerts will be enabled again
	 */
	public int shAlertSettings ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _shAlertSettings ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<int> ("shGetAlertSettings");
		#elif UNITY_EDITOR
		return 0;
		#endif
				
	}

	/**
	 * This API returns name of the page, if any requested by streethawk server, when app is in BG.
	 * 
	 * @returns string - app page to be displayed else returns null
	 */
	public string shGetViewName ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _shGetViewName ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<string> ("getAppPage");
		#elif UNITY_EDITOR
		return null;
		#endif
				
	}

	public void shRawJsonCallbackEvent (string JSON)
	{
		string title = null;
		string message = null;
		string rawJSON = JSON;
		shRawJsonCallback (title, message, rawJSON);
		Debug.Log ("rawJSON Received");
	}

	public void shNotifyAppPageEvent (string appPage)
	{
		launchAppPage (appPage);
		Debug.Log ("shNotifyAppPage Received");
	}

	public void registerObserver ()
	{
		#if UNITY_ANDROID
		INSTANCE.Call ("registerSHObserver", Callback);
		Debug.Log ("register Observer");
		#endif
	}

}
