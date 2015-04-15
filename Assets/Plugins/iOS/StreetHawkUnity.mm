#import "StreetHawkUnity.h"
#import <Foundation/Foundation.h>
#import <StreetHawkCore/StreetHawkCore.h>// your actual iOS library header

extern "C"{
    
    void _registerInstallForApp(const char * appKey,bool isDebugMode, const char * iTunesId)
    {
        [StreetHawk registerInstallForApp:CreateNSString(appKey) withDebugMode:isDebugMode?YES:NO withiTunesId:CreateNSString(iTunesId)];
    }
    void _tagNumeric(double value,const char * key)
    {
        [StreetHawk tagNumeric:value forKey:CreateNSString(key)];
    }
    void _tagString(const char * value,const char * key)
    {
        [StreetHawk tagString:CreateNSString(value) forKey:CreateNSString(key)];
    }
    void _tagDateTime(const char * date,const char * key)
    {
        NSDate *dateFromString = parseDate(CreateNSString(date), 0);
        [StreetHawk tagDatetime:dateFromString forKey:CreateNSString(key)];
    }
    void _incrementTag(const char * keyToIncrement)
    {
        [StreetHawk incrementTag:CreateNSString(keyToIncrement)];
    }
    void _removeTag(const char * keyToRemove)
    {
        [StreetHawk removeTag:CreateNSString(keyToRemove)];
    }
    void _streetHawkInit()
    {
        [StreetHawk registerInstallForApp:StreetHawk.appKey withDebugMode:StreetHawk.isDebugMode withiTunesId:StreetHawk.itunesAppId];
    }
    void _setItunesId(const char * _itunesId)
    {
        StreetHawk.itunesAppId = CreateNSString(_itunesId);
    }
    void _setPushNotificationSenderId(const char * senderId_OR_appKey)
    {
        //StreetHawk.appKey = CreateNSString(senderId_OR_appKey);
    }
    void _setEnableLogs(bool debugMode)
    {
        StreetHawk.isDebugMode = debugMode?YES:NO;
    }
    void _setPushNotificationEnabled(bool isEnable)
    {
        StreetHawk.isNotificationEnabled = isEnable?YES:NO;
    }
    void _setDefaultPushNotificationEnabled(bool isEnable)
    {
        StreetHawk.isDefaultNotificationEnabled = isEnable?YES:NO;
    }
    void _setIsDefaultLocationServiceEnabled(bool isEnable)
    {
        StreetHawk.isDefaultLocationServiceEnabled = isEnable?YES:NO;
    }
    void _setIsLocationServiceEnabled(bool isEnable)
    {
        StreetHawk.isLocationServiceEnabled = isEnable?YES:NO;
    }
    char* _getItunesId()
    {
        return cStringCopy([StreetHawk.itunesAppId UTF8String]);
    }
	char* _getSHLibraryVersion()
    {
        return cStringCopy([StreetHawk.version UTF8String]);
    }
	char* _getshGetViewName()
    {
        return cStringCopy([[StreetHawk shGetViewName] UTF8String]);
    }
    bool _isLogEnabled()
    {
        return StreetHawk.isDebugMode?true:false;
    }
    bool _isLocationSupportEnabled()
    {
        return StreetHawk.isLocationServiceEnabled;
    }
    bool _isPushNotificationEnabled()
    {
        return StreetHawk.isNotificationEnabled;
    }
    bool _locationServiceEnabledForApp(bool allowNotDetermined)
    {
        return [SHLocationManager locationServiceEnabledForApp:allowNotDetermined?YES:NO]?true:false;
    }
    void _setAlertSetting(int minutes)
    {
        [StreetHawk shSetAlertSetting:(NSInteger)minutes finish:nil];
    }
    int _getAlertSettingMinutes()
    {
        return (int)[StreetHawk getAlertSettingMinutes];
    }	
	void _shNotifyPageEnter(const char * name)
	{
        [StreetHawk shNotifyPageEnter:CreateNSString(name)];
	}
    void _shNotifyPageExit(const char * name)
	{
        [StreetHawk shNotifyPageExit:CreateNSString(name)];
	}
	void _sendSimpleFedback(const char * title,const char * message)
	{
        [StreetHawk shFeedback:nil needInputDialog:NO needConfirmDialog:YES withTitle:CreateNSString(title) withMessage:CreateNSString(message)  withPushMsgid:0 withPushData:nil];
	}
}


NSString* CreateNSString(const char* string)
{
    if (string)
        return [NSString stringWithUTF8String:string];
    else
        return [NSString stringWithUTF8String:""];
}
char* cStringCopy(const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}