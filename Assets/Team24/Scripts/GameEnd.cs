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
        public PhysicsScaffoldMotor scaffolding;
        public GameObject scaffoldWires;
        public GameObject window;
        public GameObject dirt;
        public GameObject[] endScreens;

        protected override void OnTimesUp()
        {
            float amountCleaned = Dirt.CalculateTotalCleanedPercent();
            Debug.Log("Cleaned " + amountCleaned * 100 + " percent of dirt.");
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
            // Turn off controls to avoid it interfering
            scaffolding.enabled = false;

            // Deparent the wires - the scaffolding will fall away from them
            scaffoldWires.transform.SetParent(null);

            // Launch them into space (temporary lol)
            Rigidbody rb = scaffolding.GetComponent<Rigidbody>();
            rb.useGravity = true;
            //rb.AddExplosionForce(1000f, Vector3.down * 5, 20f);
            // Send it randomly to the side
            float force = 2f;
            rb.AddForce(new Vector3(Random.Range(-force, force), 0), ForceMode.VelocityChange);
            rb.AddTorque(new Vector3(0f, 0f, (Random.value * 2f - 1f) * 10f));

            // Turn off collisions (let them fall out of map)
            scaffolding.GetComponent<BoxCollider>().enabled = false;
        }
    }
}