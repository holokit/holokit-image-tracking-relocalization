// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#import "ARKitNativeProvider.h"

@interface ARKitNativeProvider() <ARSessionDelegate>

@end

@implementation ARKitNativeProvider

- (instancetype)init {
    if (self = [super init]) {
        
    }
    return self;
}

+ (float *)getUnityMatrix:(simd_float4x4)arkitMatrix {
    float *unityMatrix = new float[16] { arkitMatrix.columns[1].x, -arkitMatrix.columns[0].x, -arkitMatrix.columns[2].x, arkitMatrix.columns[3].x,                                                 arkitMatrix.columns[1].y, -arkitMatrix.columns[0].y, -arkitMatrix.columns[2].y, arkitMatrix.columns[3].y,                                                -arkitMatrix.columns[1].z, arkitMatrix.columns[0].z, arkitMatrix.columns[2].z, -arkitMatrix.columns[3].z,                                                  arkitMatrix.columns[0].w, arkitMatrix.columns[1].w, arkitMatrix.columns[2].w, arkitMatrix.columns[3].w };
    return unityMatrix;
}

#pragma mark - ARSessionDelegate

- (void)session:(ARSession *)session didUpdateFrame:(ARFrame *)frame {
    if (self.unityARSessionDelegate != NULL) {
        [self.unityARSessionDelegate session:session didUpdateFrame:frame];
    }
    
    if (self.onARSessionUpdatedFrame != NULL) {
        float *matrix = [ARKitNativeProvider getUnityMatrix:frame.camera.transform];
        double timestamp = frame.timestamp;
        dispatch_async(dispatch_get_main_queue(), ^{
            self.onARSessionUpdatedFrame((__bridge void *)self, timestamp, matrix);
            delete[](matrix);
        });
    }
}

- (void)session:(ARSession *)session didAddAnchors:(NSArray<__kindof ARAnchor*>*)anchors {
    if (self.unityARSessionDelegate != NULL) {
        [self.unityARSessionDelegate session:session didAddAnchors:anchors];
    }
}

- (void)session:(ARSession *)session didUpdateAnchors:(NSArray<__kindof ARAnchor*>*)anchors {
    if (self.unityARSessionDelegate != NULL) {
        [self.unityARSessionDelegate session:session didUpdateAnchors:anchors];
    }
}

- (void)session:(ARSession *)session didRemoveAnchors:(NSArray<__kindof ARAnchor*>*)anchors {
    if (self.unityARSessionDelegate != NULL) {
        [self.unityARSessionDelegate session:session didRemoveAnchors:anchors];
    }
}

- (void)session:(ARSession *)session didOutputCollaborationData:(ARCollaborationData *)data {
    if (self.unityARSessionDelegate != NULL) {
        [self.unityARSessionDelegate session:session didOutputCollaborationData:data];
    }
}

@end
