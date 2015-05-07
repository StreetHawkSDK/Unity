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
 The directions of how slide show.
 */
enum SHSlideDirection
{
    /**
     Move from device's bottom to up.
     */
    SHSlideDirection_Up = 0,
    /**
     Move from device's top to down.
     */
    SHSlideDirection_Down = 1,
    /**
     Move from device's right to left.
     */
    SHSlideDirection_Left = 2,
    /**
     Move from device's left to right.
     */
    SHSlideDirection_Right = 3,
};
typedef enum SHSlideDirection SHSlideDirection;

/**
 Callback for `SHSlideContentViewController` load finish.
 */
typedef void(^SHSlideContentLoadFinish)(BOOL isSuccesss);

/**
 Protocal for slide content to implement.
 */
@protocol SHSlideContentViewController <NSObject>

@required

/**
 Property for notice content is loaded finished.
 */
@property (copy, nonatomic) SHSlideContentLoadFinish contentLoadFinishHandler;

/**
 After slide display for first time, or after rotation, content VC may need to adjust subview position according to new frame. Adjust subview position in this method.
 */
- (void)contentViewAdjustUI;

@optional

/**
 Property for passing remote notification msgid with slide.
 */
@property (nonatomic) NSInteger pushMsgid;

/**
 Property for passing remote notification comment string with slide.
 */
@property (nonatomic, strong) NSString *pushData;

@end

/**
 **Extension for slide:**
 
 Slide utility to show some content. For example:
 
 * `slideForUrl:withDirection:withSpeed:withCoverPercentage:withHideLoading:withAlertTitle:withAlertMessage:` is to slide a web view showing the url.
 * `slideForVC:withDirection:withSpeed:withCoverPercentage:withHideLoading:withAlertTitle:withAlertMessage:` is to slide a customized view controller, the view controller can be created by customer and do whatever they want.
 */
@interface SHApp (SlideExt)

/**
 Utility function to show slide with url content.
 @param url The content's url link, NSString type, for example @"www.google.com" or @"https://www.some.com". If it does not contain the protocal prefix @"://", default @"http://" will be added. But if it's not http protocal, must have the complete address.
 @param direction The slide show direction, refer to enum `SHSlideDirection`.
 @param speed The seconds duration from start slide till it complete, measured in seconds. It should be a positive number. Note: it's the time for animate slide show, not include time for loading web page. It should be a positive number, if not use 0.1 by default.
 @param percentage The width or height (depending on direction) of the slide cover on screen. It should be from 0 to 1. If set too much to outside of screen, the slide cover whole screen.
 @param isHideLoading Whether the slide should wait till web page load finished. If YES the slide will not show till web page finish loading, and then call confirm alert (if `alertTitle` or `alertMessage` not empty) or slide without activity spinner; If NO the slide show immediately with an acivity spinner, not wait till web page load finish.
 @param alertTitle Before slide it may be show a confirm dialog if `alertTitle` or `alertMessage` not empty and `needShowDialog`=YES. This title will also display on slide top.
 @param alertMessage Same as `alertTitle`.
 @param needShowDialog Before slide it may be show a confirm dialog if `alertTitle` or `alertMessage` not empty and `needShowDialog`=YES.
 @param pushMsgid When used in remote notification, pass in "i" field from server. If not used in remote notification, pass 0.
 @param pushData When used in remote notification, pass in "d" field from server. If not used in remote notification, pass nil.
 */
- (void)slideForUrl:(NSString *)url withDirection:(SHSlideDirection)direction withSpeed:(double)speed withCoverPercentage:(double)percentage withHideLoading:(BOOL)isHideLoading withAlertTitle:(NSString *)alertTitle withAlertMessage:(NSString *)alertMessage withNeedShowDialog:(BOOL)needShowDialog withPushMsgid:(NSInteger)pushMsgid withPushData:(NSString *)pushData;

/**
 Utility function to show slide with url content.
 @param contentVC A view controller to display as content of the slide.
 @param direction The slide show direction, refer to enum `SHSlideDirection`.
 @param speed The seconds duration from start slide till it complete, measured in seconds. It should be a positive number. Note: it's the time for animate slide show, not include time for loading content VC. It should be a positive number, if not use 0.1 by default.
 @param percentage The width or height (depending on direction) of the slide cover on screen. It should be from 0 to 1. If set too much to outside of screen, the slide cover whole screen.
 @param isHideLoading Whether the slide should wait till content VC load finished. If YES the slide will not show till content VC finish loading, and then call confirm alert (if `alertTitle` or `alertMessage` not empty) or slide without activity spinner; If NO the slide show immediately with an acivity spinner, not wait till content VC load finish.
 @param alertTitle Before slide it may be show a confirm dialog if `alertTitle` or `alertMessage` not empty.
 @param alertMessage Same as `alertTitle`.
 */
- (void)slideForVC:(UIViewController<SHSlideContentViewController> *)contentVC withDirection:(SHSlideDirection)direction withSpeed:(double)speed withCoverPercentage:(double)percentage withHideLoading:(BOOL)isHideLoading withAlertTitle:(NSString *)alertTitle withAlertMessage:(NSString *)alertMessage withNeedShowDialog:(BOOL)needShowDialog ;

@end
