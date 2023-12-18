// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

namespace HoloInteractive.XR.ImageTrackingRelocalization.iOS
{
    internal static class NativeApi
    {
        public static void CFRelease(ref IntPtr ptr)
        {
            CFRelease_Native(ptr);
            ptr = IntPtr.Zero;
        }

        public static double GetSystemUptime()
        {
            return GetSystemUptime_Native();
        }

        [DllImport("__Internal", EntryPoint = "HoloInteractiveImageTrackingRelocalization_NativeApi_CFRelease")]
        private static extern void CFRelease_Native(IntPtr ptr);

        [DllImport("__Internal", EntryPoint = "HoloInteractiveImageTrackingRelocalization_NativeApi_getSystemUptime")]
        private static extern double GetSystemUptime_Native();
    }
}
#endif
