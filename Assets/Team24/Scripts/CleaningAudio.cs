using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    [RequireComponent(typeof(AudioSource))]
    public class CleaningAudio : MicrogameInputEvents
    {
        public SoundCollection sounds;
        public float minDelay = 0.2f;

        AudioSource source;

        float timer;
        Vector2 lastStick;


        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            timer -= Time.deltaTime;

            if (lastStick != stick)
            {
                lastStick = stick;
                if (timer <= 0)
                {
                    PlaySound();
                    timer = minDelay;
                }
            }
        }

        void PlaySound()
        {
            source.PlayOneShot(sounds.SelectNonRepeating());
        }
    }
}
