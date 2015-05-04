#import <StreetHawkCore/ISHCustomiseHandler.h>
#import <StreetHawkCore/ISHPhonegapObserver.h>
#import <Foundation/Foundation.h>
#import <StreetHawkCore/StreetHawkCore.h>
#import "UnityAppController.h"

@interface JSONHandler : NSObject <ISHCustomiseHandler>

- (void)shRawJsonCallbackWithTitle:(NSString *)title withMessage:(NSString *)message withJson:(NSString *)json;

@end

@interface ActivityCallBack : NSObject <ISHPhonegapObserver>

- (void)shPGDisplayHtmlFileName:(NSString *)html_fileName;

@end

extern "C"
{
    void _registerInstallForApp(const char * appKey,bool isDebugMode, const char * iTunesId);
    void _tagNumeric(double value,const char * key);
    void _tagString(const char * value,const char * key);
    void _tagDateTime(const char * date,const char * key);
    void _incrementTag(const char * keyToIncrement);
    void _removeTag(const char * keyToRemove);
    void _shSetiTunesId(const char * itunesId);
    void _shSetEnableLogs(bool debugMode);
    void _shSetIsPushNotificationEnabled(bool isEnable);
    void _shSetDefaultPushNotificationSupport(bool isEnable);
    void _shSetDefaultLocationSupport(bool isEnable);
    void _shSetIsUseLocation(bool isEnable);
    void _shSetAlertSetting(int minutes);
    void _shSendSimpleFeedback(const char * title,const char * message);
    void _shCustomActivityList (const char * friendlyName[],const char * vc[],const char * xib_iphone[],const char * xib_ipad[], int count);
    void _shNotifyPageEnter(const char * name);
   
    bool _shEnableLogs();
    bool _shIsUseLocation();
    bool _shIsPushNotificationEnabled();
    
    int  _shAlertSettings();
    
    char* _getSHLibraryVersion();
    char* _shGetViewName();
    char* _shiTunesId();
}

NSString* CreateNSString(const char* string);
char* cStringCopy(const char* string);


