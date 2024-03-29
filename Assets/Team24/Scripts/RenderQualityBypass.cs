using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Reflection;

namespace team24
{
    /// <summary>
    /// Cheats around the preset render settings (I want crispier shadows)
    /// </summary>
    public class RenderQualityBypass : MonoBehaviour
    {
        // ====================================================================================================================
        //
        // NOTE FOR COMPLIANCE TESTERS:
        // "I'll dig into this after lunch. Worst-case, I think the script you showed should be compliant." -Douglass Gregary
        // See "Possibility for better default shadow quality" on Teams #Support-Technologist
        // NOTE V2:
        // This script is no longer used.
        //
        // ====================================================================================================================


        public UniversalRenderPipelineAsset rpAsset;

        /*

        int oldCascades; // 1 lol
        bool oldSoftShadows; // false :/
        FieldInfo softShadowsField;
        // Name of the field in UniversalRenderPipelineAsset
        static readonly string m_SoftShadowsSupported_FieldName = "m_SoftShadowsSupported";

        private void Awake()
        {
            // We rippin out the reflection in here
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            softShadowsField = typeof(UniversalRenderPipelineAsset).GetField(m_SoftShadowsSupported_FieldName, flags);
        }

        private void OnEnable()
        {
            // Save the old settings
            oldCascades = rpAsset.shadowCascadeCount;
            oldSoftShadows = rpAsset.supportsSoftShadows;

            // Apply our devious HQ shadows
            rpAsset.shadowCascadeCount = 4;
            softShadowsField.SetValue(rpAsset, true);
        }

        private void OnDisable()
        {
            // Reset the settings back to how they were
            rpAsset.shadowCascadeCount = oldCascades;
            softShadowsField.SetValue(rpAsset, oldSoftShadows);
        }

        */
    }
}
