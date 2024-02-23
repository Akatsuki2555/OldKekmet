using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldKekmet
{
    public class SoundImprovement : MonoBehaviour
    {
        AudioSource throttle, noThrottle;

        public float threshold;


        void Start()
        {
            noThrottle = transform.GetChild(21).GetComponent<AudioSource>();
            throttle = transform.GetChild(22).GetComponent<AudioSource>();
        }

        void Update()
        {
            if (throttle.volume > threshold) noThrottle.Stop();
            else if (!noThrottle.isPlaying) noThrottle.Play();
        }
    }
}
