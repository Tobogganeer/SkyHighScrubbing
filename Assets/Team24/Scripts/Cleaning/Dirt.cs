using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class Dirt : MonoBehaviour
    {
        public RenderTexture cleanMask;
        [Range(0, 255)]
        public int clearThreshold = 15;

        /// <summary>
        /// Calculates what percent of the surface has been cleaned.
        /// </summary>
        /// <remarks>This function is expensive!</remarks>
        /// <returns></returns>
        public float CalculateCleanedPercent()
        {
            Texture2D buffer = new Texture2D(cleanMask.width, cleanMask.height, TextureFormat.R8, false);
            RenderTexture.active = cleanMask;

            // Read the mask into a texture
            buffer.ReadPixels(new Rect(0, 0, cleanMask.width, cleanMask.height), 0, 0);
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
                if (colours[i].r < clearThreshold)
                {
                    clearPixels++;
                }
            }

            return (float)clearPixels / colours.Length;
        }
    }
}
