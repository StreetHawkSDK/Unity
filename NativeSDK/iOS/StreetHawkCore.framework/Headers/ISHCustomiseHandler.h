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

#import <Foundation/Foundation.h>

/**
 Customer override these functions to provide their own customised functions.
 */
@protocol ISHCustomiseHandler <NSObject>

@optional

/**
 Streethawk calls [ISHCustomiseHandler shRawJsonCallbackWithTitle:withMessage:withJson:] function when a customized notification code 8049 arrives at device. Customer is responsible to handle this message by their own code:
 
 1. Customer App create class (assume named `MyHandler`) inherit from `ISHCustomiseHandler` and implement this function.
 2. Customer App call `[StreetHawk shSetCustomiseHandler:<instance_MyHandler>` to register the observer.
 3. When StreetHawk sends 8049, customer's implementation is triggered.
 
 @param title Title of the payload.
 @param message Message of the payload.
 @param json Json string, usually parse to NSDicationary. Use system API `NSJSONSerialization`, or use StreetHawk utility function `NSDictionary *dict = parseObjectToDict(json)`.
 */
- (void)shRawJsonCallbackWithTitle:(NSString *)title withMessage:(NSString *)message withJson:(NSString *)json;

@end
