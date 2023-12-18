// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#import <Foundation/Foundation.h>

void HoloInteractiveImageTrackingRelocalization_NativeApi_CFRelease(void* ptr)
{
    if (ptr)
    {
        CFRelease(ptr);
    }
}

double HoloInteractiveImageTrackingRelocalization_NativeApi_getSystemUptime() {
    return [[NSProcessInfo processInfo] systemUptime];
}
