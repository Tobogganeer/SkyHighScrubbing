using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class GameEnd : MicrogameEvents
    {
        [Range(0f, 1f)]
        public float cleanThreshold = 0.85f;

        [Space]
        public ScaffoldMotor scaffolding;
        public GameObject window;
        public GameObject dirt;
        public GameObject[] endScreens;

        protected override void OnTimesUp()
        {
            float amountCleaned = Dirt.CalculateCleanedPercent();
            Debug.Log("Cleaned " + amountCleaned * 100 + " percent");
            if (amountCleaned >= cleanThreshold)
                Victory();
            else
                Failure();
        }

        void Victory()
        {
            // Hide the window (temporary, make clear later)
            window.SetActive(false);
            dirt.SetActive(false);
            // Show a random end screen
            int endScreen = Random.Range(0, endScreens.Length);
            for (int i = 0; i < endScreens.Length; i++)
                endScreens[i].SetActive(i == endScreen);
            scaffolding.gameObject.SetActive(false);
        }

        void Failure()
        {
            // Turn it off to avoid them interfering
            scaffolding.enabled = false;

            // Launch them into space (temporary lol)
            Rigidbody rb = scaffolding.gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(1000f, Vector3.down * 5, 20f);
            rb.AddTorque(new Vector3(Random.value * 100f, Random.value * 100f, Random.value * 100f));
        }
    }
}