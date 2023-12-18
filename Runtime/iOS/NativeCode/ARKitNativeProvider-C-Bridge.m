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
