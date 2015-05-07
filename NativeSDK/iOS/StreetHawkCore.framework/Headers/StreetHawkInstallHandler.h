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

/**
 A handler for install notifications.
 */
@interface SHInstallHandler : NSObject

/** @name Register/Unregister Notification */

/**
 According to system and customer code's setting, call system API to register notification <both for remote notification and local notification> or update revoked.
 
 * Call system API to register notification unless `StreetHawk.isEnableNotification=NO`.
 * Check system enable notification or `StreetHawk.isEnableNotification=NO` to update flag `revoked` in StreetHawk server. This flag can stop StreetHawk server from sending remote notification.
 */
-(void)registerForNotificationAndNotifyServer;

/** @name Install notification handlers */

/**
 Called when install/register finish successfully.
 */
- (void)installRegistrationSucceeded:(NSNotification *)aNotification;

/**
 Called when install/register finish with error.
 */
- (void)installRegistrationFailure:(NSNotification *)aNotification;

/**
 Called when install/update finish successfully.
 */
- (void)installUpdateSucceeded:(NSNotification *)aNotification;

/**
 Called when install/update finish with error.
 */
- (void)installUpdateFailure:(NSNotification *)aNotification;

@end
