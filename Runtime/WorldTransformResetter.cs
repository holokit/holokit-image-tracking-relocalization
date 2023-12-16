// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

using UnityEngine;

namespace HoloInteractive.XR.ImageTrackingRelocalization.Samples.ExternalMarkerRelocalization
{
    public class WorldTransformerResetter : MonoBehaviour
    {
        [SerializeField] private Transform m_RootTransform;

        public void OnTrackedImageStablized(Vector3 position, Quaternion rotation)
        {
            m_RootTransform.position = position;
            Quaternion finalRotation = rotation * Quaternion.Euler(90f, 0f, 0f);
            m_RootTransform.rotation = Quaternion.Euler(0f, finalRotation.eulerAngles.y, 0f);
        }
    }
}
