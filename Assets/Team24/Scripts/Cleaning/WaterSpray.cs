using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpray : MonoBehaviour
{
    public float lifetime;
    public AnimationCurve sizeMultOverLife;

    float currentLifetime;
    Vector3 startingSize;

    private void Start()
    {
        startingSize = transform.localScale;
    }

    private void Update()
    {
        currentLifetime += Time.deltaTime;
        if (currentLifetime > lifetime)
            Destroy(gameObject);
        else
        {
            // Update our scale over time
            float scale = sizeMultOverLife.Evaluate(currentLifetime / lifetime);
            transform.localScale = startingSize * scale;
        }
    }
}
