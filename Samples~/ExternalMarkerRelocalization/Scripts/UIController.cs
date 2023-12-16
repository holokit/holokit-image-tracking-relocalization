// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Yuchen Zhang <yuchenz27@outlook.com>
// SPDX-License-Identifier: MIT

using UnityEngine;
using TMPro;

namespace HoloInteractive.XR.ImageTrackingRelocalization.Samples.ExternalMarkerRelocalization
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private ImageTrackingStablizer m_RelocalizationManager;

        [SerializeField] private TMP_Text m_RelocalizationButtonText;

        public void ToggleRelocalization()
        {
            m_RelocalizationManager.IsRelocalizing = !m_RelocalizationManager.IsRelocalizing;
        }

        private void Update()
        {
            if (m_RelocalizationManager.IsRelocalizing)
                m_RelocalizationButtonText.text = "Stop Relocalization";
            else
                m_RelocalizationButtonText.text = "Start Relocalization";
        }
    }
}
