using System.Collections;
using System.Collections.Generic;
using team24;
using UnityEngine;

namespace team24
{
    public class EndPanel : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float cleanThreshold = 0.75f;
        [Range(0f, 1f)]
        public float clearedMidGameLimit = 0.25f; // Go a bit transparent to let the player know the window is clean

        [Space]
        public Dirt dirt;
        public MeshRenderer window;
        public GameObject[] endScreens;

        Material material;

        [HideInInspector]
        public float clearAmount = 0f;

        static readonly int _Alpha = Shader.PropertyToID("_Alpha");
        static readonly int _Metallic = Shader.PropertyToID("_Metallic");
        static readonly int _Smoothness = Shader.PropertyToID("_Smoothness");

        float smoothness;
        float metallic;

        bool cleared = false;
        bool gameOver = false;

        float clearSpeed = 0.2f; // Clear percent/s

        float checkTimer;
        const float CleanCheckInterval = 0.5f; // Check twice per second (expensive call)

        private void Start()
        {
            // Turn on a random end scene
            int endScreen = Random.Range(0, endScreens.Length);
            for (int i = 0; i < endScreens.Length; i++)
                endScreens[i].SetActive(i == endScreen);

            // Store the initial window values
            smoothness = material.GetFloat(_Smoothness);
            metallic = material.GetFloat(_Metallic);

            // Randomize starting time so windows will clear on different frames
            checkTimer = Random.Range(0f, CleanCheckInterval);
        }

        private void Update()
        {
            // If we aren't clean yet, check how much dirt we have
            if (!cleared)
            {
                checkTimer -= Time.deltaTime;
                if (checkTimer < 0f)
                {
                    checkTimer = CleanCheckInterval;
                    float cleanPercent = dirt.CalculateCleanedPercent();
                    if (cleanPercent > cleanThreshold)
                    {
                        cleared = true;
                    }
                }
            }
            else
            {
                clearAmount += Time.deltaTime * clearSpeed;
                // Only go a bit clear if the game isn't over
                if (!gameOver && clearAmount > clearedMidGameLimit)
                    clearAmount = clearedMidGameLimit;

                // How much of the window should be showing
                float fac = Mathf.Clamp01(1f - clearAmount);

                material.SetFloat(_Alpha, fac); // Opaque when not clear
                material.SetFloat(_Smoothness, smoothness * fac); // Shiny when not clear
                material.SetFloat(_Metallic, metallic * fac);
            }
        }

        public void GameEnded()
        {
            gameOver = true;
            clearSpeed = 0.65f; // Clear a bit faster
        }



        private void OnEnable()
        {
            // Get a copy of the window's material
            material = window.material;
        }

        private void OnDestroy()
        {
            // Destroy the copied window material
            DestroyImmediate(material);
        }
    }
}