using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace team24
{
    public class ButtonAnimations : MicrogameInputEvents
    {
        public Transform topButton;
        public Transform bottomButton;
        public float lerpSpeed = 3;

        Vector3 restPos;

        private void Start()
        {
            restPos = transform.localPosition;
        }

        protected override void OnButton1Pressed(InputAction.CallbackContext context)
        {
            transform.localPosition = topButton.localPosition;
        }

        protected override void OnButton2Pressed(InputAction.CallbackContext context)
        {
            transform.localPosition = bottomButton.localPosition;
        }

        void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, restPos, lerpSpeed * Time.deltaTime);
        }
    }
}
