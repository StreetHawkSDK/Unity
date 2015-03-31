using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
	private static extern void _streetHawkInit ();
	
	[DllImport ("__Internal")]
	private static extern void _setItunesId (string itunesId);
	
	[DllImport ("__Internal")]
	private static extern void _setPushNotificationSenderId (string senderId_OR_appKey);
	
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
	private static extern void _setAlertSetting(int minutes);
	
	[DllImport ("__Internal")]
	private static extern int _getAlertSettingMinutes();
	
	
	[DllImport ("__Internal")]
	private static extern string _getItunesId ();
	
	[DllImport ("__Internal")]
	private static extern bool _isLogEnabled ();
	
	[DllImport ("__Internal")]
	private static extern bool _isLocationSupportEnabled ();
	
	[DllImport ("__Internal")]
	private static extern bool _isPushNotificationEnabled ();
	
	[DllImport ("__Internal")]
	private static extern bool _locationServiceEnabledForApp(bool allowNotDetermined);
	
	[DllImport ("__Internal")]
	private static extern bool _enterBeacon(string UUID,double majorNumber,double minorNumber,string identifier);
	
	[DllImport ("__Internal")]
	private static extern void _exitBeacon(string UUID,double majorNumber,double minorNumber,string identifier);

	[DllImport ("__Internal")]
	private static extern string _getSHLibraryVersion();

	[DllImport ("__Internal")]
	private static extern void _shNotifyPageEnter(string name);

	[DllImport ("__Internal")]
	private static extern void _shSetManualLocationAtLatitude(double latitude,double longitude);

	[DllImport ("__Internal")]
	private static extern void _shNotifyPageExit(string name);

	[DllImport ("__Internal")]
	private static extern string _shGetViewName();

	[DllImport ("__Internal")]
	private static extern void _sendSimpleFedback(string title,string message);


	public static void SetItunesId (string itunesId)
	{
		_setItunesId (itunesId);
	}

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



		/**
		 * Get Path Segments
		 * 
		 * @param string param
	 	 */
		public static List<string> GetPathSegments ()
		{
				List<string> param = new List<string> ();
				int size;
				try {
						using (AndroidJavaObject Intent = CurrentActivity.Call<AndroidJavaObject>("getIntent")) {
								using (AndroidJavaObject Data = Intent.Call<AndroidJavaObject>("getData")) {
										using (AndroidJavaObject list = Data.Call<AndroidJavaObject> ("getPathSegments")) {
												size = list.Call<int> ("size"); 
												for (int i = 0; i < size; i++) {
														param.Add (list.Call<string> ("get", i));
												}
										}
								}
						}
				} catch (Exception e) {
						Debug.Log ("Exception " + e.Message);
						
				}
				return param;
		}

		/**
		 * Get Intent Data
		 * 
		 * @param string param
	 	 */
		public static string GetQueryParameter (string param)
		{
				string value = "";
				try {

						//getIntent().getData().getQueryParameter("param1");
						using (AndroidJavaObject Intent = CurrentActivity.Call<AndroidJavaObject>("getIntent")) {
								//	System.IntPtr method_getData = AndroidJNIHelper.GetMethodID (Intent.GetRawClass (), "getData");
								using (AndroidJavaObject Data = Intent.Call<AndroidJavaObject>("getData")) {
										value = Data.Call<string> ("getQueryParameter", param); 
								}
						}
				} catch (Exception e) {
						value = "";
				}
				return value;
		}

		/**
	 * Use this API for check if streethawk is initialized especially in non native platforms returns true is Streethawk is initialized else return false
	 * 
	 * @returns bool 
	 */
		public static bool isStreethawkInitialized ()
		{
				return StreetHawkClass.Call<bool> ("isStreethawkInitialized");
		}


		/**
	 * API to set application specific sender key for push notifications.
	 * 
	 * @param string SenderID - Project Number
	 */ 
		public static void SetGcmSenderId (string SenderID)
		{
				INSTANCE.Call ("setGcmSenderId", SenderID);
		}

		/**
	 * Call this API for submitting map of friendly names and fully qualified names to Streethawk library.
	 * 
	 * @param Dictionary<string, string> _ActivityList
	 */
		public static void CustomActivityList (Dictionary<string, string> _ActivityList)
		{
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
		}


		/**
	 * API to set beacon feature, pass true to enable this feature,  by default Beacon feature is disabled 
	 * Call this API before Init()
	 * 
	 * @param bool enable
	 */
		public static void SetBeaconSupport (bool enable)
		{
				INSTANCE.Call ("shSetBeaconSupport", enable);
		}

		/**
	 * Returns true if Streethawk's beacon is enabled in your code else returns false
	 * 
	 * @returns bool - true when enabled and false when disabled.
	 */
		public static bool isUseBeacons ()
		{
				return INSTANCE.Call<bool> ("isUseBeacons");
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
	* @returns int - 0 for successful reporting -1 for error, Check log messages for details
	*/
		public static int EnterBeacon (string UUID, int major, int minor, double distance)
		{
				return INSTANCE.Call <int> ("shEnterBeacon", CurrentActivity, UUID, major, minor, distance);
		}

		/**
	* API to notify streethawk server when device exits a beacon region
	* Use this API if you are using third party library support for beacons
	* 
	* @param string UUID - UUID of the beacon detected
	* @param int major - major number of beacon detected
	* @param int minor - minor number of beacon detected
	*	 
	* @returns int - 0 for successful reporting -1 for error, Check log messages for details
	*/
		public static int ExitBeacon (string UUID, int major, int minor)
		{
				return INSTANCE.Call <int> ("shExitBeacon", CurrentActivity, UUID, major, minor);
		}

		/**
	 * Call this API from OnApplicationPause() function from any script derived from MonoBehaviour Class
	 * 
	 */
		public static void ActivityResumed ()
		{
				INSTANCE.Call ("shActivityResumed", CurrentActivity);
		}
	
		/**
	 * Call this API from OnApplicationPause() function from any script derived from MonoBehaviour Class
	 * 
	 */
		public static void ActivityPaused ()
		{
				INSTANCE.Call ("shActivityPaused", CurrentActivity);
		}


		/**
	 * Call this API to set the Location.
	 * 
	 * @param double latitude
	 * @param double longitude
	 */
		public static void SetLocation (double latitude, double longitude)
		{
				INSTANCE.Call ("shSetLocation", latitude, longitude);
		}
#endif

	
		public static string GetUTCTime ()
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
	 * API to disable push notification feature pass false to disable this features permanently. By default push notifications are enabled
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
		public static bool tagNumeric (string key, double numeric_Value)
		{
				#if UNITY_IPHONE || UNITY_IOS
				_tagNumeric (numeric_Value, key);
				#elif UNITY_ANDROID
				return INSTANCE.Call<bool> ("tagNumeric", key, numeric_Value);
				#elif UNITY_EDITOR
				return false;
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
		public static bool tagString (string key, string string_Value)
		{
				#if UNITY_IPHONE || UNITY_IOS
				_tagString (string_Value, key);
				#elif UNITY_ANDROID
				return INSTANCE.Call<bool> ("tagString", key, string_Value);
				#elif UNITY_EDITOR
				return false;
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
		public static bool tagDateTime (string key, string datetime_Value)
		{
				#if UNITY_IPHONE || UNITY_IOS
				_tagDateTime (datetime_Value, key);
				#elif UNITY_ANDROID
				return INSTANCE.Call<bool> ("tagDatetime", key, datetime_Value);
				#elif UNITY_EDITOR
				return false;
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
	 * API to current formatted dateTime in UTC
	 * 
	 * @returns string UTC Time
	 */ 
		public static string getCurrentFormattedDateTime ()
		{
				return StreetHawkClass.CallStatic<string> ("getCurrentFormattedDateTime");
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
				_sendSimpleFedback(title,message);
				#elif UNITY_ANDROID
				INSTANCE.Call ("shSendSimpleFeedback", title, message);
				#endif
				
		}
	

		/**
	 * Call this API to set the Location manually.
	 * 
	 * @param double latitude
	 * @param double longitude
	 */
		public static void SetManualLocation (double latitude, double longitude)
		{
				#if UNITY_IPHONE || UNITY_IOS
				_shSetManualLocationAtLatitude( latitude, longitude);
				#elif UNITY_ANDROID
				INSTANCE.Call ("shSetManualLocation", latitude, longitude);
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
		_shNotifyPageEnter( page_name);
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
		return _shGetViewName();
				#elif UNITY_ANDROID
				return INSTANCE.Call<string> ("getAppPage");
				#elif UNITY_EDITOR
		return null;
				#endif
				
		}
}
