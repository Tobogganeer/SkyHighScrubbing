using System.Collections;
using System.Collections.Generic;
using team24;
using UnityEngine;

public class WaterParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem waterParticle;


    private void OnEnable()
    {
        Dirt.DirtCleaned += Dirt_DirtCleaned;
    }

    private void OnDisable()
    {
        Dirt.DirtCleaned -= Dirt_DirtCleaned;
    }

    private void Dirt_DirtCleaned(Vector3 position)
    {
        waterParticle.transform.position = position;
        waterParticle.Play();
    }
}
