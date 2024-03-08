using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    [RequireComponent(typeof(SqueegeeMotor))]
    public class SqueegeeAnimation : MonoBehaviour
    {
        public Vector3 startAnimOffset = new Vector3(1.5f, 0.8f);
        public float extendRetractTime = 0.3f;
        public int numSweeps = 2;

        [Space]
        public float waitAfterWater = 0.75f;
        public WaterSprayer water;
        public int numWaterSprays = 4;

        SqueegeeMotor motor;
        Transform target => motor.squeegeeObj.transform;

        void Start()
        {
            motor = GetComponent<SqueegeeMotor>();
        }

        public void PlayStartAnimation()
        {
            StartCoroutine(StartAnimation());
        }

        IEnumerator StartAnimation()
        {
            // Spray some water and wipe it down as a tutorial
            // No controls rn
            motor.enabled = false;

            // Spawn water and wait a sec
            for (int i = 0; i < numWaterSprays; i++)
                water.Spawn(water.rightSprayPos.position);
            yield return new WaitForSeconds(waitAfterWater);

            Vector3 startPos = target.position;
            Quaternion startRotation = target.rotation;

            Vector3 endPos = startPos + startAnimOffset;
            Quaternion endRotation = Quaternion.LookRotation(Vector3.forward, (endPos - startPos).normalized);

            for (int i = 0; i < numSweeps; i++)
            {
                float time = 0;
                // Extend
                while (time < extendRetractTime)
                {
                    time += Time.deltaTime;
                    float fac = time / extendRetractTime;
                    target.position = Vector3.Lerp(startPos, endPos, fac);
                    target.rotation = Quaternion.Slerp(startRotation, endRotation, fac);
                    yield return null;
                }
                // Retract
                while (time > 0)
                {
                    time -= Time.deltaTime;
                    float fac = time / extendRetractTime;
                    target.position = Vector3.Lerp(startPos, endPos, fac);
                    target.rotation = Quaternion.Slerp(startRotation, endRotation, fac);
                    yield return null;
                }
            }

            // Game on
            target.position = startPos;
            target.rotation = startRotation;
            motor.enabled = true;
        }
    }
}
