using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace team24
{
    public class WaterSprayer : MicrogameInputEvents
    {
        public GameObject[] waterSprayPrefabs;
        public float cooldown = 1.5f;
        public Vector2 velocityRange = new Vector2(0.5f, 2f);
        public float bonusVerticalVelocity = 1.75f;
        public Vector2 scaleRange = new Vector2(0.5f, 1.25f);

        [Space]
        public Transform leftSprayPos;
        public Transform rightSprayPos;

        float leftCooldown;
        float rightCooldown;

        private void Update()
        {
            leftCooldown -= Time.deltaTime;
            rightCooldown -= Time.deltaTime;
        }

        protected override void OnButton1Pressed(InputAction.CallbackContext context)
        {
            if (leftCooldown <= 0)
            {
                leftCooldown = cooldown;
                Spawn(leftSprayPos.position);
            }
        }

        protected override void OnButton2Pressed(InputAction.CallbackContext context)
        {
            if (rightCooldown <= 0)
            {
                rightCooldown = cooldown;
                Spawn(rightSprayPos.position);
            }
        }

        void Spawn(Vector3 position)
        {
            // Make water face towards camera
            Quaternion rot = Quaternion.LookRotation(Vector3.up, Vector3.back);
            GameObject spray = Instantiate(waterSprayPrefabs[Random.Range(0, waterSprayPrefabs.Length)], position, rot);
            spray.transform.Rotate(new Vector3(0, Random.Range(0f, 360f), 0), Space.Self);
            spray.transform.localScale *= Random.Range(scaleRange.x, scaleRange.y);

            // Apply a random velocity
            Vector3 randomVelocity = Random.insideUnitCircle.normalized * Random.Range(velocityRange.x, velocityRange.y);
            randomVelocity += Vector3.up * bonusVerticalVelocity;
            spray.GetComponent<WaterSpray>().Init(randomVelocity);
        }
    }
}
