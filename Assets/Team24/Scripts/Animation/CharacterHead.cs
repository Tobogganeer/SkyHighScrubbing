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

        Vector3 baseDirection;
        Quaternion baseRotation;

        private void Start()
        {
            baseDirection = transform.forward;
            baseRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            // Find what way we should face
            Vector3 direction;
            if (lookTarget != null)
                direction = lookTarget.position - transform.position;
            else
                direction = baseDirection;
            Vector3 rotatedDirection = Quaternion.Euler(offset) * direction;
            
            // Clamp the angle of said rotation
            Quaternion delta = Quaternion.FromToRotation(baseDirection, rotatedDirection);
            delta.ToAngleAxis(out float angle, out Vector3 axis);
            // Add the delta to our base rotation
            Quaternion rotation = baseRotation * Quaternion.AngleAxis(Mathf.Clamp(angle, -maxAngle, maxAngle), axis);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * slerpSpeed);

            //Quaternion.ang
        }
    }
}
