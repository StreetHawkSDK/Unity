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
 Streethawk calls [ISHPhoneGapObserver shPGDisplayHtmlFileName] function when a friendly names is sent from Streethawk server. Customer App developer gets the name of HTML file and need to implemented in Phonegap App by themselves. This is observer model. 
 
 1. Customer App create class (assume named `SHPhonegapObserver`) inherit from `ISHPhonegapObserver` and implement function `shPGDisplayHtmlFileName`, which get html page name and implement how to load the html page on web view.
 2. Customer App call `[StreetHawk shPGHtmlReceiver:<instance_SHPhonegapObserver>` to register the observer. 
 3. When StreetHawk sends 8004, map friendly name to html page and call registered instance of `ISHPhoneGapObserver`.
 */
@protocol ISHPhonegapObserver <NSObject>

@required

/**
 Phonegap Only! Function returns name of the html file which needs to be displayed in application. 
 @param html_fileName
 */
- (void)shPGDisplayHtmlFileName:(NSString *)html_fileName;

@end
