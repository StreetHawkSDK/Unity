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
 Handler for rating App, or open App Store to point to the App.
 */
@interface SHRateHandler : NSObject

/**
 Promote rate dialog regardless of how many times enter or user dismiss before. 
 @param infoTitle Title show on action sheet, optional. `infoTitle` and `infoMessage` cannot all be empty otherwise action sheet will not show.
 @param infoMessage Message show on action sheet, optional. `infoTitle` and `infoMessage` cannot all be empty otherwise action sheet will not show.
 @param pushMsgid When used in remote notification, pass in "i" field from server. If not used in remote notification, pass 0.
 @param pushData When used in remote notification, pass in "d" field from server. If not used in remote notification, pass nil.
 */
-(void)promoteRateWithTitle:(NSString *)infoTitle withMessage:(NSString *)infoMessage withPushMsgid:(NSInteger)pushMsgid withPushData:(NSString *)pushData;

/**
 Open AppStore App to rate `itunesAppId`. If `itunesAppId` is nil or empty, or in simulator, it cannot successfully open AppStore to locate the App.
 @param itunesAppId The App's unique id in iTunes.
 @param pushMsgid When used in remote notification, pass in "i" field from server. If not used in remote notification, pass 0.
 @param pushData When used in remote notification, pass in "d" field from server. If not used in remote notification, pass nil.
 */
-(void)rateApp:(NSString *)itunesAppId withPushMsgid:(NSInteger)pushMsgid withPushData:(NSString *)pushData;

@end
