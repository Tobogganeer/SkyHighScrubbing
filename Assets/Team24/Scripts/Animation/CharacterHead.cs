using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    /// <summary>
    /// Makes characters look towards a point
    /// </summary>
    public class CharacterHead : MonoBehaviour
    {
        public Transform lookTarget;
        public float slerpSpeed = 10f;
        public float maxAngle = 30f;
        public Vector3 offset;

        private void LateUpdate()
        {
            // Find what way we should face
            Vector3 direction = lookTarget.position - transform.position;
            Vector3 rotatedDirection = Quaternion.Euler(offset) * direction;
            Quaternion rot = Quaternion.LookRotation(rotatedDirection);
            //Quaternion.ang
        }
    }
}
