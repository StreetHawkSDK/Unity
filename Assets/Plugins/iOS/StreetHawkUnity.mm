#import "StreetHawkUnity.h"

@implementation JSONHandler

- (void)shRawJsonCallbackWithTitle:(NSString *)title withMessage:(NSString *)message withJson:(NSString *)json {
    UnitySendMessage("StreetHawk", "shRawJsonCallbackEvent", cStringCopy([json UTF8String]));
}

@end

@implementation ActivityCallBack

- (void)shPGDisplayHtmlFileName:(NSString *)html_fileName{
    UnitySendMessage("StreetHawk", "shNotifyAppPageEvent", cStringCopy([html_fileName UTF8String]));
}

@end


extern "C"{
    
    void _registerInstallForApp(const char * appKey,bool isDebugMode, const char * iTunesId)
    {
        [StreetHawk registerInstallForApp:CreateNSString(appKey) withDebugMode:isDebugMode?YES:NO withiTunesId:CreateNSString(iTunesId)];
        
        JSONHandler *handler = [[JSONHandler alloc] init];
        [StreetHawk shSetCustomiseHandler:handler];
        
        ActivityCallBack *callBack = [[ActivityCallBack alloc] init];
        [StreetHawk shPGHtmlReceiver:callBack];
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
        NSString *dateString = CreateNSString(date);
        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
        [dateFormatter setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
        NSDate *dateFromString = [[NSDate alloc] init];
        dateFromString = [dateFormatter dateFromString:dateString];
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
    void _shSetiTunesId(const char * _itunesId)
    {
        StreetHawk.itunesAppId = CreateNSString(_itunesId);
    }
    void _shSetEnableLogs(bool debugMode)
    {
        StreetHawk.isDebugMode = debugMode?YES:NO;
    }
    void _shSetIsPushNotificationEnabled(bool isEnable)
    {
        StreetHawk.isNotificationEnabled = isEnable?YES:NO;
    }
    void _shSetDefaultPushNotificationSupport(bool isEnable)
    {
        StreetHawk.isDefaultNotificationEnabled = isEnable?YES:NO;
    }
    void _shSetDefaultLocationSupport(bool isEnable)
    {
        StreetHawk.isDefaultLocationServiceEnabled = isEnable?YES:NO;
    }
    void _shSetIsUseLocation(bool isEnable)
    {
        StreetHawk.isLocationServiceEnabled = isEnable?YES:NO;
    }
    char* _shiTunesId()
    {
        return cStringCopy([StreetHawk.itunesAppId UTF8String]);
    }
    char* _getSHLibraryVersion()
    {
        return cStringCopy([StreetHawk.version UTF8String]);
    }
    char* _shGetViewName()
    {
        return cStringCopy([[StreetHawk shGetViewName] UTF8String]);
    }
    bool _shEnableLogs()
    {
        return StreetHawk.isDebugMode?true:false;
    }
    bool _shIsUseLocation()
    {
        return StreetHawk.isLocationServiceEnabled;
    }
    bool _shIsPushNotificationEnabled()
    {
        return StreetHawk.isNotificationEnabled;
    }
    void _shSetAlertSetting(int minutes)
    {
        [StreetHawk shSetAlertSetting:(NSInteger)minutes finish:nil];
    }
    int _shAlertSettings()
    {
        return (int)[StreetHawk getAlertSettingMinutes];
    }
    void _shSendSimpleFeedback(const char * title,const char * message)
    {
        [StreetHawk shFeedback:nil needInputDialog:NO needConfirmDialog:YES withTitle:CreateNSString(title) withMessage:CreateNSString(message)  withPushMsgid:0 withPushData:nil];
    }
    void _shNotifyPageEnter(const char * name)
    {
        [StreetHawk shNotifyPageEnter:CreateNSString(name)];
    }
    void _shCustomActivityList (const char * friendlyName[],const char * vc[],const char * xib_iphone[],const char * xib_ipad[], int count)
    {
        NSMutableArray* activitList = [[NSMutableArray alloc] init];
        for (int i =0; i<count; i++) {
            SHFriendlyNameObject *name = [[SHFriendlyNameObject alloc] init];
            NSString *_friendlyName = CreateNSString(friendlyName[i]);
            NSString *_vc = CreateNSString(vc[i]);
            NSString *_xib_iphone = CreateNSString(xib_iphone[i]);
            NSString *_xib_ipad = CreateNSString(xib_ipad[i]);
            if([_friendlyName length]>0)
                name.friendlyName = _friendlyName;
            if([_vc length]>0)
                name.vc = _vc;
            if([_xib_iphone length]>0)
                name.xib_iphone = _xib_iphone;
            if([_xib_ipad length]>0)
                name.xib_ipad = _xib_ipad;
            
            [activitList addObject:name];
            
        }
        NSArray *array = [activitList copy];
        
        [StreetHawk shCustomActivityList: array];
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