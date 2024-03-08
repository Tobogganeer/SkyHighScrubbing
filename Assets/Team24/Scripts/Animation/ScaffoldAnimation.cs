using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    [RequireComponent(typeof(PhysicsScaffoldMotor))]
    public class ScaffoldAnimation : MonoBehaviour
    {
        public AnimationCurve arrivalCurve;
        public AnimationCurve departureCurve;
        public Transform topWaitingPoint;
        public float travelTime = 1.5f;

        PhysicsScaffoldMotor scaffold;
        Rigidbody rb;
        BoxCollider coll;

        Vector3 start;
        Vector3 target;

        private IEnumerator Start()
        {
            scaffold = GetComponent<PhysicsScaffoldMotor>();
            rb = GetComponent<Rigidbody>();
            coll = GetComponent<BoxCollider>();

            // Turn everything off
            SetComponents(false);

            // We want to get here
            target = transform.position;
            // ... and start up top
            start = topWaitingPoint.position;

            float time = 0;

            while (time < travelTime)
            {
                // Move towards our target
                time += Time.deltaTime;
                float fac = time / travelTime;
                float interpFactor = arrivalCurve.Evaluate(fac);
                transform.position = Vector3.LerpUnclamped(start, target, interpFactor);
                yield return null;
            }

            // Turn everything back on
            SetComponents(true);
        }

        public void LeaveScene()
        {
            StartCoroutine(Leave());
        }

        IEnumerator Leave()
        {
            // Turn them off
            SetComponents(false);

            // We want to get here
            target = topWaitingPoint.position;
            start = topWaitingPoint.position;

            float time = 0;

            while (time < travelTime)
            {
                // Move towards our target
                time += Time.deltaTime;
                float fac = time / travelTime;
                float interpFactor = departureCurve.Evaluate(fac);
                transform.position = Vector3.LerpUnclamped(start, target, interpFactor);
                yield return null;
            }
        }

        void SetComponents(bool active)
        {
            scaffold.enabled = active;
            rb.isKinematic = !active;
            coll.enabled = active;
        }
    }
}
