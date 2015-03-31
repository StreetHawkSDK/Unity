

extern "C" {
    void _registerInstallForApp(const char * filename,bool isDebugMode, const char * iTunesId);
    
    void _tagNumeric(double value,const char * key);
    void _tagString(const char * value,const char * key);
    void _tagDateTime(const char * date,const char * key);
    void _incrementTag(const char * keyToIncrement);
    void _removeTag(const char * keyToRemove);
    
    void _streetHawkInit();
    
    void _setItunesId(const char * itunesId);
    void _setPushNotificationSenderId(const char * senderId_OR_appKey);
    void _setEnableLogs(bool debugMode);
    void _setPushNotificationEnabled(bool isEnable);
    void _setDefaultPushNotificationEnabled(bool isEnable);
    void _setIsDefaultLocationServiceEnabled(bool isEnable);
    void _setIsLocationServiceEnabled(bool isEnable);
    void _setAlertSetting(int minutes);
    
    char* _getItunesId();
    bool _isLogEnabled();
    bool _isLocationSupportEnabled();
    bool _isPushNotificationEnabled();
    
    
    bool _locationServiceEnabledForApp(bool allowNotDetermined);
    bool _enterBeacon(const char * UUID,double majorNumber,double minorNumber,const char * identifier);
    void _exitBeacon(const char * UUID,double majorNumber,double minorNumber,const char * identifier);
	void _shSetManualLocationAtLatitude(double latitude,double longitude);
    int _getAlertSettingMinutes();
	char* _getSHLibraryVersion();
	void _shNotifyPageEnter(const char * name);
    void _shNotifyPageExit(const char * name); 
    void _sendSimpleFedback(const char * title,const char * message);	
    
    
}

NSString* CreateNSString(const char* string);
char* cStringCopy(const char* string);


