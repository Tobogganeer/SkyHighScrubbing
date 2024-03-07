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
        [Range(0f, 1f)]
        public float panelCleanThreshold = 0.65f;

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
        static readonly int _Alpha = Shader.PropertyToID("_Alpha");
        static readonly int _Metallic = Shader.PropertyToID("_Metallic");
        static readonly int _Smoothness = Shader.PropertyToID("_Smoothness");
        const float EndOfRoundTime = 3.5f;
        const float WindowFadeTime = 1.0f;


        private void Start()
        {
            // Make the text empty
            letterGradeText.text = "";
            // Rotate it randomly a small amount
            letterGradeText.rectTransform.Rotate(0, 0, Random.Range(-letterGradeRotation, letterGradeRotation));

            // Get a copy of each window's material
            foreach (EndPanel panel in endScreens)
                panel.material = panel.window.material;
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

            foreach (EndPanel panel in endScreens)
            {
                // Check if each panel is cleaned
                float cleaned = panel.dirt.CalculateCleanedPercent();
                if (cleaned > panelCleanThreshold)
                    StartCoroutine(ClearPanel(panel));
            }
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

        IEnumerator ClearPanel(EndPanel panel)
        {
            // Turn on a random end scene
            int endScreen = Random.Range(0, panel.endScreens.Length);
            for (int i = 0; i < panel.endScreens.Length; i++)
                panel.endScreens[i].SetActive(i == endScreen);

            Material mat = panel.material;
            float smoothness = mat.GetFloat(_Smoothness);
            float metallic = mat.GetFloat(_Metallic);

            float fade = 0;
            while (fade < WindowFadeTime)
            {
                // Slowly make the window transparent
                fade += Time.deltaTime;
                float highToLowFac = Mathf.Clamp01(1f - fade / WindowFadeTime);

                mat.SetFloat(_Alpha, highToLowFac);
                mat.SetFloat(_Smoothness, smoothness * highToLowFac);
                mat.SetFloat(_Metallic, metallic * highToLowFac);

                yield return null;
            }
        }

        private void OnDestroy()
        {
            // Destroy the copies of each window's material
            foreach (EndPanel panel in endScreens)
                DestroyImmediate(panel.material);
        }


        [System.Serializable]
        public class EndPanel
        {
            public Dirt dirt;
            public MeshRenderer window;
            public GameObject[] endScreens;

            [HideInInspector]
            public Material material;
        }
    }
}