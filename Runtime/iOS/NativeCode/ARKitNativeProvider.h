// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#import <ARKit/ARKit.h>

// XRSessionExtensions.GetNativePtr
typedef struct UnityXRNativeSession
{
    int version;
    void* sessionPtr;
} UnityXRNativeSession;

typedef void (*OnARSessionUpdatedFrame)(void * _Nonnull, double, const float *);

@interface ARKitNativeProvider : NSObject

@property (nonatomic, weak, nullable) id<ARSessionDelegate> unityARSessionDelegate;
@property (nonatomic, assign, nullable) OnARSessionUpdatedFrame onARSessionUpdatedFrame;

@end
