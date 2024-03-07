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

        public AudioSource FailSFX;
        public AudioClip Fail_Sfx;

        public AudioSource SuccsessSFX;
        public AudioClip Succsess_Sfx;

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
            //Debug.Log("Cleaned " + amountCleaned * 100 + " percent of dirt.");

            // Not a very hard "victory"...
            if (amountCleaned < fThreshold)
                Failure();

            ClearCleanWindows();
            CalculateLetterGrade(amountCleaned);
            StartCoroutine(ScaleLetterGrade());
        }

        void ClearCleanWindows()
        {
            // Tell each panel to clear if they are cleaned
            foreach (EndPanel panel in endScreens)
                panel.GameEnded();
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

            // If you succeeded expectations...
            if (percentCleaned > aThreshold)
                grade = "A+";
            // ...or if you completely failed them...
            else if (percentCleaned < fThreshold)
                grade = "F";
            // Otherwise...
            else
            {
                percentCleaned = Mathf.Clamp(percentCleaned, fThreshold, aThreshold);
                // 0-1, F-A
                float fac = Mathf.InverseLerp(fThreshold, aThreshold, percentCleaned);
                int index = Mathf.CeilToInt(fac * LetterGrades.Length) - 1;
                grade = LetterGrades[index];
            }

            if (grade == "F"|| grade == "D"|| grade == "C")
                FailSFX.Play();
            
            else if (grade == "B"|| grade ==  "A")
                SuccsessSFX.Play();

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
    }
}