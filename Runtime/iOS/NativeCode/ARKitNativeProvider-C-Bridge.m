// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#import "ARKitNativeProvider.h"

void* HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_init() {
    ARKitNativeProvider *provider = [[ARKitNativeProvider alloc] init];
    return (__bridge_retained void *)provider;
}

void HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_registerCallbacks(void *self,
                                                                                      OnARSessionUpdatedFrame onARSessionUpdatedFrame) {
    ARKitNativeProvider *provider = (__bridge ARKitNativeProvider *)self;
    [provider setOnARSessionUpdatedFrame:onARSessionUpdatedFrame];
}

void HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_interceptUnityARSessionDelegate(void *self, UnityXRNativeSession *nativeARSessionPtr) {
    if (nativeARSessionPtr == NULL) {
        return;
    }
    
    ARKitNativeProvider *provider = (__bridge ARKitNativeProvider *)self;
    ARSession *session = (__bridge ARSession *)nativeARSessionPtr->sessionPtr;
    [provider setUnityARSessionDelegate:session.delegate];
    [provider setSession:session];
    if (session.delegate != provider) {
        [session setDelegate:provider];
    }
}

void HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_restoreUnityARSessionDelegate(void *self, UnityXRNativeSession *nativeARSessionPtr) {
    if (nativeARSessionPtr == NULL) {
        return;
    }
    
    ARKitNativeProvider *provider = (__bridge ARKitNativeProvider *)self;
    ARSession *session = (__bridge ARSession *)nativeARSessionPtr->sessionPtr;
    if (session.delegate == provider) {
        [session setDelegate:provider.unityARSessionDelegate];
    }
}

void HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_resetOrigin(void *self, float position[3], float rotation[4]) {
    ARKitNativeProvider *provider = (__bridge ARKitNativeProvider *)self;
    simd_float4x4 transform_matrix = [ARKitNativeProvider getSimdFloat4x4WithPosition:position rotation:rotation];
    [[provider session] setWorldOrigin:transform_matrix];
}
