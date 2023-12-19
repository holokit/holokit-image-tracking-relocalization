// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.Netcode;

namespace HoloInteractive.XR.ImageTrackingRelocalization.iOS
{
    /// <summary>
    /// Partial class separating host and client code; this file contains client-specific code.
    /// </summary>
    public partial class NetworkImageTrackingStablizer : NetworkBehaviour
    {
        private ARTrackedImageManager m_ARTrackedImageManager;

        private void Start_Client()
        {
            m_ARTrackedImageManager = FindFirstObjectByType<ARTrackedImageManager>();
            if (m_ARTrackedImageManager == null)
            {
                Debug.LogWarning("[NetworkImageTrackingStablizer] Failed to find ARTrackedImageManager in the scene.");
                return;
            }
            m_ARTrackedImageManager.enabled = false;
        }

        public void StartTrackingMarker()
        {

        }

        public void StopTrackingMarker()
        {

        }
    }
}
#endif
