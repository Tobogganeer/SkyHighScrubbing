using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class WaterSpray : MonoBehaviour
    {
        public float lifetime;
        public AnimationCurve sizeMultOverLife;
        public float drag = 1f;
        public float gravity = 1f;

        float currentLifetime;
        Vector3 startingSize;
        Vector3 velocity;

        private void Start()
        {
            startingSize = transform.localScale;
        }

        public void Init(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        private void Update()
        {
            currentLifetime += Time.deltaTime;
            if (currentLifetime > lifetime)
            {
                Destroy(gameObject);
                return;
            }

            // Update our scale over time
            float scale = sizeMultOverLife.Evaluate(currentLifetime / lifetime);
            transform.localScale = startingSize * scale;

            // Apply gravity and drag
            velocity += Vector3.down * gravity * Time.deltaTime;
            velocity = velocity * (1f - Time.deltaTime * drag);
            transform.position += velocity * Time.deltaTime;
        }
    }
}
