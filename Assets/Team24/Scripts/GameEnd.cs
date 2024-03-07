using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace team24
{
    public class GameEnd : MicrogameEvents
    {
        [Range(0f, 1f)]
        public float aThreshold = 0.75f;
        [Range(0f, 1f)]
        public float fThreshold = 0.25f;

        [Space]
        public PhysicsScaffoldMotor scaffolding;
        public GameObject scaffoldWires;

        [Space]
        public EndPanel[] endScreens;

        [Space]
        public TextMeshProUGUI letterGradeText;
        public AnimationCurve letterGradeSize;
        public float letterGradeRotation = 5f;

        static readonly string[] LetterGrades = { "F", "D", "C", "B", "A" };
        const float EndOfRoundTime = 3.5f;


        private void Start()
        {
            // Make the text empty
            letterGradeText.text = "";
            // Rotate it randomly a small amount
            letterGradeText.rectTransform.Rotate(0, 0, Random.Range(-letterGradeRotation, letterGradeRotation));
        }

        protected override void OnTimesUp()
        {
            float amountCleaned = Dirt.CalculateTotalCleanedPercent();
            Debug.Log("Cleaned " + amountCleaned * 100 + " percent of dirt.");

            // Not a very hard "victory"...
            if (amountCleaned >= fThreshold)
                Victory();
            else
                Failure();

            CalculateLetterGrade(amountCleaned);
            StartCoroutine(ScaleLetterGrade());
        }

        void Victory()
        {
            // Hide the window (temporary, make clear later)
            //window.SetActive(false);
            //dirt.SetActive(false);
            //// Show a random end screen
            //int endScreen = Random.Range(0, endScreens.Length);
            //for (int i = 0; i < endScreens.Length; i++)
            //    endScreens[i].SetActive(i == endScreen);
            //scaffolding.gameObject.SetActive(false);
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

        void CalculateLetterGrade(float percentCleaned)
        {
            string grade;

            // If you succeeded expectations
            if (percentCleaned > aThreshold)
                grade = "A+";
            // Otherwise...
            else
            {
                percentCleaned = Mathf.Clamp(percentCleaned, fThreshold, aThreshold);
                // 0-1, F-A
                float fac = Mathf.InverseLerp(fThreshold, aThreshold, percentCleaned);
                int index = Mathf.CeilToInt(fac * LetterGrades.Length) - 1;
                grade = LetterGrades[index];
            }

            letterGradeText.text = grade;
        }

        IEnumerator ScaleLetterGrade()
        {
            float lifeTime = 0;
            while (lifeTime < EndOfRoundTime)
            {
                lifeTime += Time.deltaTime;
                letterGradeText.rectTransform.localScale = Vector3.one * letterGradeSize.Evaluate(lifeTime / EndOfRoundTime);
                yield return null;
            }
        }

        [System.Serializable]
        public class EndPanel
        {
            public Dirt dirt;
            public MeshRenderer window;
            public GameObject[] endScreens;
        }
    }
}