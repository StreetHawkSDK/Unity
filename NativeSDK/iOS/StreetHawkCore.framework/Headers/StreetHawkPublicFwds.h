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

#ifndef SHPublicFwds_h
#define SHPublicFwds_h

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

//App
@class          SHApp;

//CrashHandler
@class          SHCrashHandler;

//FeedObject
@class          SHFeedObject;
@class          SHFeedNewsObject;
@class          SHFeedOfferObject;
@class          SHFeedJsonObject;
@class          SHFeedUnknownObject;

//FriendlyName
@class          SHFriendlyNameObject;

//Install
@class          SHInstall;
@class          SHInstallHandler;

//LocationManager
@class          SHLocationManager;

//Logger
@class          SHLogger;

//NotificationHandler
@class          SHNotificationHandler;
@class          PushDataForApplication;

//Object
@class          SHObject;

//RateHandler
@class          SHRateHandler;

//Request
@class          SHRequest;

//UI
@class          SHActionSheet;
@class          SHAlertSettingsViewController;
@class          SHAlertView;
@class          SHBaseTableViewController;
@class          SHSlideViewController;

/**
 Callback handler with a result or an error.
 
 * If successfully result is the requested object, error is nil.
 * if fail result is nil, error is NSError for failure details.
 */
typedef void (^SHCallbackHandler)(NSObject *result, NSError *error);

/**
 Defined as common error domain reported from StreetHawk.
 */
#define SHErrorDomain  @"SHErrorDomain"

/**
 Make sure not pass nil or NSNull, this is useful to avoid insert nil to NSArray and cause crash.
 */
#define NONULL(str)     ((str && str != (id)[NSNull null]) ? (str) : @"")

#endif
