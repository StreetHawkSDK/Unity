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

#ifndef __STREETHAWK__CORE__UTILS__H__
#define __STREETHAWK__CORE__UTILS__H__

#import "StreetHawkPublicFwds.h"

/** @name Data Format Convert Utility */

/**
 Convert an ASCII encoded c-string to NSString, for example `cstringToNSString(__FILE__)` to get NSString of current file.
 */
extern NSString *cstringToNSString(const char *input);

/**
 Converts a byte array into an hex string.
 @param data Byte data.
 @return Hex string.
 */
extern NSString *dataToHexString(NSData *data);

/**
 Get a new instance of NSDateFormatter for the supported date format, timezone and locale. It's auto-release, caller should not release it again.
 @param dateFormat The format string of date, by default (nil) it's yyyy-MM-dd HH:mm:ss, this is also the recognizable date format of server.
 @param timeZone The time zone of format, by default (nil) it's UTC.
 @param locale The locale of format, by default (nil) it's en-US.
 @return A new instance of date formatter.
 */
extern NSDateFormatter *getDateFormatter(NSString *dateFormat, NSTimeZone *timeZone, NSLocale *locale);

/**
 Formats a date into a string in StreetHawk-wide format. Must use this format to be recognizable by server. Format is yyyy-MM-dd HH:mm:ss in UTC timezone.
 @param date The date value to be formatted.
 @return The return string.
 */
extern NSString *formatStreetHawkDate(NSDate *date);

/**
 Parses date string into NSDate format. It tries to support as much format as possible. Refer to `input` parameters for the supported date time format.
 @param input Date time string. It supports this kinds of strings:
 
 * yyyy-MM-dd HH:mm:ss, for example 2012-12-20 18:20:50
 * yyyy-MM-dd, for example 2012-12-20
 * dd/MM/yyyy HH:mm:ss, for example 20/12/2012 18:20:50
 * dd/MM/yyyy, for example 20/12/2012
 * MM/dd/yyyy HH:mm:ss, for example 12/20/2012 18:20:50
 * MM/dd/yyyy, for example 12/20/2012
 
 @param offsetSeconds The offsetSeconds parameter tells how many seconds the parsed date is to be offset by.
 */
extern NSDate *parseDate(NSString *input, int offsetSeconds);

/**
 Parse the string or json object to a dictionary. It handles as much situation as possible, for example, the obj is a right dictionary, or the obj is a string, or the string obj contains wrong "\"" etc.
 @param obj The possible object to be parsed to dictionary.
 @return Try all possible way to parse it to dictionary. If fail return nil.
 */
extern NSDictionary *parseObjectToDict(NSObject *obj);

/** @name URL Process Utility */

/**
 Appends a whole bunch of parameter and values to a string in the form: param1=value1&param2=value2&param3=value3....
 @param params Array listed as [param1, value1, param2, value2, param3, value3, ...], it must be paired.
 @param isForPost If YES only "&" is used; otherwise first append should be "?".
 @return Result string formatted by pass in value.
 */
extern NSMutableString *appendParamsArrayToString(NSMutableString *str, NSArray *params, BOOL isForPost);

/**
 Appends a whole bunch of parameter and values to a string in the form: param1=value1&param2=value2&param3=value3....
 @param params Dictionary listed as {param1 = value1, param2 = value2, param3 = value3, ...}, it must be paired.
 @param isForPost If YES only "&" is used; otherwise first append should be "?".
 @return Result string formatted by pass in value.
 */
extern NSMutableString *appendParamsDictToString(NSMutableString *str, NSDictionary *params, BOOL isForPost);

/**
 Parse get request string's parameter string to NSDictionary. For example, param1=value1&param2=value2&param3=value3 is parsed to {param1:value1, param2:value2, param3=value3}.
 @param str Parameter string of a get request, formatted as: param1=value1&param2=value2&param3=value3...
 @return Dictionary parsed from the parameter string.
 */
extern NSDictionary *parseGetParamStringToDict(NSString *str);

/** @name UI Utility */

/**
 A common way to present error by showing an alert view with error details.
 @param error The error to present. If error is nil nothing happen.
 @param announceNetworkError If the error is network problem, this decides whether to show the alert view or not.
 */
extern void presentErrorAlert(NSError *error, BOOL announceNetworkError);

/**
 Get corresponding view controller for a view.
 */
extern UIViewController *getViewController(UIView *view);

/**
 Dismiss all message views, including UIAlertView, UIActionSheet, UIModalView.
 */
extern void dismissAllMessageView();

/** @name Resources and Bundles Utility */

/**
 Find the bundle contains the resource with type. StreetHawk library has its own resource bundle `StreetHawkCoreRes.bundle` for images, xibs, strings etc, these resources are not in main bundle. To find them properly, need to give the correct bundle. This utility function is to get properly bundle.
 @param resourceName The resource name required, for example @"InputiBeaconViewController" or @"loginfo".
 @param type The resource type required, for example @"nib" or @"png".
 @param mandatory Whether this resource is mandatory. If not find suitable bundle, trigger assert when `mandatory` = YES.
 @return Return find bundle, it may be main bundle, or sub-bundle, or even nil.
 */
extern NSBundle *findBundleForResource(NSString *resourceName, NSString *type, BOOL mandatory);

/**
 Search for localized string for a `key`, first search in App's bundle, then search StreetHawkCoreRes.bundle. If nothing match return `defaultStr`. This provides a mechanism for customer App to override resource strings inside StreetHawk library. Customer should add Localizable.strings file and override the resource strings with same key defined in StreetHawkRes.bundle's Localizable.strings.
 @param key Key registered in Localizable.strings.
 @param defaultStr If nothing matching in Localizable.strings (both customer's and StreetHawkCoreRes.bundle's), use this `defaultStr`.
 */
extern NSString *localizedString(NSString *key, NSString *defaultStr);

/** @name Other Utility */

/**
 Call phone number. If success return YES else return NO.
 @param phone The string for phone number to call.
 @return If pass in phone string can call, return YES; else return NO.
 */
extern BOOL callPhoneNumber(NSString *phone);

/**
 Get mac address of current device, the output is for example: 00:23:32:CB:AB:80. However since iOS 7 this function cannot work on device anymore, always return fake address.
 */
extern NSString *getMacAddress();

/**
 Get carrier's name from current device, for example "AT&T", "China Mobile" etc. If current device does not connect to carrier, for example iPad Wifi, return "Other".
 */
extern NSString *getCarrierName();

/**
 Get device's screen resolution, for example "640*480".
 */
extern NSString *getScreenResolution();

/**
 Enum for this App's mode.
 */
enum SHAppMode
{
    /**
     Cannot detect which mode is. This should never happen, in debug SDK trigger assert.
     */
    SHAppMode_Unknown,
    /**
     The App is running on simulator. Against it all below is running on device.
     */
    SHAppMode_Simulator,
    /**
     The App is built with development provisioning, will use sandbox remote notification.
     */
    SHAppMode_DevProvisioning,
    /**
     The App is built with ad-hoc provisioning, will use production remote notification.
     */
    SHAppMode_AdhocProvisioning,
    /**
     The App is downloaded from AppStore. will use production remote notification.
     */
    SHAppMode_AppStore,
    /**
     The App is downloaded from enterprise distribution way. will use production remote notification.
     */
    SHAppMode_Enterprise,
};
typedef enum SHAppMode SHAppMode;

/**
 Return the mode of current App.
 */
extern SHAppMode appMode();

/**
 Return the string to describe current mode. 
 */
extern NSString *appModeString(SHAppMode mode);

/**
 Enum for development platforms.
 */
enum SHDevelopmentPlatform
{
    /**
     Native iOS build by Xcode. Most Apps use this, and it's default.
     */
    SHDevelopmentPlatform_Native,
    /**
     Phonegap Apps.
     */
    SHDevelopmentPlatform_Phonegap,
    /**
     Titanium Apps.
     */
    SHDevelopmentPlatform_Titanium,
    /**
     Xamarin Apps.
     */
    SHDevelopmentPlatform_Xamarin,
    /**
     Unity Apps.
     */
    SHDevelopmentPlatform_Unity,
};
typedef enum SHDevelopmentPlatform SHDevelopmentPlatform;

/**
 Return string describing which development platform current App is.
 */
extern NSString *developmentPlatformString();

/**
 Return app/status result of streethawk function should be enabled. 
 */
extern BOOL streetHawkIsEnabled();

#endif
