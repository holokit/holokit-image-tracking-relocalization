// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#if UNITY_IOS
using UnityEngine;
using Unity.Netcode;

namespace HoloInteractive.XR.ImageTrackingRelocalization.iOS
{
    public class PlayerController : NetworkBehaviour
    {
        private void Update()
        {
            if (IsOwner)
            {
                transform.SetPositionAndRotation(Camera.main.transform.position, Camera.main.transform.rotation);
            }
        }
    }
}
#endif
