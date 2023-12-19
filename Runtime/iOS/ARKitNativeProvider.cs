// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace HoloInteractive.XR.ImageTrackingRelocalization.iOS
{
    public class ARKitNativeProvider: IDisposable
    {
        IntPtr m_Ptr;

        static Dictionary<IntPtr, ARKitNativeProvider> s_Providers = new();

        public ARKitNativeProvider()
        {
            m_Ptr = Init_Native();
            s_Providers[m_Ptr] = this;
            RegisterCallbacks();
            InterceptUnityARSessionDelegate();
        }

        public void Dispose()
        {
            if (m_Ptr != IntPtr.Zero)
            {
                RestoreUnityARSessionDelegate();
                s_Providers.Remove(m_Ptr);
                NativeApi.CFRelease(ref m_Ptr);
                m_Ptr = IntPtr.Zero;
            }
        }

        private void RegisterCallbacks()
        {
            RegisterCallbacks_Native(m_Ptr, OnARSessionUpdatedFrameDelegate);
        }

        private void InterceptUnityARSessionDelegate()
        {
            var xrSessionSubsystem = GetLoadedXRSessionSubsystem();
            if (xrSessionSubsystem != null)
            {
                InterceptUnityARSessionDelegate_Native(m_Ptr, xrSessionSubsystem.nativePtr);
            }
        }

        private void RestoreUnityARSessionDelegate()
        {
            var xrSessionSubsystem = GetLoadedXRSessionSubsystem();
            if (xrSessionSubsystem != null)
            {
                RestoreUnityARSessionDelegate_Native(m_Ptr, xrSessionSubsystem.nativePtr);
            }
        }

        private static XRSessionSubsystem GetLoadedXRSessionSubsystem()
        {
            List<XRSessionSubsystem> xrSessionSubsystems = new();
            SubsystemManager.GetSubsystems(xrSessionSubsystems);
            foreach (var subsystem in xrSessionSubsystems)
            {
                return subsystem;
            }
            Debug.Log("[ARKitNativeProvider] Failed to get loaded xr session subsystem");
            return null;
        }

        public event Action<double, Matrix4x4> OnARSessionUpdatedFrame;

        [DllImport("__Internal", EntryPoint = "HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_init")]
        private static extern IntPtr Init_Native();

        [DllImport("__Internal", EntryPoint = "HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_registerCallbacks")]
        private static extern void RegisterCallbacks_Native(IntPtr self, Action<IntPtr, double, IntPtr> onARSessionUpdatedFrame);

        [DllImport("__Internal", EntryPoint = "HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_interceptUnityARSessionDelegate")]
        private static extern void InterceptUnityARSessionDelegate_Native(IntPtr self, IntPtr sessionPtr);

        [DllImport("__Internal", EntryPoint = "HoloInteractiveImageTrackingRelocalization_ARKitNativeProvider_restoreUnityARSessionDelegate")]
        private static extern void RestoreUnityARSessionDelegate_Native(IntPtr self, IntPtr sessionPtr);

        [AOT.MonoPInvokeCallback(typeof(Action<IntPtr, double, IntPtr>))]
        private static void OnARSessionUpdatedFrameDelegate(IntPtr providerPtr, double timestamp, IntPtr matrixPtr)
        {
            if (s_Providers.TryGetValue(providerPtr, out ARKitNativeProvider provider))
            {
                if (provider.OnARSessionUpdatedFrame == null)
                    return;

                float[] matrixData = new float[16];
                Marshal.Copy(matrixPtr, matrixData, 0, 16);
                Matrix4x4 matrix = new();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        matrix[i, j] = matrixData[(4 * i) + j];
                    }
                }
                provider.OnARSessionUpdatedFrame?.Invoke(timestamp, matrix);
            }
        }
    }
}
#endif
