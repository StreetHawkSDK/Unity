using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

public class StreetHawk
{
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
	private static extern void _setItunesId (string itunesId);
	
	[DllImport ("__Internal")]
	private static extern void _setEnableLogs (bool debugMode);
	
	[DllImport ("__Internal")]
	private static extern void _setPushNotificationEnabled (bool isEnable);
	
	[DllImport ("__Internal")]
	private static extern void _setDefaultPushNotificationEnabled (bool isEnable);
	
	[DllImport ("__Internal")]
	private static extern void _setIsDefaultLocationServiceEnabled (bool isEnable);
	
	[DllImport ("__Internal")]
	private static extern void _setIsLocationServiceEnabled (bool isEnable);
	
	[DllImport ("__Internal")]
	private static extern void _setAlertSetting (int minutes);
	
	[DllImport ("__Internal")]
	private static extern int _getAlertSettingMinutes ();
	
	
	[DllImport ("__Internal")]
	private static extern string _getItunesId ();
	
	[DllImport ("__Internal")]
	private static extern bool _isLogEnabled ();
	
	[DllImport ("__Internal")]
	private static extern bool _isLocationSupportEnabled ();
	
	[DllImport ("__Internal")]
	private static extern bool _isPushNotificationEnabled ();
	
	[DllImport ("__Internal")]
	private static extern bool _enterBeacon (string UUID, double majorNumber, double minorNumber);
	
	[DllImport ("__Internal")]
	private static extern void _exitBeacon (string UUID, double majorNumber, double minorNumber);

	[DllImport ("__Internal")]
	private static extern string _getSHLibraryVersion ();

	[DllImport ("__Internal")]
	private static extern void _shNotifyPageEnter (string name);

	[DllImport ("__Internal")]
	private static extern string _shGetViewName ();

	[DllImport ("__Internal")]
	private static extern void _sendSimpleFedback (string title, string message);

	#endif

	#if UNITY_ANDROID
	//UnityPlayer Activity
	private static AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
	//Current Activity 
	private static AndroidJavaObject CurrentActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
	//Current Application
	public static AndroidJavaObject CurrentApplication = CurrentActivity.Call<AndroidJavaObject> ("getApplication");
	//Reference to StreetHawk Enum
	private static AndroidJavaClass StreetHawkClass = new AndroidJavaClass ("com.streethawk.library.StreetHawk");
	//Instance of StreetHawk Enum
	public static AndroidJavaObject INSTANCE = StreetHawkClass.GetStatic<AndroidJavaObject> ("INSTANCE");

	#endif

	public static string GetURI ()
	{
		string value = "";
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
	 * Use this API for check if streethawk is initialized especially in non native platforms returns true is Streethawk is initialized else return false
	 * 
	 * @returns bool 
	 */
	public static bool isStreethawkInitialized ()
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
	 * API to set application specific sender key for push notifications.
	 * 
	 * @param string SenderID - Project Number
	 */ 
	public static void SetGcmSenderId (string SenderID)
	{
		#if UNITY_ANDROID
		INSTANCE.Call ("setGcmSenderId", SenderID);
		#endif
	}

	/**
	 * Call this API for submitting map of friendly names and fully qualified names to Streethawk library.
	 * 
	 * @param Dictionary<string, string> _ActivityList
	 */
	public static void CustomActivityList (Dictionary<string, string> _ActivityList)
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
	 * API to set beacon feature, pass true to enable this feature,  by default Beacon feature is disabled 
	 * Call this API before Init()
	 * 
	 * @param bool enable
	 */
	public static void SetBeaconSupport (bool enable)
	{
		#if UNITY_ANDROID
		INSTANCE.Call ("shSetBeaconSupport", enable);
		#endif
	}

	/**
	 * Returns true if Streethawk's beacon is enabled in your code else returns false
	 * 
	 * @returns bool - true when enabled and false when disabled.
	 */
	public static bool isUseBeacons ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return false;
		#elif UNITY_ANDROID
		return INSTANCE.Call<bool> ("isUseBeacons");
		#elif UNITY_EDITOR
		return false;
		#endif
	}
	

	/**
	 * Call this API from OnApplicationPause() function from any script derived from MonoBehaviour Class
	 * 
	 */
	public static void ActivityResumed ()
	{
		#if UNITY_ANDROID
		INSTANCE.Call ("shActivityResumed", CurrentActivity);
		#endif
	}
	
	/**
	 * Call this API from OnApplicationPause() function from any script derived from MonoBehaviour Class
	 * 
	 */
	public static void ActivityPaused ()
	{
		#if UNITY_ANDROID
		INSTANCE.Call ("shActivityPaused", CurrentActivity);
		#endif
	}


	/**
	 * Call this API to set the Location.
	 * 
	 * @param double latitude
	 * @param double longitude
	 */
	public static void SetLocation (double latitude, double longitude)
	{
		#if UNITY_ANDROID
		INSTANCE.Call ("shSetLocation", latitude, longitude);
		#endif
	}


	public static void SetItunesId (string itunesId)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_setItunesId (itunesId);
		#endif
	}
	
	public static bool IsLogEnabled ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _isLogEnabled ();
		#elif UNITY_ANDROID
		return false;
		#elif UNITY_EDITOR
		return false;
		#endif
	}
	
	public static void SetEnableLogs (bool enable)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_setEnableLogs (enable);
		#endif
	}

	/**
	 * API to current formatted dateTime in UTC
	 * 
	 * @returns string UTC Time
	 */
	public static string getCurrentFormattedDateTime ()
	{
		DateTime time = DateTime.Now;           
		string format = "yyyy-MM-dd HH:mm:ss";   
		return time.ToString (format);
	}


	/**
	 * Initialise Streethawk SDK
	 */
	public static void Init (string appKey, string SenderID, string iTunesId)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_registerInstallForApp (appKey, true, iTunesId);
		#elif UNITY_ANDROID
		INSTANCE.Call ("setGcmSenderId", SenderID);
		INSTANCE.Call ("init", CurrentApplication);
		#endif
	}
	
	
	/**
	 * API to disable push notification feature pass false to disable this features. By default push notifications are enabled
	 * 
	 * @param bool enable
	 */
	public static void SetPushNotificationSupport (bool enable)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_setPushNotificationEnabled (enable);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shSetPushNotificationSupport", enable);
		#endif
	}
	

	
	/**
	 * API to query status of push notifications.
	 * 
	 * @returns bool
	 */
	public static bool isPushNotificationsEnabled ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _isPushNotificationEnabled ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<bool> ("isPushNotificationsEnabled");
		#elif UNITY_EDITOR
		return false;
		#endif
	}


	/**
	 * API to numeric tag a profile
	 * 
	 * @param string key
	 * @param double numeric_Value
	 *	 
	 * @returns bool - true for success false if failed 
	 */
	public static void tagNumeric (string key, double numeric_Value)
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
	public static void tagString (string key, string string_Value)
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
	public static void tagDateTime (string key, string datetime_Value)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_tagDateTime (datetime_Value, key);
		#elif UNITY_ANDROID
		INSTANCE.Call<bool> ("tagDatetime", key, datetime_Value);
		#endif	
	}

	/**
	 * Api to delete a custom profile tag
	 * 
	 * @param string key
	 */
	public static void removeTag (string key)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_removeTag (key);
		#elif UNITY_ANDROID
		INSTANCE.Call ("removeTag", key);
		#endif
				
	}

	/**
	 * Api to increment a tag
	 * 
	 * @param string key
	 */
	public static void incrementTag (string key)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_incrementTag (key);
		#elif UNITY_ANDROID
		INSTANCE.Call ("incrementTag", key);
		#endif
	}


	/**
	 * API to set location feature. Set false to disable location , true to resume location reporting by application by default locations feature is enabled
	 * Call this API before Init()
	 * 
	 * @param bool enable
	 */
	public static void SetLocationSupport (bool enable)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_setIsLocationServiceEnabled (enable);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shSetLocationSupport", enable);
		#endif
				
	}

	/**
	 * Returns true if Streethawk's location is enabled in your code else returns false
	 * 
	 * @returns bool - true when enabled and false when disabled.
	 */
	public static bool isUseLocation ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _isLocationSupportEnabled ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<bool> ("isUseLocation");
		#elif UNITY_EDITOR
		return false;
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
	* @param double distance - for android, distance of beacon from device
	*	 
	* @returns int - true for successful reporting, false for error, Check log messages for details
	*/
	public static bool EnterBeacon (string UUID, int major, int minor, double distance)
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
	public static bool ExitBeacon (string UUID, int major, int minor)
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
	 * API to get version of StreetHawk Library
	 * 
	 * @returns string - version of Streethawk Library
	 */
	public static string getSHLibraryVersion ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _getSHLibraryVersion ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<string> ("getSHLibraryVersion");
		#elif UNITY_EDITOR
		return null;
		#endif
				
	}

	/** 
	 * GetAlertSettings returns the time remaining in minutes before pause minutes set in AlertSetting(int) expires
	 *	 
	 * @returns int - time remaining before alerts will be enabled again
	 */
	public static int GetAlertSettings ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _getAlertSettingMinutes ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<int> ("shGetAlertSettings");
		#elif UNITY_EDITOR
		return 0;
		#endif
				
	}

	/**
	 * call AlertSetting() for pausing push messages for given duration of time in minutes.
	 * 
	 * @param int pauseMinutes
	 */
	public static void SetAlertSettings (int pauseMinutes)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_setAlertSetting (pauseMinutes);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shAlertSetting", pauseMinutes);
		#endif
	}

	/**
	 * Api to send simple feedback to server.
	 * 
	 * @param string title
	 * @param string message
	 */
	public static void SendSimpleFeedback (string title, string message)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_sendSimpleFedback (title, message);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shSendSimpleFeedback", title, message);
		#endif
				
	}
	
	/**
	 * Use this API to notify streethawk when user enters a new page.
	 * App developer is expected to call this API for all the pages whose analytics is of interest to him.
	 * 
	 * @param string page_name
	 */
	public static void NotifyPageEntered (string page_name)
	{
		#if UNITY_IPHONE || UNITY_IOS
		_shNotifyPageEnter (page_name);
		#elif UNITY_ANDROID
		INSTANCE.Call ("shNotifyPageEntered", page_name);
		#endif
				
	}

	/**
	 * This API returns name of the page, if any requested by streethawk server, when app is in BG.
	 * 
	 * @returns string - app page to be displayed else returns null
	 */
	public static string getAppPage ()
	{
		#if UNITY_IPHONE || UNITY_IOS
		return _shGetViewName ();
		#elif UNITY_ANDROID
		return INSTANCE.Call<string> ("getAppPage");
		#elif UNITY_EDITOR
		return null;
		#endif
				
	}
}
