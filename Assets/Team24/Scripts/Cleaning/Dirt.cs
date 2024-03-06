using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class Dirt : MonoBehaviour
    {
        private static Dirt instance;
        private void Awake()
        {
            instance = this;
        }

        [Header("GPU dirt")]
        public RenderTexture cleanMask;
        [Range(0, 255)]
        public int clearThreshold = 15;

        [Header("CPU dirt")]
        public Vector2 dirtExtents;
        public Vector2Int areaSubdivisions;

        /// <summary>
        /// Calculates what percent of the surface has been cleaned.
        /// </summary>
        /// <remarks>This function is expensive!</remarks>
        /// <returns></returns>
        public static float CalculateCleanedPercent()
        {
            RenderTexture rt = instance.cleanMask;
            Texture2D buffer = new Texture2D(rt.width, rt.height, TextureFormat.R8, false);
            RenderTexture.active = rt;

            // Read the mask into a texture
            buffer.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            buffer.filterMode = FilterMode.Bilinear;

            RenderTexture.active = null;

            // Expensive and yucky
            Color32[] colours = buffer.GetPixels32();

            // Get rid of the texture
            DestroyImmediate(buffer);

            // Check how many are empty/clear
            int clearPixels = 0;
            for (int i = 0; i < colours.Length; i++)
            {
                if (colours[i].r < instance.clearThreshold)
                {
                    clearPixels++;
                }
            }

            return (float)clearPixels / colours.Length;
        }


        private void CreateCPUAreas()
        {

        }

        /// <summary>
        /// Defines an area of dirt on the CPU side
        /// </summary>
        class CPUArea
        {
            public Vector2 Min { get; private set; }
            public Vector2 Center { get; private set; }
            public Vector2 Size { get; private set; }

            public CPUArea(Vector2 corner, Vector2 size)
            {
                Min = corner;
                Size = size;
                Center = Min + size / 2f;
            }
        }
    }
}
