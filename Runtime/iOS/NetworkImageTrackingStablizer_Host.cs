// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.Netcode;
using System;

namespace HoloInteractive.XR.ImageTrackingRelocalization.iOS
{
    public struct TimedCameraPose
    {
        public double Timestamp;
        public Matrix4x4 PoseMatrix;
    }

    /// <summary>
    /// Partial class separating host and client code; this file contains host-specific code.
    /// </summary>
    public partial class NetworkImageTrackingStablizer : NetworkBehaviour
    {
        /// <summary>
        /// The offset from the host's camera to its screen center. This is used by the client to correctly render the alignment marker.
        /// </summary>
        public readonly NetworkVariable<Vector3> HostCameraToScreenCenterOffset = new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        /// <summary>
        /// If you want to dynimcally set this offset for different devices, set this value properly before calling StartDisplayingMarker() function.
        /// </summary>
        public Vector3 CameraToMarkerOffset
        {
            set => m_CameraToMarkerOffset = value;
        }

        public bool IsDisplayingMarker => m_IsDisplayingMarker;

        [Header("Offset Vectors")]
        [Tooltip("The offset from the host's camera to the center of the displayed marker.")]
        [SerializeField] private Vector3 m_CameraToMarkerOffset;

        [Tooltip("The offset from the host's camera to its screen center. This is used by the client to correctly render the alignment marker.")]
        [SerializeField] private Vector3 m_CameraToScreenCenterOffset;

        [Header("Calibration Parameters")]
        [Tooltip("The maximum timestamp gap between the current time and a stored camera pose. Host does not store poses with timestamp earlier than this threshold.")]
        [SerializeField] private double m_TimedCameraPoseTimestampThreshold = 0.6;

        [Tooltip("The maximum acceptable timestamp deviation. We only process camera poses with timestamp deviation smaller than this threshold.")]
        [SerializeField] private double m_TimestampDeviationThreshold = 0.034;

        /// <summary>
        /// Stores a queue of camera poses with timestamps.
        /// </summary>
        private readonly Queue<TimedCameraPose> m_TimedCameraPoseQueue = new();

        private ARKitNativeProvider m_ARKitNativeProvider;

        private MarkerRenderer m_MarkerRenderer;

        private bool m_IsDisplayingMarker;

        public void StartDisplayingMarker()
        {
#if !UNITY_EDITOR
            m_ARKitNativeProvider = new();
            m_ARKitNativeProvider.OnARSessionUpdatedFrame += OnARSessionUpdatedFrame;
#endif

            m_MarkerRenderer.SpawnMarker();
            m_IsDisplayingMarker = true;
        }

        public void StopDisplayingMarker()
        {
#if !UNITY_EDITOR
            m_ARKitNativeProvider.OnARSessionUpdatedFrame -= OnARSessionUpdatedFrame;
            m_ARKitNativeProvider.Dispose();
            m_ARKitNativeProvider = null;
#endif

            m_MarkerRenderer.DestroyMarker();
            m_IsDisplayingMarker = false;
        }

        private void OnARSessionUpdatedFrame(double timestamp, Matrix4x4 matrix)
        {
            TimedCameraPose timedCameraPose = new()
            {
                Timestamp = timestamp,
                PoseMatrix = matrix
            };

            m_TimedCameraPoseQueue.Enqueue(timedCameraPose);
            double currentTime = NativeApi.GetSystemUptime();
            // Get rid of early poses
            while (m_TimedCameraPoseQueue.Count > 0 && currentTime - m_TimedCameraPoseQueue.Peek().Timestamp > m_TimedCameraPoseTimestampThreshold)
            {
                _ = m_TimedCameraPoseQueue.Dequeue();
            }
        }
    }
}
#endif
