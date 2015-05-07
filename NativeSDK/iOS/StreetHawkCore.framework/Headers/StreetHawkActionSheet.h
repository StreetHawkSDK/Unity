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
 Callback when click button on SHActionSheet.
 */
typedef void(^SHActionSheetHandler)(UIActionSheet *view, NSInteger buttonIndex);

/**
 UIActionSheet needs to have delegate for the click events, which is not convenient and spread in different parts of code. SHActionSheet wrapper UIActionSheet by having handler to deal with callback. The create and button click can be in same piece of code.
 */
@interface SHActionSheet : UIActionSheet <UIActionSheetDelegate>

/**
 Callback when click button on SHActionSheet.
 */
@property (nonatomic, copy) SHActionSheetHandler closedHandler;

/**
 Create SHActionSheet.
 */
- (id)initWithTitle:(NSString *)title withHandler:(SHActionSheetHandler)handler cancelButtonTitle:(NSString *)cancelTitle otherButtonTitles:(NSString *)otherButtonTitles,...;

/**
 Show SHActionSheet in main windows.
 */
- (void)show;

@end
