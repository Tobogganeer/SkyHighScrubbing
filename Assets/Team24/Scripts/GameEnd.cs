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
        public GameObject scaffolding;
        public GameObject[] endScreens;

        protected override void OnTimesUp()
        {
            float amountCleaned = Dirt.CalculateCleanedPercent();
            if (amountCleaned >= cleanThreshold)
                Victory();
            else
                Failure();
        }

        void Victory()
        {

        }

        void Failure()
        {

        }
    }
}