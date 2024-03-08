using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class SqueegeeHead : MonoBehaviour
    {
        // This class makes a trail of gameobjects as this object moves
        // This is to prevent gaps in the squeegee due to moving too far
        // in between frames.

        public int lerpCopies = 20;
        public GameObject squeegeePrefab;
        public float cleaningPower = 10f;
        public float waterCleaningMultiplier = 4f;

        List<Transform> copies = new List<Transform>();
        Vector3 lastPosition;
        Quaternion lastRotation;

        // Static accessor for the CleanerManager
        public static float CleaningPower { get; private set; }
        public static float WaterCleaningMultiplier { get; private set; }

        private void Start()
        {
            lastPosition = transform.position;
            lastRotation = transform.rotation;

            // Spawn the copies of the brush
            for (int i = 0; i < lerpCopies; i++)
                copies.Add(Instantiate(squeegeePrefab, transform).transform);
        }

        private void LateUpdate()
        {
            PositionCopies();
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            Dirt.Clean(transform.position); // Clean on the CPU side too

            CleaningPower = cleaningPower;
            WaterCleaningMultiplier = waterCleaningMultiplier;
        }

        void PositionCopies()
        {
            Vector3 start = lastPosition;
            Vector3 end = transform.position;

            Quaternion startRot = lastRotation;
            Quaternion endRot = transform.rotation;

            for (int i = 0; i < copies.Count; i++)
            {
                // Move each copy a step of the way from our last-current position
                float fac = (float)i / copies.Count;
                copies[i].position = Vector3.Lerp(start, end, fac);
                copies[i].rotation = Quaternion.Slerp(startRot, endRot, fac);
            }
        }
    }
}