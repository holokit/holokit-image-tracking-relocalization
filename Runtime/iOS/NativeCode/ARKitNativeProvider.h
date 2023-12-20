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
@property (nonatomic, strong, nullable) ARSession *session;
@property (nonatomic, assign, nullable) OnARSessionUpdatedFrame onARSessionUpdatedFrame;

+ (simd_float4x4)getSimdFloat4x4WithPosition:(float [3])position rotation:(float [4])rotation;

@end