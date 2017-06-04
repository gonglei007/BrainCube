//
//  ChartBoostManager.h
//  CB
//
//  Created by Mike DeSaro on 12/20/11.
//


#import <Foundation/Foundation.h>
#import "Chartboost.h"
#import "CBInPlay.h"

#ifdef UNITY_4_2_0
  #import "UnityAppController.h"
#else
  #import "AppController.h"
#endif


@interface ChartBoostManager : NSObject <ChartboostDelegate>

@property (nonatomic) BOOL shouldPauseClick;
@property (nonatomic) BOOL shouldRequestFirstSession;

// Properties used by delegates
@property (nonatomic) BOOL hasCheckedWithUnityToDisplayInterstitial;
@property (nonatomic) BOOL hasCheckedWithUnityToDisplayRewardedVideo;
@property (nonatomic) BOOL hasCheckedWithUnityToDisplayMoreApps;
@property (nonatomic) BOOL unityResponseShouldDisplayInterstitial;
@property (nonatomic) BOOL unityResponseShouldDisplayRewardedVideo;
@property (nonatomic) BOOL unityResponseShouldDisplayMoreApps;

@property (nonatomic, retain) NSString *gameObjectName;

+ (ChartBoostManager*)sharedManager;

- (void)startChartBoostWithAppId:(NSString*)appId appSignature:(NSString*)appSignature;

@end


#ifdef UNITY_4_2_0
@interface UnityAppController(ChartBoostBugFix)
#else
@interface AppController(ChartBoostBugFix)
#endif

- (UIWindow*)window;

@end
