// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace HoloInteractive.XR.ImageTrackingRelocalization.iOS
{
    /// <summary>
    /// Partial class separating host and client code.
    /// </summary>
    public partial class NetworkImageTrackingStablizer : NetworkBehaviour
    {
        private void Start()
        {
            m_MarkerRenderer = GetComponent<MarkerRenderer>();

            Start_Host();
            Start_Client();
        }
    }
}
#endif
