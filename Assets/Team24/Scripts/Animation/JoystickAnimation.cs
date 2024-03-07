using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class JoystickAnimation : MicrogameInputEvents
    {
        public float rotateSlerpSpeed = 5f;
        public float rotationAmountDegrees = 45f;

        void Update()
        {
            Vector2 dir = stick.normalized;
            Quaternion rot = Quaternion.Euler(dir.y * rotationAmountDegrees, 0f, -dir.x * rotationAmountDegrees);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * rotateSlerpSpeed);
        }
    }
}