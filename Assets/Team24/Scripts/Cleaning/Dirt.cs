using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class Dirt : MonoBehaviour
    {
        static List<Dirt> allDirt = new List<Dirt>();

        private void OnEnable()
        {
            allDirt.Add(this);
        }
        private void OnDisable()
        {
            allDirt.Remove(this);
        }


        [Header("GPU dirt")]
        public DirtGPUManager gpuManager;
        [Range(0, 255)]
        public int clearThreshold = 240;

        [Header("CPU dirt")]
        public Vector2 dirtExtents;
        public Vector2Int areaSubdivisions;

        List<CPUArea> cpuAreas;


        private void Start()
        {
            CreateCPUAreas();
        }

        /// <summary>
        /// Calculates what percent of all surfaces have been cleaned.
        /// </summary>
        /// <remarks>This function is expensive!</remarks>
        /// <returns></returns>
        public static float CalculateTotalCleanedPercent()
        {
            float percent = 0;
            // Prevent div by 0
            if (allDirt.Count == 0)
                return 0f;

            foreach (Dirt dirt in allDirt)
                percent += dirt.CalculateCleanedPercent() / allDirt.Count;

            return percent;
        }

        /// <summary>
        /// Calculates what percent of the surface has been cleaned.
        /// </summary>
        /// <remarks>This function is expensive!</remarks>
        /// <returns></returns>
        public float CalculateCleanedPercent()
        {
            RenderTexture rt = gpuManager.cleanedTex;
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
                if (colours[i].r > clearThreshold)
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

        /// <summary>
        /// Cleans the position closest to <paramref name="position"/>.
        /// </summary>
        /// <param name="position"></param>
        public static void Clean(Vector3 position)
        {
            foreach (Dirt dirt in allDirt)
                dirt.GetClosestArea(position).IsClean = true;
        }

        /// <summary>
        /// Returns true if the cpu-area closest to <paramref name="position"/> is clean.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool IsClean(Vector3 position)
        {
            bool isClean = false;
            foreach (Dirt dirt in allDirt)
                isClean |= dirt.GetClosestArea(position).IsClean;

            return isClean;
        }

        CPUArea GetClosestArea(Vector3 position)
        {
            CPUArea closest = null;
            float sqrDist = float.PositiveInfinity;

            // Very inefficient! Looping hundreds of times...
            foreach (CPUArea area in cpuAreas)
            {
                float currentSqrDist = (area.Center - position).sqrMagnitude;
                if (currentSqrDist < sqrDist)
                {
                    closest = area;
                    sqrDist = currentSqrDist;
                }
            }

            return closest;
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
            public bool IsClean { get; set; }

            public CPUArea(Vector3 corner, Vector2 size)
            {
                Min = corner;
                Size = size;
                Center = Min + (Vector3)size / 2f;
                IsClean = false;
            }
        }
    }
}
