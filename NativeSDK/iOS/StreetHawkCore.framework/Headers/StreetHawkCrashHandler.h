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
 Handler to deal with crash. It uses open-source CrashReporter.framework. The usage is:
 
 * When App initialized, call `enableCrashReporter` to have crash handler get ready. 
 * If crash happen, the crash report is logged locally. Use `hasPendingCrashReport` to check whether local has pending crash report. 
 * If local has crash report, use `loadPendingCrashReport` to get the report data string. 
 * After send crash report to server, use `purgePendingCrashReport` to clear the local one.
 
 For StreetHawkCoreCompact, remove PLCrashReport to reduce library size, thus only can catch some basic information by `ExceptionHandler`.
 */
@interface SHCrashHandler : NSObject

/**
 Make crash reporter to get ready. If error happen it will log as error event and sent to server.
 */
-(BOOL)enableCrashReporter;

/**
 Whether local has un-sent crash report. 
 */
-(BOOL)hasPendingCrashReport;

/**
 If local has crash report, use this to load the crash report string. It may return nil if fail to load the data. If error happen it will log as error event and sent to server.
 @return The text format is compatible with iTunesConnect download crash report.
 */
-(NSString *)loadPendingCrashReport;

/**
 Return the date parsed from current crash report. If no crash report or fail to load, it's nil.
 */
-(NSDate *)crashReportDate;

/**
 Clear local un-sent crash report. If error happen it will log as error event and sent to server.
 */
-(BOOL)purgePendingCrashReport;

@end
