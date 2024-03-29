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
        public float arrivalTime = 2.0f;
        public float departureTime = 2.5f;

        [Space]
        public SqueegeeAnimation squeegee;
        public float delayBeforeSqueegeeAnim = 1.75f;

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

            // Queue this up
            Invoke(nameof(StartSqueegeeAnim), delayBeforeSqueegeeAnim);

            // Turn everything off
            SetComponents(false);

            // We want to get here
            target = transform.position;
            // ... and start up top
            start = topWaitingPoint.position;

            float time = 0;

            while (time < arrivalTime)
            {
                // Move towards our target
                time += Time.deltaTime;
                float fac = Mathf.Clamp01(time / arrivalTime);
                float interpFactor = arrivalCurve.Evaluate(fac);
                transform.position = Vector3.LerpUnclamped(start, target, interpFactor);
                yield return null;
            }

            // Turn everything back on
            SetComponents(true);
            rb.position = target;
            transform.position = target;
        }

        void StartSqueegeeAnim()
        {
            squeegee.PlayStartAnimation();
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
            start = transform.position;

            float time = 0;

            while (time < departureTime)
            {
                // Move towards our target
                time += Time.deltaTime;
                float fac = Mathf.Clamp01(time / departureTime);
                float interpFactor = departureCurve.Evaluate(fac);
                transform.position = Vector3.LerpUnclamped(start, target, interpFactor);
                yield return null;
            }

            transform.position = target;
        }

        void SetComponents(bool active)
        {
            scaffold.enabled = active;
            rb.isKinematic = !active;
            coll.enabled = active;
        }
    }
}
