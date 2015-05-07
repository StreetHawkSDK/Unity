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
 Category extension of NSObject.
 */
@interface NSObject(StreetHawkExt)

/**
 Present the `contentViewController` in the middle of full screen. It covers on top as a new window and layout in middle of screen. The xib of `contentViewController` should setup right "Freedom" size. To dismiss this view controller, call `[contentViewController dismissModalDialogViewController]`.
 */
- (void)presentModalDialogViewController:(UIViewController *)contentViewController;

@end

/**
 Category extension of UIViewController.
 */
@interface UIViewController(StreetHawkExt)

/**
 If a view controller has already presented a modal VC, forwards the call recursively to the VC that is currently being presented. For example, A has presented B, if call [A presentModalViewController:C], C cannot show up because B has already been presented. This recursive call will forward A to B, thus it will call [B presentModalViewController:C], so C cover on B.
 */
- (void)presentModalTopViewController:(UIViewController *)modalViewController animated:(BOOL)animated;

/**
 Dismiss the view controller which is presented by `presentModalDialogViewController`.
 */
- (void)dismissModalDialogViewController;

@end

/**
 SHBaseViewController and SHBaseTableViewController conform this protocol to handle common stuff:
 
 1. Log when VC enter(viewDidAppear)/exit(viewWillDisappear).
 2. Adjust UI for iOS 7.
 3. Move or shrink UI when keyboard show/hide.
 4. Load xib from all possible bundles.
 */
@protocol SHBaseVC <NSObject>

/**
 When keyboard show it may cover current view and make some part invisible. If `isViewAdjustForKeyboard`=YES, this view will automatically move up till while view is visible, and if it go out of the top of window, it will shrink size to lower height. This is useful for input dialog such as FeedbackInputViewController; If `isViewAdjustForKeyboard`=NO nothing happen when keyboard show/hide. It's NO by default because keyboard maybe show up for textbox in other view, and this view should not be affected.
 */
@property (nonatomic) BOOL isViewAdjustForKeyboard;

/**
 The handling function for keyboard did show. It has the default implementation to adjust view position and size, and child class can override it if want some customized handling.
 @param frameBegin The start frame before keyboard show. Note: it's not only read from UIKeyboardFrameBeginUserInfoKey, but also converted to current coordinate system.
 @param frameEnd The end frame after keyboard show. Note: it's not only read from UIKeyboardFrameEndUserInfoKey, but also converted to current coordinate system.
 @param second The time of this animation.
 */
- (void)keyboardDidShowFrom:(CGRect)frameBegin to:(CGRect)frameEnd duration:(double)second;

/**
 The handling function for keyboard did hide. It has the default implementation to adjust view position and size, and child class can override it if want some customized handling.
 @param frameBegin The start frame before keyboard hide. Note: it's not only read from UIKeyboardFrameBeginUserInfoKey, but also converted to current coordinate system.
 @param frameEnd The end frame after keyboard hide. Note: it's not only read from UIKeyboardFrameEndUserInfoKey, but also converted to current coordinate system.
 @param second The time of this animation.
 */
- (void)keyboardDidHideFrom:(CGRect)frameBegin to:(CGRect)frameEnd duration:(double)second;

@end

/**
 Protocol for deal with deeplinking. `StreetHawkBaseViewController` and `StreetHawkBaseTableViewController` conform this protocol, customer App's view controller is recommended to inherit from `StreetHawkBaseViewController` or `StreetHawkBaseTableViewController`, so can implement this protocol. Or customer's App's view controller can directly conform this protocol for deeplinking.
 */
@protocol ISHDeepLinking <NSObject>

@optional

/**
 Implement this function for receive deeplinking parameters. Customer App needs to hold the pass in `dictParam` in some internal data structure, cannot rely on this function to show to UI. Because this function is called before UI loaded, the controls are not created yet. `StreetHawkBaseViewController` or `StreetHawkBaseTableViewController` automatically calls `displayDeepLinkingToUI` on `viewDidLoad` to display data to UI.
 @param dictParam Pass in parameters.
 */
- (void)receiveDeepLinkingData:(NSDictionary *)dictParam;

/**
 Implement this function if need to show deeplinking data to UI. The data was received in `receiveDeepLinkingData:` before UI loaded, and it should be stored in customer's view controller internal data. Call this function whenever it's ready to show data to UI controller. `viewDidLoad` is already automatically called by `StreetHawkBaseViewController` or `StreetHawkBaseTableViewController`.
 */
- (void)displayDeepLinkingToUI;

@end

/**
 Base class for all view controller inherit from UIViewController. It sends logs when enter/exit this VC.
 */
@interface StreetHawkBaseViewController : UIViewController <ISHDeepLinking>

@end

/**
 Base class for all view controller inherit from UITableViewController. It sends logs when enter/exit this VC.
 */
@interface StreetHawkBaseTableViewController : UITableViewController <ISHDeepLinking>

@end

/**
 Base class for all view controller inherit from StreetHawkBaseViewController.
 */
@interface SHBaseViewController : StreetHawkBaseViewController <SHBaseVC>

@end

/**
 Base class for all view controller inherit from StreetHawkBaseTableViewController.
 */
@interface SHBaseTableViewController : StreetHawkBaseTableViewController <SHBaseVC>

@end
