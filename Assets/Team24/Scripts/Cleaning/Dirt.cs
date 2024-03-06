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

        List<CPUArea> cpuAreas;


        private void Start()
        {
            CreateCPUAreas();
        }

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
            if (areaSubdivisions.x <= 0 || areaSubdivisions.y <= 0)
                throw new System.Exception("Dirt areaSubdivisions are invalid - cannot create CPUAreas!");

            cpuAreas = new List<CPUArea>();
            // Find out how big each area should be
            float xStep = dirtExtents.x / areaSubdivisions.x;
            float yStep = dirtExtents.y / areaSubdivisions.y;
            Vector2 size = new Vector2(xStep, yStep);
            Vector3 pos = transform.position - (Vector3)dirtExtents / 2f + (Vector3)size / 2f;

            for (float x = 0; x < dirtExtents.x; x += xStep)
            {
                for (float y = 0; y < dirtExtents.y; y += yStep)
                {
                    // Create and add each area
                    cpuAreas.Add(new CPUArea(new Vector3(x, y) + pos, size));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Same code as CreateCPUAreas. Draws said areas
            if (areaSubdivisions.x <= 0 || areaSubdivisions.y <= 0)
                return;

            float xStep = dirtExtents.x / areaSubdivisions.x;
            float yStep = dirtExtents.y / areaSubdivisions.y;
            Vector3 size = new Vector3(xStep, yStep, 0.5f);
            Vector3 pos = transform.position - (Vector3)dirtExtents / 2f + new Vector3(xStep, yStep) / 2f;
            Gizmos.color = Color.green;

            for (float x = 0; x < dirtExtents.x; x += xStep)
            {
                for (float y = 0; y < dirtExtents.y; y += yStep)
                {
                    Gizmos.DrawWireCube(new Vector3(x, y) + pos, size);
                }
            }
        }

        /// <summary>
        /// Defines an area of dirt on the CPU side
        /// </summary>
        class CPUArea
        {
            public Vector3 Min { get; private set; }
            public Vector3 Center { get; private set; }
            public Vector2 Size { get; private set; }

            public CPUArea(Vector3 corner, Vector2 size)
            {
                Min = corner;
                Size = size;
                Center = Min + (Vector3)size / 2f;
            }
        }
    }
}
