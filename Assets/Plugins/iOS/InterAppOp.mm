#import <UIKit/UIKit.h>
#import "UnityAppController.h"
 
 @interface InterAppOp: UnityAppController
 {
 }
 
 
 -(BOOL) application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation;
 
 @end
 @implementation InterAppOp 

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    
    if ([launchOptions objectForKey:UIApplicationLaunchOptionsURLKey]) {
        NSURL *url = [launchOptions objectForKey:UIApplicationLaunchOptionsURLKey];
        
        [self performSelector:@selector(openURLAfterDelay:) withObject:url afterDelay:2];
    }
    
    return YES;
}

- (void) openURLAfterDelay:(NSURL*) url
{
    
    UnitySendMessage("StreetHawk", "URLHandler", [[url absoluteString] UTF8String]);
}

-(BOOL) application:(UIApplication *)application handleOpenURL:(NSURL *)url
{
    
    UnitySendMessage("StreetHawk", "URLHandler", [[url absoluteString] UTF8String]);
    
    return YES;
    
}

 -(BOOL) application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
 
 {
     UnitySendMessage("StreetHawk", "URLHandler", [[url absoluteString] UTF8String]);
     return YES;
     
 }
 @end
 
 
 
 IMPL_APP_CONTROLLER_SUBCLASS(InterAppOp)