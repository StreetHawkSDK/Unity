/*
 * Copyright 2012 StreetHawk
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Created by Christine XYS. 
 */

#import "StreetHawkPublicFwds.h"
#import "StreetHawkUtils.h" //for enum SHDevelopmentPlatform
#import "StreetHawkFeedObject.h" //for SHNewFeedsHandler and SHFeedsFetchHandler

/**
 Callback once open url app delegate triggers.
 */
typedef void (^SHOpenUrlHandler)(NSURL *openUrl);

/**
 Singleton to access SHApp.
 */
#define StreetHawk          [SHApp sharedInstance]

/**
 The SHApp Class is core of whole SDK. It contains almost all the functions.
 
 **Normal usage:**
 
 - SHApp is singleton, access it by `[SHApp sharedInstance]` or `StreetHawk`.
 
 - When your Application starts, usually in `- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions`, call  `registerInstallForApp:withDebugMode:withiTunesId:` to initialize all required StreetHawk features.
 
     This is simple, here are a few lines of sample code: 
 
        [StreetHawk registerInstallForApp:appId(registered with the StreetHawk Cloud) withDebugMode:Yes withiTunesId:@"123456"(string id register in AppStore)];
 
 - Add tag for user is simple, code like:
 
        [StreetHawk tagString:@"a@a.com" forKey:@"sh_email"];
 
 - Submit friendly names, code like:
 
        SHFriendlyNameObject *name1 = [[SHFriendlyNameObject alloc] init];
        SHFriendlyNameObject *name2 = [[SHFriendlyNameObject alloc] init];
        SHFriendlyNameObject *name3 = [[SHFriendlyNameObject alloc] init];
        name1.friendlyName = FRIENDLYNAME_LOGIN;
        name1.vc = @"LogTagCasesViewController";
        name2.friendlyName = FRIENDLYNAME_REGISTER;
        name2.vc = @"LogTagCasesViewController";
        name3.friendlyName = @"Input iBeacon region";
        name3.vc = @"BeaconRegionInputViewController";
        name3.xib_iphone = @"BeaconRegionInputViewController";
        [StreetHawk shCustomActivityList:@[name1, name2, name3]];
 
 **Necessary project settings:**
 
 To have your StreetHawk-enabled Application work properly, some project settings are necessary. 
 
 - Add "-ObjC" into "Other Linker Flags". It's necessary for using SDK otherwise link error may occur for category methods.
 - Add resource bundle into project. It's necessary for SDK's xib and image resources.
 - Add "Background fetch" in "Background Modes", since iOS 7.
 - Add "NSLocationAlwaysUsageDescription" or "NSLocationWhenInUseUsageDescription" in Info.plist for enabling location service since iOS 8.
 - Because StreetHawk SDK uses significant location change at background, there is NO need to add "Required background modes" with *location service*.
 
 */
@interface SHApp : NSObject<UIApplicationDelegate>

/** @name Create and initialize */

/**
 Singleton creator of SHApp. Normally use `StreetHawk` to represent `[SHApp sharedInstance]`.
 @return Singleton SHApp instance.
 */
+ (SHApp *)sharedInstance;

/**
 Initialize for an Application, setting up the environment.
 @param appKey The global name of the app. This is registered in StreetHawk server, once registered it cannot change.
 @param isDebugMode The mode of whether print NSLog in Xcode console.
 @param iTunesId The App id after register in iTunes, for example @"337064413". It used for rating App, if this id is not setup, rating dialog will not promote.
 */
- (void)registerInstallForApp:(NSString *)appKey withDebugMode:(BOOL)isDebugMode withiTunesId:(NSString *)iTunesId;

/** @name Global properties */

/**
 The allocated name or code for this app as set in the StreetHawk Cloud, for example "SHSheridan1". It's mandatory for an Application to work.
 */
@property (nonatomic, readonly, weak) NSString *appKey;

/**
 Set up default host url, it's the starting url. Normally customers does not need to set it, and use "https://api.streethawk.com" as starting url. Some test App can set it be "https://api-dev.streethawk.com". It's only used for initialise, set this before call `registerInstallForApp:withDebugMode:withiTunesId:`.
 */
- (void)setDefaultStartingUrl:(NSString *)defaultUrl;

/**
 The current alive host url. It can be switched to other host at runtime by app_status. This is readonly function return the local cached alive host root url, if it's empty return default one `hostDefaultRootUrl`. Get function contains version, for example @"https://api.streethawk.com/V1". Use `makeBaseUrlString(StreetHawk.hostAliveRootUrl, @"install/details/")` to create request path.
 * Set: if set nil or empty or same host url, nothing happen; otherwise the alive host root url is changed to new one. Set function should NOT contain version, just be @"https://api.streethawk.com".
 */
@property (nonatomic, readonly, weak) NSString *hostAliveRootUrl;

/**
 Decide whether need to show debug log in console.
 */
@property (nonatomic) BOOL isDebugMode;

/**
 StreetHawk uses PLCrashReport to collect App's crash report and upload to StreetHawk server when next launch the App. You can check crash report on web site. It's enabled by default.
 */
@property (nonatomic) BOOL isEnableCrashReport;

/**
 The App id after register in iTunes, for example @"337064413". It used for rating App, if this id is not setup, rating dialog will not promote.
 */
@property (nonatomic, strong) NSString *itunesAppId;

/**
The application version and build version of current Application, formatted as @"[CFBundleShortVersionString] ([CFBundleVersion])", for example @"1.2.7 (10)". This is version for Application project, not for SDK. Use `version` to get StreetHawkCore.framework SDK version.
*/
@property (nonatomic, strong, readonly) NSString *clientVersion;

/**
 Version for StreetHawkCore.framework SDK, formatted as [server api version] / [framework version] (X.Y.Z), for example "1/1.2.5". In Finder [framework version] is visible by view framework file info.
 */
@property (nonatomic, strong, readonly) NSString *version;

/**
 Before successfully install, it's nil. After install once, it's the install instance.
 */
@property (nonatomic, strong) SHInstall *currentInstall;

/**
 Make sure installs/register or installs/update happen in sequence.
 */
@property (nonatomic, strong) dispatch_semaphore_t install_semaphore;

/**
 A flag to indicate whether current is uploading logs. 
 */
@property (nonatomic) BOOL uploading_log;

/**
 An enum for current App's development platform, refer to `SHDevelopmentPlatform` for supporting platforms. This is only used internally, and setup by Phonegap plugin, Titanium module, Xamarin binding etc. Normal customer does not need to change it.
 */
@property (nonatomic, readonly) SHDevelopmentPlatform developmentPlatform;

/**
 StreetHawk requires AppDelegate has some common functions, if `autoIntegrateAppDelegate` is YES (by default), customer App does not need to manually implement any of the push-related UIApplicationDelegate protocol methods or pass notifications to the library. The library is able to do this by setting itself as the app delegate, intercepting messages and forwarding them to your original app delegate. This must be setup before register install. It's YES by default but if custome App set it to NO, customer App must implement these functions manually:
 
 `- (void)application:(UIApplication *)application didRegisterUserNotificationSettings:(UIUserNotificationSettings *)notificationSettings  //since iOS 8.0
 {
    [StreetHawk handleUserNotificationSettings:notificationSettings];
 }
 
 - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken
 {
    [StreetHawk setApnsDeviceToken:deviceToken];
 }
 
 - (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo
 {
    [StreetHawk handleRemoteNotification:userInfo];
 }
 
 - (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler
 {
    [StreetHawk handleRemoteNotification:userInfo needComplete:YES fetchCompletionHandler:completionHandler];
 }
 
 - (void)application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forRemoteNotification:(NSDictionary *)userInfo completionHandler:(void (^)())completionHandler
 {
    [StreetHawk handleRemoteNotification:userInfo withActionId:identifier needComplete:YES completionHandler:completionHandler];
 }

 - (void)application:(UIApplication *)application didReceiveLocalNotification:(UILocalNotification *)notification
 {
    [StreetHawk handleLocalNotification:notification];
 }
 
 - (void)application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forLocalNotification:(UILocalNotification *)notification completionHandler:(void (^)())completionHandler
 {
    [StreetHawk handleLocalNotification:notification withActionId:identifier needComplete:YES completionHandler:completionHandler];
 }

 - (void)application:(UIApplication *)application performFetchWithCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler
 {
    [StreetHawk shBackgroundTask:completionHandler];
 }
 
 - (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
 {
    return [StreetHawk openURL:url sourceApplication:sourceApplication annotation:annotation];
 }`
 */
@property (nonatomic) BOOL autoIntegrateAppDelegate;

/** @name Location Service */

/**
 Default value to initialise `isLocationServiceEnabled`, it's called once when App first launch to set to `isLocationServiceEnabled`. A typical usage is to delay asking for location allow permission (*** would like to use your current location (Don't allow/OK)):
 
    `StreetHawk.isDefaultLocationServiceEnabled = NO; //not trigger location service when App launch.`
    `[registerInstallForApp... ];   //do register without trigger location service`
    `StreetHawk.isLocationServiceEnabled = YES; //later trigger location service when need it.`
 */
@property (nonatomic) BOOL isDefaultLocationServiceEnabled;

/**
 Property to control using location service or not. Geo-location update, iBeacon, region update needs this to be enabled to work. Internal CLLocation is not released when disable location service, but all functions not trigger StreetHawk's notification.
 */
@property (nonatomic) BOOL isLocationServiceEnabled;

/** @name Notification */

/**
 Default value to initialise `isNotificationEnabled`, it's called once when App first launch to set to `isNotificationEnabled`. A typical usage is to delay asking for notification permission:
 
    `StreetHawk.isDefaultNotificationEnabled = NO; //not trigger remote/local notification when App launch.`
    `[registerInstallForApp... ];   //do register, it will not register notification.`
    `StreetHawk.isNotificationEnabled = YES; //later trigger remote/local notification when need it.`
 */
@property (nonatomic) BOOL isDefaultNotificationEnabled;

/**
 Property to control enabling remote/local notification.
 
 1. If user set `isDefaultNotificationEnabled = NO` before calling `registerInstallForApp...`, notification is not register and system permission dialog not promote.
 2. Call `registerInstallForApp... `, it will not register notification.
 3. Later when user wants to do register, set `isNotificationEnabled = YES` and system permission dialog promote.
 4. Step 1 is optional. If not manually set `isDefaultNotificationEnabled = NO`, it's YES by default and system permission dialog show when `registerInstallForApp` called at very first launch.
 5. Set `isNotificationEnabled = NO` again after `isNotificationEnabled = YES` makes StreetHawk server receive `revoked`, and cause StreetHawk server not send notification to client. But it does not call system unregisterForRemoteNotification, so this App can still receive remote notification from other way.
 */
@property (nonatomic) BOOL isNotificationEnabled;

/**
 Property to define what kind of types will display for notification.
 
 * Before iOS 8, it's combine of UIRemoteNotificationType, default value is UIRemoteNotificationTypeAlert|UIRemoteNotificationTypeBadge|UIRemoteNotificationTypeSound|UIRemoteNotificationTypeNewsstandContentAvailability. 
 * Since iOS 8, it's combine of UIUserNotificationType, default value is UIUserNotificationTypeAlert|UIUserNotificationTypeBadge|UIUserNotificationTypeSound.
 
 Customer is free to change the types, and make sure setting up this property before registering for remote notification, such as before calling [registerForInstall...].
 */
@property (nonatomic) NSUInteger notificationTypes;

/**
 Property for customer to add their own interactive notification. It's same as iOS 8 defined categories set. Code snippet:
 
    // Define an action for the category
    UIMutableUserNotificationAction *action = [[UIMutableUserNotificationAction alloc] init];
    action.destructive = NO;
    action.activationMode = UIUserNotificationActivationModeForeground;
    action.authenticationRequired = YES;
    action.title = @"Action!";
    action.identifier = @"custom_action";
    // Define the category
    UIMutableUserNotificationCategory *category = [[UIMutableUserNotificationCategory alloc] init];
    [category setActions:@[action] forContext:UIUserNotificationActionContextMinimal];
    [category setActions:@[action] forContext:UIUserNotificationActionContextDefault];
    category.identifier = @"custom_category";
    // Set the custom categories
    StreetHawk.notificationCategories = [NSSet setWithArray:@[category]];
 
 Customer is free to add their own categories, make sure categories' `identifier` is not same as StreetHawk's pre-defined code such as 8000, 8004 etc. In case customer's category uses same `identifier` as StreetHawk's predefined code, StreetHawk's function will be override. Customer's categories, combined with StreetHawk's predefined categories, work side by side. Customer needs to handle their own category by their own code, usually needs to implement their own AppDelegate functions, check document http://api.streethawk.com/v1/docs/ios-manualsetup.html#ios-manualsetup. Make sure setting up this property before registering for remote notification, such as before calling [registerForInstall...].
 */
@property (nonatomic, strong) NSSet *notificationCategories NS_AVAILABLE_IOS(8_0);

/**
 Handle user notification settings callback. Call this in customer App's UIApplicationDelegate if NOT auto-integrate. If `StreetHawk.autoIntegrateAppDelegate = YES;` make sure NOT call this otherwise cause dead loop. Code snippet:
 
    `- (void)application:(UIApplication *)application didRegisterUserNotificationSettings:(UIUserNotificationSettings *)notificationSettings`
    `{`
        `[StreetHawk handleUserNotificationSettings:notificationSettings];`
    `}`
 */
- (void)handleUserNotificationSettings:(UIUserNotificationSettings *)settings NS_AVAILABLE_IOS(8_0);

/**
 Set StreetHawk SDK the token assigned by Apple for push notification. Previous device token is cached in NSUserDefaults key "APNS_DEVICE_TOKEN". If current data is nil, it's ignored and previous used. When setting a different device token, an install update sent immediately for updating server's token. Call this in customer App's UIApplicationDelegate if NOT auto-integrate. If `StreetHawk.autoIntegrateAppDelegate = YES;` make sure NOT call this otherwise cause dead loop. Code snippet:
 
    `-(void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken`
    `{`
        `[StreetHawk setApnsDeviceToken:deviceToken];`
    `}`
 
 @param value Device token got from Apple's register remote notification.
 */
- (void)setApnsDeviceToken:(NSData *)value;

/**
 Get current saved apns device token as NSString. 
 */
- (NSString *)apnsDeviceToken;

/**
 Customer Application should implement this in UIApplicationDelegate to forward handling to StreetHawk library if NOT auto-integrate. If `StreetHawk.autoIntegrateAppDelegate = YES;` make sure NOT call this otherwise cause dead loop. Code snippet:
 
    `-(void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo
    {
        [StreetHawk handleRemoteNotification:userInfo needComplete:YES fetchCompletionHandler:nil];
    }`
 
    or
 
    `-(void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler
    {
        [StreetHawk handleRemoteNotification:userInfo needComplete:YES fetchCompletionHandler:completionHandler];
    }`
 
 @param userInfo Payload passed in by remote notification.
 @param needComplete Whether need to call `completionHandler` when task finish. If `completionHandler`=nil this does not matter YES or NO.
 @param completionHandler Pass in system's to finish when task is done.
 */
- (void)handleRemoteNotification:(NSDictionary *)userInfo needComplete:(BOOL)needComplete fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler;

/**
 Customer Application should implement this in UIApplicationDelegate to forward handling to StreetHawk library if NOT auto-integrate. If `StreetHawk.autoIntegrateAppDelegate = YES;` make sure NOT call this otherwise cause dead loop. Code snippet:
 
    `- (void)application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forRemoteNotification:(NSDictionary *)userInfo completionHandler:(void (^)())completionHandler
    {
        [StreetHawk handleRemoteNotification:userInfo withActionId:identifier needComplete:YES completionHandler:completionHandler];
    }`
 
 @param userInfo Payload passed in by remote notification.
 @param identifier Action button's identifier.
 @param needComplete Whether need to call `completionHandler` when task finish. If `completionHandler`=nil this does not matter YES or NO.
 @param completionHandler Pass in system's to finish when task is done.
 */
- (void)handleRemoteNotification:(NSDictionary *)userInfo withActionId:(NSString *)identifier needComplete:(BOOL)needComplete completionHandler:(void (^)())completionHandler NS_AVAILABLE_IOS(8_0);

/**
 Customer Application should implement this in UIApplicationDelegate to forward handling to StreetHawk library if NOT auto-integrate. If `StreetHawk.autoIntegrateAppDelegate = YES;` make sure NOT call this otherwise cause dead loop. Code snippet:
 
    `- (void)application:(UIApplication *)application didReceiveLocalNotification:(UILocalNotification *)notification`
    {
        [StreetHawk handleLocalNotification:notification needComplete:YES fetchCompletionHandler:nil];
    }` 
 
 @param notification Object passed in by local notification.
 @param needComplete Whether need to call `completionHandler` when task finish. If `completionHandler`=nil this does not matter YES or NO.
 @param completionHandler Pass in system's to finish when task is done.
 */
- (void)handleLocalNotification:(UILocalNotification *)notification needComplete:(BOOL)needComplete fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler;

/**
 Customer Application should implement this in UIApplicationDelegate to forward handling to StreetHawk library if NOT auto-integrate. If `StreetHawk.autoIntegrateAppDelegate = YES;` make sure NOT call this otherwise cause dead loop. Code snippet:
 
    `- (void)application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forLocalNotification:(UILocalNotification *)notification completionHandler:(void (^)())completionHandler
    {
        [StreetHawk handleLocalNotification:notification withActionId:identifier needComplete:YES completionHandler:completionHandler];
    }`
 
 @param notification Object passed in by local notification.
 @param identifier Action button's identifier.
 @param needComplete Whether need to call `completionHandler` when task finish. If `completionHandler`=nil this does not matter YES or NO.
 @param completionHandler Pass in system's to finish when task is done.
 */
- (void)handleLocalNotification:(UILocalNotification *)notification withActionId:(NSString *)identifier needComplete:(BOOL)needComplete completionHandler:(void (^)())completionHandler NS_AVAILABLE_IOS(8_0);

/**
 Set badge number on Application icon. In iOS 8 it needs to check permission, if not have permission return NO.
 @param badgeNumber Number shown on App icon.
 @return If set successfully return YES, else return NO in case no permission.
 */
- (BOOL)setApplicationBadge:(NSInteger)badgeNumber;

/**
 If set pause_minutes >= StreetHawk_AlertSettings_Forever, it treats as pause forever.
 */
#define SHAlertSettings_Forever 129600     //3 month

/**
 Set alert settings times measured by minutes.
 @param pauseMinutes Minute measured pause time. If `pauseMinutes` <= 0 means not pause, if `pauseMinutes` >= `StreetHawk_AlertSettings_Forever` means pause forever.
 @param handler Callback for finish.
 */
- (void)shSetAlertSetting:(NSInteger)pauseMinutes finish:(SHCallbackHandler)handler;

/**
 Get alert settings times measured by minutes.
 @return Integer value, measured in minutes. If `return` <= 0 means not pause, if `return` >= `StreetHawk_AlertSettings_Forever` means pause forever.
 */
- (NSInteger)getAlertSettingMinutes;

/**
 Client setup minutes pass to server. Server will calculate pause time according to it. `pause_until` is calculated by current time and `pause_minutes`. For example if current time is 8:00 and set `pause_minutes` = 60 (1 hour), `pause_until` will be 9:00. Later when time pass and now is 8:30, `pause_minutes` is still 60, but must relay on `pause_until`(9:00) to know the stop time, cannot use current time + `pause_minutes`.
 
 * `pause_minutes` is minute value.
 * If set `pause_minutes` >= `SH_AlertSettings_Forever`, it treats as pause forever.
 * If set `pause_minutes` <= 0, it treats as not paused.
 
 @handler Need to read from server, use this asynchronous callback. It's (NSDate* pauseUntil, NSError *error). If never set, return ([NSDate date], nil).
 */
- (void)getAlertSettingPauseUntil:(SHCallbackHandler)handler;

/** @name Handlers */

/**
 An instance to deal with location.
 */
@property (nonatomic, readonly, weak) SHLocationManager *locationManager;

/**
 An instance to deal with log.
 */
@property (nonatomic, readonly, weak) SHLogger *logger;

/**
 Handler for notification stuff.
 */
@property (nonatomic, readonly, weak) SHNotificationHandler *notificationHandler;

/**
 Handler for crash report stuff.
 */
@property (nonatomic, readonly, weak) SHCrashHandler *crashHandler;

/**
 Hanlder for rating. 
 */
@property (nonatomic, readonly, weak) SHRateHandler *rateHandler;

/** @name Global properties and methods */

/**
 API to trigger feedback. It behaves in this way:
 
 * If define option choice list by `arrayChoice`, a choice list will show first, after user select one of the option: a) If `needInput` is Yes, an input UI with the selected choice is displayed for user to input free text; b) If `needInput` is No user's selected choice is posted to server directly.
 * If `arrayChoice` = nil or empty, input free text UI is displayed for user to type.
 * Choice title is mandatory, free text detail content is optional.
 @param arrayChoice The option choice list. For example, @[@"Product not Available", @"Wrong Address", @"Description mismatch"]. It can be nil.
 @param needInput Whether need to show free text input dialog. If `arrayChoice` is nil or empty always show input dialog regardless of this settings.
 @param needConfirm Whether need to show confirm alert dialog of Cancel/Yes Please!. When App in FG and notification arrive needs to show confirm dialog.
 @param infoTitle The title display on choice list. If nil shows "<App Name> loves Feedback!".
 @param infoMessage The message display on choice list. It can be nil or empty.
 @param pushMsgid When used in remote notification, pass in "i" field from server. If not used in remote notification, pass 0.
 @param pushData When used in remote notification, pass in "d" field from server. If not used in remote notification, pass nil.
 */
-(void)shFeedback:(NSArray *)arrayChoice needInputDialog:(BOOL)needInput needConfirmDialog:(BOOL)needConfirm withTitle:(NSString *)infoTitle withMessage:(NSString *)infoMessage withPushMsgid:(NSInteger)pushMsgid withPushData:(NSString *)pushData;

/**
 Push notification 8004/8006/8007 is to launch a certain view controller, however it's difficult for server to know "how to launch the view controller". In iOS platform it requires the following elements to initialize a view controller:
 
 * view controller class name -- mandatory, must be a class inherit from UIViewController.
 * xib for iPhone -- optional, used for intialize the view controller on iPhone device, and xib name is different from class name; if nil use class name as the xib name.
 * xib for iPad -- optional, used for intialize the view controller on iPad device, and xib name is different from class name; if nil use class name as the xib name.
 
 For Phonegap App, it can be load an html page. Refer to `shPGHtmlReceiver` and `shPGLoadHtml`.
 
 Server may have problem to send push notification, because:
 
 * iOS and Android possibly have different class name, different mechanism to launch view controller. Need to consider the platform details.
 * Applications even running on the same platform can have different ac/vc names in different versions.
 
 To make server simpler in this aspect, client side provide a way to register "friendly name", while locally store the map for vc and xib. For example, client side can register: friendly name = "login", vc = "MyLoginViewController", xib_iphone = "MyLoginViewController_iphone", xib_ipad = "MyLoginViewController_ipad", so server display "login" in web console when sending push notification. After client side receive this notification, find vc and xib locally to initialize view controller. 
 
 **Note 1: Friendly names will be visible in StreetHawk web interface and they should be the same across different platforms (ios, android etc.).**
 **Note 2: Use friendly name = "register" (FRIENDLYNAME_REGISTER) for register page, which can be specifically handled by 8006 push notification.
 **Note 3: Use friendly name = "login" (FRIENDLYNAME_LOGIN) for login page, which can be specifically handled by 8007 push notification.
 **Note 4: The friendly name will be submitted when next `app_status` with "submit_views" = true.
 
 @param arrayFriendlyNameObj The array with each object defines a friendly name object.
 @return If the format is correct for register friendly name, return YES; otherwise return NO, and error information can refer to console log output (withDebugMode:YES).
 */
- (BOOL)shCustomActivityList:(NSArray *)arrayFriendlyNameObj;

/**
 Send enter log (8108) for `page`. For trace view it's recommended to inherit from `StreetHawkViewController` or `StreetHawkBaseViewController`, which automatically call `shNotifyPageEnter` on `viewDidAppear` and `shNotifyPageExit` on `viewDidDisappear`. But for App which cannot do inheritance (for example Phonegap, Titanium and Xamarin), call `shNotifyPageEnter` and `shNotifyPageExit` explictly.
 Note: if history has a page record, it sends exit log (8108) for the history. This is a workaround fix for "forget" add `shNotifyPageExit` on `viewDidDisappear`, and more importantly, some App such as Phonegap cannot call `shNotifyPageExit`.
 @param page Enter page name. It cannot be nil. For UIViewController it's class name such as `self.class.description`; for Phonegap it's html page name such as `index.html`.
 */
- (void)shNotifyPageEnter:(NSString *)page;

/**
 Send exit log (8109) for `page`. For trace view it's recommended to inherit from `StreetHawkViewController` or `StreetHawkBaseViewController`, which automatically call `shNotifyPageEnter` on `viewDidAppear` and `shNotifyPageExit` on `viewDidDisappear`. But for App which cannot do inheritance (for example Phonegap, Titanium and Xamarin), call `shNotifyPageEnter` and `shNotifyPageExit` explictly. 
 @param page Exit page name. It cannot be nil. For UIViewController it's class name such as `self.class.description`; for Phonegap it's html page name such as `index.html`.
 */
- (void)shNotifyPageExit:(NSString *)page;

/**
 Array for hosting customised handler.
 */
@property (nonatomic, strong) NSMutableArray *arrayCustomisedHandler;

/**
 Register handler for customised tasks.
 @param handler Instance class of `ISHCustomiseHandler` to let customer implement their own code.
 */
- (void)shSetCustomiseHandler:(id<ISHCustomiseHandler>)handler;

/**
 Register observer for phonegap App to load html page when receive 8004 push notification.
 @param phonegapObserver Instance class of `ISHPhonegapObserver` to load html on customer's phonegap web view.
 */
- (void)shPGHtmlReceiver:(id<ISHPhonegapObserver>)phonegapObserver;

/**
 Get stored view name for push 8004, this is used for App launches and check whether a 8004 push notification occured. If this App is waken up by 8004 push notification, the view name is stored locally and read by this function, so that App knows a specific page should be loaded.
 @return Locally stored view name when 8004 comes. It's read only once, after read local cache is cleared.
 */
- (NSString *)shGetViewName;

/**
 Trigger phonegap observer to load html page. This is used for 8004 notification which can map friendly name to html file name.
 @param htmlFile The html page register by `shCustomActivityList`.
 */
- (void)shPGLoadHtml:(NSString *)htmlFile;

/**
 Callback happen when open url delegate trigger.
 */
@property (nonatomic, copy) SHOpenUrlHandler openUrlHandler;

/** @name Background Regular Task */

/**
 Perform background task at certain time interval. It leverages `UIApplicationDelegate` function `- (void)application:(UIApplication *)application performFetchWithCompletionHandler:(void (^)(UIBackgroundFetchResult result))completionHandler` to do some tasks at background. Note:
 
 1. Customer App must have Background mode -> fetch enabled to have this work. 
 2. This function is available since iOS 7.0. Previous iOS system cannot support it. 
 3. User App implement this function by calling it in AppDelegate.m if NOT auto-integrate. If `StreetHawk.autoIntegrateAppDelegate = YES;` make sure NOT call this otherwise cause dead loop. Code snippet:
    `- (void)application:(UIApplication *)application performFetchWithCompletionHandler:(void (^)(UIBackgroundFetchResult result))completionHandler`
    `{`
        `[StreetHawk shBackgroundTask:completionHandler needComplete:YES];`
    `}`
 
 This function perform following tasks:
 1. It's called every hour.
 2. If user's location service is enabled, every time it's launched, send non-priority log for current user location (domain=location, code=19).
 3. Check whether install/log is sent within 6 hours. If inside 6 hours no install/log sent, sends priority heartbeat log (domain=system, code=8051).
 */
- (void)shBackgroundTask:(void (^)(UIBackgroundFetchResult result))completionHandler needComplete:(BOOL)needComplete NS_AVAILABLE_IOS(7_0);

/** @name Open Url Scheme */

/**
 Handle open URL, customer's App must register "URL Types" in Info.plist with its own scheme. User App implement this function by calling it in AppDelegate.m if NOT auto-integrate. If `StreetHawk.autoIntegrateAppDelegate = YES;` make sure NOT call this otherwise cause dead loop. Code snippet:
    `- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation`
    `{`
        `return [StreetHawk openURL:url sourceApplication:sourceApplication annotation:annotation];`
    `}`
 
 This function performs following tasks:
 1. Deeplinking: open a view and call function of the view. The url must be formatted as: <url scheme>://launchvc?vc=<friendly name or vc>&xib_iphone=<xib_iphone>&xib_ipad=<xib_ipad>&<additional params>. "<url scheme>" must same as Info.plist register URL type; "launchvc" is pre-defined command, case insensitive; "vc" is friendly name or UIViewController's class name, mandatory; others are optional.
 */
- (BOOL)openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation;

/** @name Feeds */

/**
 Callback happen when new feed detects by app_status/feed.
 */
@property (nonatomic, copy) SHNewFeedsHandler newFeedHandler;

/**
 Fetch feeds starting from `offset`.
 @param offset Offset from which to fetch.
 @param handler Callback for fetch handler, which return NSArray of SHFeedObject and error if meet.
 */
- (void)feed:(NSInteger)offset withHandler:(SHFeedsFetchHandler)handler;

/**
 Send no priority logline for feed once it's seen by user. Code=8201, domain is empty, comment=passin_feed_id, feed_id=passin_feed_id, result=passin_int.
 */
- (void)sendLogForFeed:(NSInteger)feed_id withResult:(NSInteger)result;

/** @name Permission */

/**
 Does user disable location permission for this App in system preference settings App. It's used to check before promote settings dialog by calling `- (void)launchSystemPreferenceSettings` to let user reset location since iOS 8, or before iOS 8 needs to show self made instruction. It's only return YES when make sure global location is disabled or App location is disabled. If this App not has location required (for example not have location key in Info.plist), or not ask for location service by prevent enable it, return NO.
 */
@property (nonatomic, readonly) BOOL systemPreferenceDisableLocation;

/**
 Does user disable notification permission for this App in system preference settings App. It's used to check before promote settings dialog by calling `- (void)launchSystemPreferenceSettings` to let user reset location since iOS 8, or before iOS 8 needs to show self made instruction. Return YES if notification is disabled or no type is enabled.
 */
@property (nonatomic, readonly) BOOL systemPreferenceDisableNotification;

/**
 Show this App's preference settings page. Only available since iOS 8. In previous iOS nothing happen.
 @return YES if can show preference page since iOS 8; NO if called in previous iOS and nothing happen.
 */
- (BOOL)launchSystemPreferenceSettings NS_AVAILABLE_IOS(8_0);

@end

/**
 **Extension for logs:**
 
 * The Profile for this install will be tagged with the given values. You can use these tags to run campaigns.
 
 [StreetHawk tagString:@"a@a.com" forKey:@"sh_email"];
 
 * Remove a tagged profile if not need it anymore.
 
 [StreetHawk removeTag:@"sh_email"];
 */
@interface SHApp (LoggerExt)

/**
 Send log with domain="custom", code=8999. It's used for tagging a string value for user. For example, you can tag user's email as by:
 
 `[StreetHawk tagString:@"a@a.com" forKey:@"sh_email"];`
 
 This will send log comment as {"key": "sh_email", "value": @"a@a.com", "type": "string"}.
 
 @param value The value for tag to the user profile. Cannot be empty. It can be NSString, or NSDictionary, or NSArray. 
 @param key The key for tag to the user profile. Cannot be empty.
 */
-(void)tagString:(NSObject *)value forKey:(NSString *)key;

/**
 Send log with domain="custom", code=8999. It's used for tagging a number value for user. For example, you can tag user's favourite product count by:
 
 `[StreetHawk tagNumeric:8 forKey:@"fave_product"];`
 
 This will send log comment as {"key": "fave_product", "value": [NSNumber numberWithDouble:8], "type": "numeric"}.
 
 @param value The number value for tag to the user profile.
 @param key The key for tag to the user profile. Cannot be empty.
 */
-(void)tagNumeric:(double)value forKey:(NSString *)key;

/**
 Send log with domain="custom", code=8999. It's used for tagging a date value for user. For example, you can tag user's visit time by:
 
 `[StreetHawk tagDatetime:[NSDate date] forKey:@"visit_time"];`
 
 This will send log comment as {"key": "visit_time", "value": [NSDate date], "type": "datetime"}.
 
 @param value The date value for tag to the user profile. Cannot be empty.
 @param key The key for tag to the user profile. Cannot be empty.
 */
-(void)tagDatetime:(NSDate *)value forKey:(NSString *)key;

/**
 This is opposite function of `tagString` or `tagNumeric` or `tagDatetime`. It's to remove a user tag by the key, for example `tagDatetime` adds {"key": "sh_date_of_birth", "value": "2012-12-12 11:11:11", "type": "datetime"}, so this `removeUserTag` can remove the tag by key = "sh_date_of_birth". It send log with domain="custom", code=8998, comment = "{key : "sh_date_of_birth"}".
 @param key Key for existing tag. Cannot be empty.
 */
-(void)removeTag:(NSString *)key;

/**
 Send log with domain="custom", code=8999, comment={"key": "<key>", "value": 1, "operator": "+", "type": "numeric"}.
 @param key Key for existing tag. Cannot be empty.
 */
-(void)incrementTag:(NSString *)key;

@end
